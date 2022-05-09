namespace Server;

public class ServerFactory : IAsyncDisposable
{
    private readonly WebApplication _app;
    private readonly CancellationToken _token;

    private ServerFactory(WebApplicationOptions options)
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

    public static async Task<ServerFactory> RunWithOptions(WebApplicationOptions options)
    {
        var server = new ServerFactory(options);
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