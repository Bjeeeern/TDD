using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Server;
using Xunit;

namespace Tests.TestSuites.Experiments;

public class Server
{
    [Fact]
    public async Task CanStartAndShutDown()
    {
        await using var server = await WebApplicationRunner.RunWithOptions(new WebApplicationOptions
        {
            ApplicationName = "Server",
            EnvironmentName = "Development",
            Args = new[] {"--urls", "https://127.0.0.1:0;http://127.0.0.1:0"}
        });
    }
}