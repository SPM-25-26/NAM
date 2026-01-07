using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using nam.Server.DTOs;
using nam.Server.Services.Interfaces;
using System.Security.Claims;

namespace nam.Server.Endpoints
{
    public class QuestionaireEndpoints
    {
        private static Serilog.ILogger? _logger;

        public static void ConfigureLogger(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task<IResult> UpdateQuestionaire(
            [FromServices] IQuestionaireService questionaireService,
            QuestionaireDto questionaireDto,
            HttpContext httpContext,
            CancellationToken cancellationToken = default)
        {
            if (_logger is null)
            {
                throw new InvalidOperationException("Logger not configured. Please call ConfigureLogger before using this endpoint.");
            }
            var userEmail = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.Warning("UpdateQuestionaire: User email claim is missing.");
                return TypedResults.Unauthorized();
            }
            var questionaire = new Questionaire
            {
                Interest = questionaireDto.Interest,
                TravelStyle = questionaireDto.TravelStyle,
                AgeRange = questionaireDto.AgeRange,
                TravelRange = questionaireDto.TravelRange,
                TravelCompanions = questionaireDto.TravelCompanions,
                DiscoveryMode = questionaireDto.DiscoveryMode
            };
            try
            {
                var updateResult = await questionaireService.UpdateAsync(questionaire, userEmail, cancellationToken);
                if (!updateResult)
                {
                    _logger.Warning("UpdateQuestionaire: Failed to update questionaire for user {UserEmail}.", userEmail);
                    return TypedResults.BadRequest("Failed to update questionaire.");
                }
                _logger.Information("UpdateQuestionaire: Successfully updated questionaire for user {UserEmail}.", userEmail);
                return TypedResults.Ok("Questionaire updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "UpdateQuestionaire: An error occurred while updating questionaire for user {UserEmail}.", userEmail);
                return TypedResults.InternalServerError("An error occurred while updating the questionaire.");
            }
        }

        public static async Task<IResult> GetQuestionaire(
                [FromServices] IQuestionaireService questionaireService,
                HttpContext httpContext,
                CancellationToken cancellationToken = default)
        {
            if (_logger is null)
            {
                throw new InvalidOperationException("Logger non configurato. È necessario chiamare ConfigureLogger prima di utilizzare questo endpoint.");
            }
            var userEmail = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.Warning("GetQuestionaire: Il claim email dell'utente è mancante.");
                return TypedResults.Unauthorized();
            }
            try
            {
                var questionaire = await questionaireService.GetByUserMailAsync(userEmail, cancellationToken);
                if (questionaire == null)
                {
                    _logger.Information("GetQuestionaire: Nessun questionario trovato per l'utente {UserEmail}.", userEmail);
                    return TypedResults.NotFound("Nessun questionario trovato per l'utente specificato.");
                }
                _logger.Information("GetQuestionaire: Questionario recuperato con successo per l'utente {UserEmail}.", userEmail);
                return TypedResults.Ok(questionaire);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetQuestionaire: Errore durante il recupero del questionario per l'utente {UserEmail}.", userEmail);
                return TypedResults.InternalServerError("Si è verificato un errore durante il recupero del questionario.");
            }

        }
    }
}
