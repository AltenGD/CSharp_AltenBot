using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using CSharp_AltenBot.Modules;

namespace CSharp_AltenBot
{
    class Bot
    {
        public DiscordClient Client { get; set; }
        private CommandsNextModule _commands { get; set; }

        public async Task Start()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;
            var cfg = new DiscordConfiguration
            {
                Token = Config.bot.token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                LogLevel = LogLevel.Info,
                UseInternalLogHandler = true
            };
            Client = new DiscordClient(cfg);
            await Client.ConnectAsync();
            SetUpEvents();
            SetupCommands();
            await Task.Delay(-1);

        }

        private void SetupCommands()
        {
            var cncfg = new CommandsNextConfiguration
            {
                StringPrefix = Config.bot.cmdPrefix,
                EnableDms = true,
                EnableMentionPrefix = true,
                EnableDefaultHelp = false
            };
            _commands = Client.UseCommandsNext(cncfg);
            _commands.RegisterCommands<NormalCommands>();
        }

        private void SetUpEvents()
        {
            Client.DebugLogger.LogMessage(LogLevel.Info, "AltenBot", "Setting up events.", DateTime.Now);

            Client.Heartbeated += _client_HeartBeated;

            Client.SocketOpened += _client_SocketOpened;


            Client.Ready += async (e) =>
            {
                Client.DebugLogger.LogMessage(LogLevel.Info, "AltenBot", "Ready", DateTime.Now);
                Client.DebugLogger.LogMessage(LogLevel.Info, "AltenBot", $"Bot Name: '{Client.CurrentUser.Username}'\n GuildCount: {Client.Guilds.Count} Guild(s).", DateTime.Now);
                await Client.UpdateStatusAsync(new DiscordGame($"{Config.bot.cmdPrefix}help | Currently on {Client.Guilds.Count} different servers!"));
            };
        }
        private async Task SetGame()
        {
            await Client.UpdateStatusAsync(new DiscordGame($"{Config.bot.cmdPrefix}help | Currently on {Client.Guilds.Count} different servers!"));
        }
        private Task _client_SocketOpened()
        {
            Client.DebugLogger.LogMessage(LogLevel.Info, "AltenBot", "Socket opened", DateTime.Now);
            return Task.CompletedTask;
        }


        private Task _client_HeartBeated(HeartbeatEventArgs e)
        {
            Client.DebugLogger.LogMessage(LogLevel.Info, "AltenBot", $"Heart Beat: {e.Ping}ms", DateTime.Now);
            return Task.CompletedTask;
        }

        private Task _client_SocketClosed()
        {
            Client.DebugLogger.LogMessage(LogLevel.Info, "AltenBot", "Socket Closed", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
