using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Server;
using Tests.Utilities;
using Xunit;

namespace Tests.TestSuites.Experiments;

public class Server
{
    [Fact]
    public async Task CanStartAndShutDown()
    {
        await using var server = await WebApp.RunWithOptions(new WebApplicationOptions
        {
            ApplicationName = "Server",
            EnvironmentName = "Development",
            ContentRootPath = CommonPaths.ServerProject(),
            WebRootPath = CommonPaths.ServerWebRoot()
        });
    }
}