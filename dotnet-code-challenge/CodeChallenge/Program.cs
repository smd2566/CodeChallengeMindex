using CodeChallenge.Config;

/// <summary>
/// Application entry point. Everything (database setup, dependency injection, routing)
/// is configured in <see cref="App"/>; this class only builds and runs it.
/// It is also public so the integration tests can host the app via WebApplicationFactory&lt;Program&gt;.
/// </summary>
public class Program
{
    /// <summary>Builds the configured web application and starts listening for requests.</summary>
    public static void Main(string[] args)
    {
        new App().Configure(args).Run();
    }
}