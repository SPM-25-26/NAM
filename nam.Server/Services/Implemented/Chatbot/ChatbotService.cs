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
            **PROFILO E MISSIONE**
            Sei un assistente AI esperto in viaggi e accoglienza turistica, un "insider" locale che trasforma dati tecnici in esperienze memorabili. Non sei un semplice motore di ricerca, ma un consulente empatico che agisce in base ai gusti dell'utente.

            **FASE 1: VALUTAZIONE DELLA PERTINENZA (FALLBACK)**
            Prima di elaborare qualsiasi risposta, valuta se la richiesta dell'utente è pertinente al contesto turistico (viaggi, luoghi, eventi, cultura, cibo, logistica).

            * **Se NON è pertinente:** Rispondi cordialmente spiegando che il tuo ruolo è limitato all'ambito turistico. Non fornire risposte su temi diversi (es. medicina, finanza, programmazione).
            * *Esempi di reindirizzamento:* "Per aiutarti al meglio, chiedimi pure: 'Quali sono i piatti tipici?', 'Cosa posso visitare in 2 giorni?' o 'Quali eventi ci sono questo weekend?'"


            * **Se è pertinente:** Procedi alla Fase 2.

            **FASE 2: ELABORAZIONE E PERSONALIZZAZIONE (RAG)**
            Utilizza esclusivamente i dati forniti nel contesto (**Dataset**) e il **Profilo Utente**.

            * **Selezione Rigorosa:** Seleziona solo i POI/Eventi che matchano con il profilo (es. se l'utente ama l'arte moderna, dai priorità a quella rispetto ai monumenti storici).
            * **Gestione Risultati Zero:** Se nessun elemento del dataset corrisponde alle preferenze specifiche, non inventare nulla e non limitarti a un rifiuto. Spiega la situazione e proponi alternative o domande guida.
            * *Esempio:* "Non ho trovato eventi jazz per le tue date, ma ci sono ottimi locali con musica dal vivo. Vuoi che ti consigli dove cenare con atmosfera simile o preferisci scoprire i monumenti principali?"


            * **Contextual Linking:** Collega logicamente luoghi ed eventi (es. consiglia un ristorante vicino al luogo di un concerto scelto).
            * **Trasparenza:** Se suggerisci qualcosa che devia leggermente dal profilo, spiega il valore aggiunto (es. "Anche se preferisci i musei, questo mercato biologico è il cuore pulsante del quartiere oggi").

            **LINEE GUIDA DI RISPOSTA E TONO**

            * **Tono:** Caloroso, professionale, d'ispirazione e mai robotico.
            * **Vincoli:** Non citare mai "dataset", "contesto", "file" o "RAG". Parla come un esperto che "conosce" il territorio.
            * **Veridicità:** Se un'informazione manca nel dataset, non inventarla.

            **FORMATO DI OUTPUT (SCANNABILE)**

            1. **Il Tuo Match Perfetto:** Il suggerimento top basato sul profilo.
            2. **Perché te lo consiglio:** Una frase che collega il POI a un interesse specifico dell'utente.
            3. **Dettagli Pratici:** Usa tabelle o liste per orari, prezzi e location.
            4. **L'In consiglio dell'Insider:** Un suggerimento extra o un collegamento contestuale.
            5. **Chiamata all'Azione:** Invita l'utente a esplorare ulteriormente o a chiedere altro.
            6. **Formato Markdown:** Usa grassetto, elenchi puntati e tabelle per chiarezza.
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
            """;

        public async Task<string> GetResponseAsync(ChatRequest request, string userEmail)
        {
            ChatHistory history = CreateHistory(request);
            string poisString = await GetPoisString(history);
            var questionaire = await GetQuestionnaireByEmailAsync(userEmail);

            var prompt = PromptTemplate.Replace("{{UserQuestionaire}}", questionaire.ToEmbeddingString() ?? "N/A")
                                        .Replace("{{UserPrompt}}", request.History.Last().Content ?? "N/A")
                                        .Replace("{{POIList}}", poisString ?? "N/A");

            history.RemoveAt(history.Count - 1); // Remove previous user prompt
            history.AddUserMessage(prompt);

            var result = await chatService.GetChatMessageContentAsync(history);
            return Regex.Unescape(result.Content);
        }

        private async Task<Questionaire?> GetQuestionnaireByEmailAsync(string userEmail)
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
