using System;
using System.Linq;
using System.Threading.Tasks;
using Kook;
using Kook.WebSocket;

namespace KookBot;

public class BotHandler : IDisposable
{
    public bool IsDisposed { get; private set; }
    private KookSocketClient _client;

    public async void Initialize(string token) {
        var config = new KookSocketConfig { MessageCacheSize = 100 };
        _client = new KookSocketClient(config);

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        _client.Ready += () => {
            Console.WriteLine("Bot is connected!");
            return Task.CompletedTask;
        };
    }

    public void Update() {
        Console.WriteLine(_client.Guilds.Count);
        foreach (var clientGuild in _client.Guilds) {
            CheckMessageForGuild(clientGuild);
        }
    }

    private static async void CheckMessageForGuild(SocketGuild guild) {
        if (!ChatCatcher.IncomingMessages.TryDequeue(out string text))
            return;
        var channel = guild.TextChannels.First(c => c.Name is "机器人");
        await channel.SendTextAsync(text);
    }

    public async Task StopAsync() {
        await _client.LogoutAsync();
        await _client.StopAsync();
    }

    public void Dispose() {
        IsDisposed = true;
        if (_client is null)
            return;
        _client.Dispose();

        GC.SuppressFinalize(this);
    }
}