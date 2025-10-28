using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteAgent;
public class WsClient
{
    private ClientWebSocket _ws = new ClientWebSocket();
    private readonly Config _cfg;

    public WsClient(Config cfg) { _cfg = cfg; }

    public async Task ConnectAsync(CancellationToken ct)
    {
        var uri = new Uri(_cfg.ServerUrl);
        await _ws.ConnectAsync(uri, ct);
        var register = new { type = "register", payload = new { agentId = _cfg.AgentId } };
        await SendJson(register, ct);
        _ = ReceiveLoop(ct);
    }

    private async Task ReceiveLoop(CancellationToken ct)
    {
        var buf = new byte[64 * 1024];
        while (_ws.State == WebSocketState.Open && !ct.IsCancellationRequested)
        {
            var res = await _ws.ReceiveAsync(new ArraySegment<byte>(buf), ct);
            if (res.MessageType == WebSocketMessageType.Close) { await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", ct); break; }
            var message = Encoding.UTF8.GetString(buf, 0, res.Count);
            HandleMessage(message);
        }
    }

    private void HandleMessage(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var type = doc.RootElement.GetProperty("type").GetString();
            Console.WriteLine($"[Agent] Received message type={type}");
            // TODO: implement handling: start_session, stop_session, file_chunk, etc.
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public async Task SendJson(object obj, CancellationToken ct)
    {
        var s = JsonSerializer.Serialize(obj);
        var bytes = Encoding.UTF8.GetBytes(s);
        await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, ct);
    }
}
