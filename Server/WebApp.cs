namespace Server;

public class WebApp : IAsyncDisposable
{
    private readonly WebApplication _app;
    private readonly CancellationToken _token;

    private WebApp(WebApplicationOptions options)
    {
        _token = new CancellationToken();

        var builder = WebApplication.CreateBuilder(options);
        _app = builder.Build();

        _app.UseDefaultFiles();
        _app.UseStaticFiles();
    }

    public async ValueTask DisposeAsync()
    {
        await _app.StopAsync(_token);
    }

    public static async Task<WebApp> RunWithOptions(WebApplicationOptions options)
    {
        var server = new WebApp(options);
        await server.Start();
        return server;
    }

    private async Task Start()
    {
        await _app.StartAsync(_token);
    }

    public void BlockUntilShutdown()
    {
        _token.WaitHandle.WaitOne();
    }
}