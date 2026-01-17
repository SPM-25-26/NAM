using DataInjection.Qdrant.Data;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using nam.Server.Services.Interfaces.Chatbot;
using System.Text.RegularExpressions;

namespace nam.Server.Services.Implemented.Chatbot
{
    public class ChatbotService(
        Serilog.ILogger logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IChatCompletionService chatService,
        IEmbeddingGenerator<string, Embedding<float>> embedder,
        VectorStoreCollection<Guid, POIEntity> store
        ) : IChatbotService
    {
        private HttpClient client = httpClientFactory.CreateClient("entities-api");

        private string SystemPrompt => """
            Sei un assistente AI esperto in viaggi e accoglienza turistica. La tua missione è fornire consigli su **Luoghi, Eventi e Raccomandazioni** trasformando dati tecnici in esperienze memorabili. Non sei un semplice motore di ricerca, ma un consulente che conosce i gusti dell'utente.
            **Fonti di Conoscenza:**
                1. **Dataset Ingerito (RAG):** Utilizza esclusivamente i dati forniti nel contesto per descrivere POI ed eventi. Se un'informazione manca, non inventarla.
                2. **Profilo Utente:** Adatta ogni risposta alle preferenze, al budget, allo stile di viaggio e alle necessità (es. mobilità ridotta, viaggi con bambini) descritte nel profilo.
            
            **Linee Guida per la Risposta:**
                * **Pertinenza:** Seleziona dal dataset solo i risultati che matchano con il Profilo Utente. Se l'utente ama l'arte moderna, dai priorità a quella rispetto ai monumenti storici, anche se entrambi sono presenti nel contesto.
                * **Contextual Linking:** Collega i luoghi agli eventi. (Esempio: "Visto che ti piace il jazz [Profilo] e c'è un concerto stasera [Evento], ti consiglio di cenare al Ristorante X che si trova proprio accanto [Luogo]").
                * **Trasparenza:** Se raccomandi qualcosa che si discosta leggermente dal profilo, spiega il perché (es. "Sappiamo che preferisci i musei, ma questo evento all'aperto è unico per la data del tuo soggiorno").
            
            **Formato di Output (Scannabile):**
                * **Il Tuo Match Perfetto:** Evidenzia il suggerimento migliore in base al profilo.
                * **Dettagli Pratici:** Usa tabelle o liste per orari, prezzi e location.
                * **Perché te lo consiglio:** Una breve frase che collega il POI/Evento a un interesse specifico dell'utente.
            
            **Vincoli:**
                * Non citare mai "il dataset", "il contesto" o "il file caricato".
                * Sii caloroso, professionale e d'ispirazione.
                * Usa un linguaggio naturale, evitando elenchi robotici.
            """;

        private string PromptTemplate => """
            ### PROFILO UTENTE (Questionario)
            Di seguito sono riportate le preferenze dell'utente raccolte tramite il questionario:
            {{UserQuestionaire}}
            ---

            ### DATI RECUPERATI (Punti di Interesse ed Eventi)
            Ecco i risultati pertinenti trovati nel nostro database per la zona richiesta:
            {{POIList}}

            ---

            ### RICHIESTA DELL'UTENTE
            "{{UserPrompt}}"

            ---

            ### ISTRUZIONI DI ELABORAZIONE
                1. **VALUTAZIONE DELLA PERTINENZA (FALLBACK):** Prima di rispondere, valuta se la "Richiesta dell'Utente" è pertinente al contesto turistico (viaggi, consigli su luoghi, eventi, cultura, cibo o logistica locale).
                
                    - **Se NON è pertinente:** Rispondi in modo cortese spiegando che il tuo ruolo è quello di assistente turistico specializzato e che non puoi fornire assistenza su temi diversi. Invita l'utente a farti una domanda sulla destinazione o sui suoi interessi di viaggio.
                    
                    - **Se è pertinente:** Procedi con il punto 2.
                    
                2. **PERSONALIZZAZIONE:** Seleziona esclusivamente i POI/Eventi dalla lista fornita che meglio si adattano al profilo dell'utente. Spiega chiaramente perché ogni scelta è rilevante rispetto alle risposte del questionario.
                
                3. **TONO DI VOCE:** Mantieni un tono empatico, utile e da "insider" locale.
            """;

        public async Task<string> GetResponseAsync(ChatRequest request, string userEmail)
        {
            ChatHistory history = CreateHistory(request);
            string poisString = await GetPoisString(history);
            var questionaire = await GetByEmailAsync(userEmail);

            var prompt = PromptTemplate.Replace("{{UserQuestionaire}}", questionaire.ToEmbeddingString() ?? "N/A")
                                        .Replace("{{UserPrompt}}", request.History.Last().Content ?? "N/A")
                                        .Replace("{{POIList}}", poisString ?? "N/A");

            history.RemoveAt(history.Count - 1); // Remove previous user prompt
            history.AddUserMessage(prompt);

            var result = await chatService.GetChatMessageContentAsync(history);
            return Regex.Unescape(result.Content);
        }

        private async Task<Questionaire?> GetByEmailAsync(string userEmail)
        {
            var response = await client.GetAsync("/api/user/questionaire");
            response.EnsureSuccessStatusCode();
            var questionaire = await response.Content.ReadFromJsonAsync<Questionaire>();

            return questionaire;
        }

        private async Task<string> GetPoisString(ChatHistory history)
        {
            var embeddingResult = await embedder.GenerateAsync(history.Last().Content);
            var vector = embeddingResult.Vector;
            var searchResults = await store.SearchAsync(vector, top: 3)
                .Where(r => r.Score > 0.6f)
                                            .ToListAsync();

            var poiTasks = searchResults
                .Select(async (result, i) =>
                {
                    var poi = result.Record;
                    var score = result.Score;
                    Console.WriteLine($"POI #{i + 1} Score: {score}");
                    var queryParams = new Dictionary<string, string?> { { "identifier", poi.EntityId } };
                    var uri = QueryHelpers.AddQueryString(poi.apiEndpoint, queryParams);

                    var response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();

                    var poiContent = await response.Content.ReadAsStringAsync();
                    return $"[POI #{i + 1} | Score: {score:F4}]\n{poiContent}\n";
                });

            var poiContents = await Task.WhenAll(poiTasks);
            var poisString = string.Join("\n", poiContents);
            return poisString;
        }

        private ChatHistory CreateHistory(ChatRequest request)
        {
            var history = new ChatHistory();
            history.AddSystemMessage(SystemPrompt);
            foreach (var msg in request.History)
            {
                // Maps "user", "assistant", or "system" to AuthorRole
                history.Add(new ChatMessageContent(new AuthorRole(msg.Role), msg.Content));
            }

            return history;
        }
    }
    public record ChatMessageDto(string Role, string Content);
    public record ChatRequest(List<ChatMessageDto> History);

}
