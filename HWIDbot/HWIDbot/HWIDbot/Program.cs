using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Discord;

namespace HWIDbot
{
    class Program
    {
        static void Main(string[] args) => new Program().runBot().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task runBot()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();

            string botToken = "NDMyNTU5MTEwNzU2NjMwNTM4.Da0feg.L1VRB45g_mHZnWujA2WSG259BT0";

            _client.Log += Log;
            await RegisterCommands();
            await _client.LoginAsync(TokenType.Bot, botToken);
            await _client.StartAsync();


            await Task.Delay(-1);

        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }

        public async Task RegisterCommands()
        {
            _client.MessageReceived += handeCommand;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }


        private async Task handeCommand(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message is null || message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
