using DataInjection.Qdrant.Data;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using nam.Server.Services.Interfaces.Chatbot;

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

            [POI #1]
            {{POIList}}

            ---

            ### RICHIESTA DELL'UTENTE
            "{{UserPrompt}}"

            ---

            ### ISTRUZIONE FINALE
            In base al profilo sopra descritto e ai dati forniti, elabora una risposta personalizzata. Seleziona solo i POI/Eventi che meglio si adattano all'utente e spiega il perché della tua scelta.
            """;

        public async Task<string> GetResponseAsync(ChatRequest request, string userEmail)
        {
            ChatHistory history = CreateHistory(request);
            string poisString = await GetPoisString(history);
            var questionaire = await GetByEmailAsync(userEmail);

            var prompt = PromptTemplate.Replace("{{UserQuestionaire}}", questionaire.ToEmbeddingString() ?? "N/A")
                                        .Replace("{{UserPrompt}}", questionaire.ToEmbeddingString() ?? "N/A")
                                        .Replace("{{POIList}}", poisString ?? "N/A");

            history.RemoveAt(history.Count - 1); // Remove previous user prompt
            history.AddUserMessage(prompt);


            var result = await chatService.GetChatMessageContentAsync(history);
            return result.Content;
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
            var searchResult = await store.SearchAsync(vector, top: 3)
                .Select(p => p.Record)
                .Select(async poi =>
                {
                    var queryParams = new Dictionary<string, string?>
                    {
                        { "identifier", poi.EntityId }
                    };
                    var uri = QueryHelpers.AddQueryString(poi.apiEndpoint, queryParams);
                    var response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    var str = await response.Content.ReadAsStringAsync();
                    return str;
                })
                .Select((entity, i) => $"POI nr.{i + 1}\n{entity}")
                .ToListAsync();

            var poisString = string.Join("\n", searchResult);
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
