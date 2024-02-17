using System;
using Terraria;

namespace KookBot;

public static class ServerStatusMonitor
{
    private static int timer;
    
    public static void InjectDetours() {
        // 服务器开启，机器人上线
        On_Netplay.StartServer += orig => {
            orig.Invoke();

            string token = Environment.GetEnvironmentVariable("KookToken", EnvironmentVariableTarget.User)!;
            KookBot.Bot.Initialize(token);
        };

        // 服务器关闭后机器人下线
        On_Netplay.StopBroadCasting += orig => {
            orig.Invoke();

            KookBot.Bot.StopAsync().Wait();
            KookBot.Bot.Dispose();
        };

        // 服务器更新事件
        On_Netplay.UpdateServerInMainThread += orig => {
            orig.Invoke();

            timer++;
            if (timer % 60 == 0)
                KookBot.Bot.Update();
        };
    }
}