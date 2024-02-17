using System;
using Terraria.ModLoader;

namespace KookBot;

public class KookBot : Mod
{
    internal static BotHandler Bot;
    
    public override void Load() {
        Bot = new BotHandler();
        ServerStatusMonitor.InjectDetours();
        ChatCatcher.InjectDetours();
    }

    public override void Unload() {
        if (Bot is null)
            return;
        if (!Bot.IsDisposed)
            Bot.Dispose();
        Bot = null;
    }
}