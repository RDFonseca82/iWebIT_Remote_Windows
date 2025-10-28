using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteAgent;
public class AgentWorker : BackgroundService
{
    private readonly WsClient _ws;
    private readonly Config _cfg;

    public AgentWorker()
    {
        _cfg = Config.Load("config.json");
        _ws = new WsClient(_cfg);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _ws.ConnectAsync(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}
