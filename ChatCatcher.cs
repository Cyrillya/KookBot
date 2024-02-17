using System.Collections.Generic;
using Terraria;
using Terraria.Chat.Commands;

namespace KookBot;

public class ChatCatcher
{
    public static Queue<string> IncomingMessages = new ();
    
    public static void InjectDetours() {
        // 捕获文本
        On_SayChatCommand.ProcessIncomingMessage += (orig, self, text, clientId) => {
            orig.Invoke(self, text, clientId);

            IncomingMessages.Enqueue($"{Main.player[clientId].name} 说: {text}");
        };
    }
}