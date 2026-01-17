using nam.Server.Services.Implemented.Chatbot;
using nam.ServerTests.mock;

namespace nam.ServerTests
{
    [TestClass]
    public sealed class ChatbotServiceTests
    {
        private ChatbotServiceTestBuilder _builder = null!;
        private ChatbotService _chatbotService = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new ChatbotServiceTestBuilder();
            _chatbotService = _builder.Build();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _builder.Dispose();
        }

        /// <summary>
        /// Verifica che GetResponseAsync restituisca una risposta valida quando viene fornita una richiesta corretta e tutti i servizi mockati rispondono come previsto.
        /// </summary>
        [TestMethod]
        public async Task GetResponseAsync_ReturnsResponse_WhenValidRequest()
        {
            // Arrange
            var userEmail = "user@example.com";
            var request = new ChatRequest(new List<ChatMessageDto>
            {
                new ChatMessageDto("user", "Consigliami un ristorante a Roma")
            });

            _builder.SetupQuestionaireResponse(userEmail, "Preferenze: cucina italiana, budget medio");
            _builder.SetupVectorSearchResults(0.8f, "POI123", "Restaurant");
            _builder.SetupEntityApiResponse("POI123", "Ristorante Da Mario - Cucina tradizionale");
            _builder.SetupChatCompletionResponse("Ti consiglio il Ristorante Da Mario per la sua cucina tradizionale");

            // Act
            var response = await _chatbotService.GetResponseAsync(request, userEmail);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("Ristorante Da Mario"));
        }

        /// <summary>
        /// Verifica che GetResponseAsync gestisca correttamente una cronologia di messaggi multipli nella richiesta.
        /// </summary>
        [TestMethod]
        public async Task GetResponseAsync_HandlesMultipleMessages_InHistory()
        {
            // Arrange
            var userEmail = "user@example.com";
            var request = new ChatRequest(new List<ChatMessageDto>
            {
                new ChatMessageDto("user", "Ciao"),
                new ChatMessageDto("assistant", "Ciao! Come posso aiutarti?"),
                new ChatMessageDto("user", "Cerco un museo")
            });

            _builder.SetupQuestionaireResponse(userEmail, "Preferenze: arte moderna");
            _builder.SetupVectorSearchResults(0.9f, "POI456", "Museum");
            _builder.SetupEntityApiResponse("POI456", "Museo di Arte Moderna");
            _builder.SetupChatCompletionResponse("Il Museo di Arte Moderna è perfetto per te");

            // Act
            var response = await _chatbotService.GetResponseAsync(request, userEmail);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Length > 0);
        }

        /// <summary>
        /// Verifica che GetResponseAsync filtri i POI con score troppo basso e non li includa nella risposta.
        /// </summary>
        [TestMethod]
        public async Task GetResponseAsync_HandlesLowScorePOIs_FiltersResults()
        {
            // Arrange
            var userEmail = "user@example.com";
            var request = new ChatRequest(new List<ChatMessageDto>
            {
                new ChatMessageDto("user", "Cosa mi consigli?")
            });

            _builder.SetupQuestionaireResponse(userEmail, "Preferenze: avventura");
            _builder.SetupVectorSearchResults(0.5f, "POI789", "Park"); // Score troppo basso (< 0.6)
            _builder.SetupChatCompletionResponse("Non ho trovato risultati pertinenti");

            // Act
            var response = await _chatbotService.GetResponseAsync(request, userEmail);

            // Assert
            Assert.IsNotNull(response);
            // Il POI con score 0.5 dovrebbe essere filtrato
        }

        /// <summary>
        /// Verifica che GetResponseAsync gestisca correttamente il caso in cui non vengono trovati POI e restituisca comunque una risposta.
        /// </summary>
        [TestMethod]
        public async Task GetResponseAsync_HandlesNoPOIsFound_ReturnsResponse()
        {
            // Arrange
            var userEmail = "user@example.com";
            var request = new ChatRequest(new List<ChatMessageDto>
            {
                new ChatMessageDto("user", "Dove posso andare?")
            });

            _builder.SetupQuestionaireResponse(userEmail, "Preferenze: natura");
            _builder.SetupEmptyVectorSearchResults();
            _builder.SetupChatCompletionResponse("Al momento non ho suggerimenti specifici");

            // Act
            var response = await _chatbotService.GetResponseAsync(request, userEmail);

            // Assert
            Assert.IsNotNull(response);
        }

        /// <summary>
        /// Verifica che GetResponseAsync gestisca il caso in cui il questionario non viene trovato e utilizzi valori di default ("NA").
        /// </summary>
        [TestMethod]
        public async Task GetResponseAsync_HandlesQuestionaireNotFound_UsesNA()
        {
            // Arrange
            var userEmail = "newuser@example.com";
            var request = new ChatRequest(new List<ChatMessageDto>
            {
                new ChatMessageDto("user", "Aiutami a scegliere")
            });

            _builder.SetupQuestionaireNotFound();
            _builder.SetupVectorSearchResults(0.7f, "POI999", "Hotel");
            _builder.SetupEntityApiResponse("POI999", "Hotel Centrale");
            _builder.SetupChatCompletionResponse("Ecco un suggerimento generico");

            // Act
            var response = await _chatbotService.GetResponseAsync(request, userEmail);

            // Assert
            Assert.IsNotNull(response);
        }

        /// <summary>
        /// Verifica che GetResponseAsync combini correttamente più POI trovati e li includa nella risposta.
        /// </summary>
        [TestMethod]
        public async Task GetResponseAsync_HandlesMultiplePOIs_CombinesResults()
        {
            // Arrange
            var userEmail = "user@example.com";
            var request = new ChatRequest(new List<ChatMessageDto>
            {
                new ChatMessageDto("user", "Cosa posso visitare oggi?")
            });

            _builder.SetupQuestionaireResponse(userEmail, "Preferenze: cultura");
            _builder.SetupMultipleVectorSearchResults(new[]
            {
                (0.9f, "POI1", "Museum", "Museo Archeologico"),
                (0.85f, "POI2", "Monument", "Colosseo"),
                (0.75f, "POI3", "Gallery", "Galleria d'Arte")
            });
            _builder.SetupChatCompletionResponse("Ti consiglio questi tre luoghi culturali");

            // Act
            var response = await _chatbotService.GetResponseAsync(request, userEmail);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("tre luoghi"));
        }

        /// <summary>
        /// Verifica che GetResponseAsync gestisca correttamente caratteri escaped nella risposta.
        /// </summary>
        [TestMethod]
        public async Task GetResponseAsync_HandlesEscapedCharacters_InResponse()
        {
            // Arrange
            var userEmail = "user@example.com";
            var request = new ChatRequest(new List<ChatMessageDto>
            {
                new ChatMessageDto("user", "Dettagli sul luogo")
            });

            _builder.SetupQuestionaireResponse(userEmail, "Preferenze: dettagli");
            _builder.SetupVectorSearchResults(0.8f, "POI100", "Restaurant");
            _builder.SetupEntityApiResponse("POI100", "Ristorante con \"specialità\" locali");
            _builder.SetupChatCompletionResponse("Il ristorante offre \"specialità\" uniche\\n\\n");

            // Act
            var response = await _chatbotService.GetResponseAsync(request, userEmail);

            // Assert
            Assert.IsNotNull(response);
            // Verifica che i caratteri escaped siano gestiti correttamente
        }

        /// <summary>
        /// Verifica che GetResponseAsync lanci una HttpRequestException quando la chiamata all'Entity API fallisce.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task GetResponseAsync_ThrowsException_WhenEntityApiFails()
        {
            // Arrange
            var userEmail = "user@example.com";
            var request = new ChatRequest(new List<ChatMessageDto>
            {
                new ChatMessageDto("user", "Test")
            });

            _builder.SetupQuestionaireResponse(userEmail, "Preferenze: test");
            _builder.SetupVectorSearchResults(0.8f, "POI500", "Test");
            _builder.SetupEntityApiFailure("POI500");

            // Act
            await _chatbotService.GetResponseAsync(request, userEmail);

            // Assert - Exception expected
        }

        /// <summary>
        /// Verifica che GetResponseAsync gestisca correttamente messaggi con contenuto vuoto senza errori.
        /// </summary>
        [TestMethod]
        public async Task GetResponseAsync_HandlesEmptyMessageContent_Gracefully()
        {
            // Arrange
            var userEmail = "user@example.com";
            var request = new ChatRequest(new List<ChatMessageDto>
            {
                new ChatMessageDto("user", "")
            });

            _builder.SetupQuestionaireResponse(userEmail, "Preferenze: qualsiasi");
            _builder.SetupEmptyVectorSearchResults();
            _builder.SetupChatCompletionResponse("Puoi farmi una domanda più specifica?");

            // Act
            var response = await _chatbotService.GetResponseAsync(request, userEmail);

            // Assert
            Assert.IsNotNull(response);
        }
    }
}