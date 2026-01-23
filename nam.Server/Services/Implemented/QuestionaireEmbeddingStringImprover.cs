using Microsoft.SemanticKernel.ChatCompletion;

namespace nam.Server.Services.Implemented
{
    public class QuestionaireEmbeddingStringImprover(IChatCompletionService chatService)
    {

        private readonly string prompt = """
            ## Prompt di Raffinazione Semantica

            Agisci come un esperto di analisi psicografica e turistica. Trasforma i seguenti dati grezzi di un questionario in una descrizione densa, coesa e ricca di sfumature semantiche. L'obiettivo è generare un testo ottimizzato per modelli di embedding che catturi non solo le parole chiave, ma l'intento latente e il profilo comportamentale del viaggiatore. Espandi i concetti sintetici in descrizioni articolate senza aggiungere informazioni arbitrarie non deducibili dai dati.

            **Dati Input:**
            {dati_questionario}

            **Istruzioni Output:**

            * Utilizza un registro formale e analitico.
            * Esplicita le relazioni tra i campi (es. come l'età influenza la modalità di scoperta).
            * Utilizza sinonimi colti e terminologia specifica del settore travel (es. serendipità, turismo esperienziale, slow travel).
            * Produci un singolo paragrafo di circa 100-150 parole.

            ---

            ### Esempio di Trasformazione

            **Input:**

            * Interest: "article", "organization"
            * TravelStyle: "relax", "culturale"
            * AgeRange: "over_50"
            * TravelRange: "Esploratore"
            * TravelCompanions: "friends"
            * DiscoveryMode: "Sorprendimi"

            **Output Raffinato:**
            Il profilo delinea un viaggiatore maturo, appartenente alla fascia demografica over 50, che approccia l'esperienza turistica con un equilibrio tra rigore intellettuale e desiderio di decompressione. L'interesse per la saggistica e le strutture organizzative suggerisce una propensione per itinerari che offrano profondità analitica e contestualizzazione storica o sociale. Nonostante la ricerca di momenti di relax, la natura di "esploratore" e la preferenza per una modalità di scoperta basata sulla sorpresa indicano un'apertura verso la serendipità e l'imprevisto controllato. Il viaggio è concepito come un'attività di socialità condivisa con un gruppo di pari, dove la dimensione culturale non è passiva, ma una ricerca attiva di stimoli nuovi e non convenzionali, privilegiando l'autenticità del momento rispetto alla pianificazione rigida.
            """;

        public async Task<string> ImproveEmbeddingStringAsync(string questionaireData, CancellationToken cancellationToken = default)
        {
            var updatedPrompt = prompt.Replace("{dati_questionario}", questionaireData);
            var improvedString = await chatService.GetChatMessageContentAsync(updatedPrompt, cancellationToken: cancellationToken);
            return improvedString.Content;
        }
    }
}
