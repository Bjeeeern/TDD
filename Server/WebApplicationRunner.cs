using Microsoft.AspNetCore.StaticFiles;

namespace Server;

public class WebApplicationRunner : IAsyncDisposable
{
    private readonly WebApplication _app;

    private WebApplicationRunner(WebApplicationOptions options)
    {
        var builder = WebApplication.CreateBuilder(options);

        _app = builder.Build();
        _app.UseDefaultFiles();
        _app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = ProvideAdditionalAllowedFileExtentions()
        });

        _app.UseRouting();
        _app.UseEndpoints(endpoints => endpoints.MapFallbackToFile("/index.html"));
    }

    public string HttpsUri => _app.Urls.First(uri => uri.Contains("https"));

    public async ValueTask DisposeAsync()
    {
        await _app.StopAsync();
    }

    private IContentTypeProvider ProvideAdditionalAllowedFileExtentions()
    {
        var provider = new FileExtensionContentTypeProvider();

        provider.Mappings[".dll"] = "application/dll";
#if DEBUG
        provider.Mappings[".pdb"] = "application/pdb";
        provider.Mappings[".blat"] = "application/blat";
        provider.Mappings[".dat"] = "application/dat";
#endif //DEBUG

        return provider;
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