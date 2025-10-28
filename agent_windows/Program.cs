using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RemoteAgent;
public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureServices((context, services) => {
                services.AddHostedService<AgentWorker>();
            })
            .Build();

        await host.RunAsync();
    }
}
