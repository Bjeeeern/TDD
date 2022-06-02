namespace Server;

public class WebApplicationRunner : IAsyncDisposable
{
    private readonly WebApplication _app;

    private WebApplicationRunner(WebApplicationOptions options)
    {
        var builder = WebApplication.CreateBuilder(options);
        _app = builder.Build();

        _app.UseDefaultFiles();
        _app.UseStaticFiles();
    }

    public string HttpsUri => _app.Urls.First(uri => uri.Contains("https"));

    public async ValueTask DisposeAsync()
    {
        await _app.StopAsync();
    }

    public static async Task<WebApplicationRunner> RunWithOptions(WebApplicationOptions options)
    {
        var server = new WebApplicationRunner(options);
        await server._app.StartAsync();
        return server;
    }

    public void BlockUntilShutdown()
    {
        _app.Lifetime.ApplicationStopping.WaitHandle.WaitOne();
    }
}