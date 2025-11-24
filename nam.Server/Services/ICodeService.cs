namespace nam.Server.Services
{
    /// <summary>
    /// Provides functionality for generating authentication codes.
    /// </summary>
    public interface ICodeService
{
    /// <summary>
    /// Generates a new authentication code.
    /// </summary>
    /// <returns>
    /// A string representing the newly generated authentication code.
    /// </returns>
    string GenerateAuthCode();
    
    /// <summary>
    /// Gets the number of minutes the generated authentication code 
    /// remains valid before it expires.
    /// </summary>
   int TimeToLiveMinutes { get; }
}

}