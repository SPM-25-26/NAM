namespace nam.Server.Models.ApiResponse
{
    /// <summary>
    /// DTO di risposta standardizzata per tutte le API.
    /// </summary>
    /// <typeparam name="T">Tipo del payload dati (può essere null per risposte senza body specifico).</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica se l'operazione è andata a buon fine.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Messaggio leggibile dall'utente/client che descrive l'esito.
        /// </summary>
        public string Message { get; init; } = string.Empty;

        /// <summary>
        /// Eventuale codice di errore/logico comprensibile dal client (es. INVALID_CREDENTIALS, TOKEN_REVOKED, ecc.).
        /// </summary>
        public string? ErrorCode { get; init; }

        /// <summary>
        /// Dati restituiti in caso di successo (o anche in alcuni errori).
        /// </summary>
        public T? Data { get; init; }

        public static ApiResponse<T> Ok(T? data = default, string message = "Operazione eseguita correttamente.")
            => new()
            {
                Success = true,
                Message = message,
                Data = data
            };

        public static ApiResponse<T> Fail(string message, string? errorCode = null, T? data = default)
            => new()
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                Data = data
            };
    }
}
