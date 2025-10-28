using System.IO;
using System.Text.Json;

namespace RemoteAgent;
public class Config
{
    public string ServerUrl { get; set; } = "ws://127.0.0.1:8443";
    public string AgentId { get; set; } = "agent-office-01";
    public string PreSharedKey { get; set; } = "supersecret_local_key";
    public int CaptureIntervalMs { get; set; } = 500;
    public bool EnableInteractive { get; set; } = true;

    public static Config Load(string path)
    {
        if (!File.Exists(path))
        {
            var def = new Config();
            File.WriteAllText(path, JsonSerializer.Serialize(def, new JsonSerializerOptions { WriteIndented = true }));
            return def;
        }
        var txt = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Config>(txt)!;
    }
}
