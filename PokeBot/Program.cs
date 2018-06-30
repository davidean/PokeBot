using System;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace PokeBot
{
    class Program
    {

        private DiscordSocketClient client;
        private CommandService command;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();




        private async Task MainAsync()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
            });

            command = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug,

            });


            client.MessageReceived += client_messageRecieved;
            await command.AddModulesAsync(Assembly.GetEntryAssembly());

            client.Ready += client_ready;
            client.Log += client_log;


            string token = "";
            using (var stream = new FileStream(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.0", @"txt\Token.txt"), FileMode.Open, FileAccess.Read))
            using (var ReadToken = new StreamReader(stream))
            {
                token = ReadToken.ReadToEnd();
            }


            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task client_log(LogMessage message)
        {
            Console.WriteLine($"{DateTime.Now} at {message.Source} {message.Message}");

        }

        private async Task client_ready()
        {
            await client.SetGameAsync("po!help for help");
        }

        private async Task client_messageRecieved(SocketMessage MessageParam)
        {
            //configure commands here.
            var message = MessageParam as SocketUserMessage;
            var context = new SocketCommandContext(client, message);

            if(context.Message == null || context.Message.Content == "")
            {
                return;
            }

            if (context.User.IsBot)
            {
                return;
            }


            int argspos = 0;
            if (!(message.HasStringPrefix("po!", ref argspos) || message.HasMentionPrefix(client.CurrentUser, ref argspos))) 
            {
                return;
            }

            var result = await command.ExecuteAsync(context, argspos);

            if (!result.IsSuccess)
            {
                Console.WriteLine("Something went wrong!!!!");
            }


        }


        }
}





    



