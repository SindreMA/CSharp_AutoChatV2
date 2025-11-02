using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TemplateBot;

namespace CSharp_AutoChatV2
{
    class Program
    {
        static async Task Main(string[] args)
            => await new Program().StartAsync();
        private CommandHandler _handler;
        private DiscordSocketClient _client;
        public string Token = "";
        public async Task StartAsync()
        {
            // Try to load token from environment variable first
            Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

            if (string.IsNullOrWhiteSpace(Token))
            {
                // Fallback to JSON file if environment variable is not set
                try
                {
                    string json = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Configs", "Token.json"));
                    Token = JsonConvert.DeserializeObject<string>(json);
                    await Log("Token loaded from JSON file", ConsoleColor.Yellow);
                }
                catch (Exception)
                {
                    await Log("Cannot import token from environment variable or JSON file!", ConsoleColor.Red);
                }
            }
            else
            {
                await Log("Token loaded from DISCORD_TOKEN environment variable", ConsoleColor.Green);
            }
            try
            {
                await Log("Setting up the bot", ConsoleColor.Green);
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };
            _client = new DiscordSocketClient(config);
            _handler = new CommandHandler(_client);
            await Log("Logging in...", ConsoleColor.Green);
            await _client.LoginAsync(TokenType.Bot, Token);
            await Log("Connecting...", ConsoleColor.Green);
            await _client.StartAsync();
            _client.GuildAvailable += _client_GuildAvailable;
            await Task.Delay(-1);
            }
            catch (Exception)
            {
                await Log("error at login, check that token is valid!", ConsoleColor.Red);
                Console.ReadLine();
            }

        }

        private async Task _client_GuildAvailable(SocketGuild arg)
        {

            await Log(arg.Name + " Connected!", ConsoleColor.Green);
        }
        public static async Task Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.Now + " : " + message, color);
            Console.ResetColor();

            await Task.CompletedTask;
        }

        public static string Replaceemoji(string inn)        {            string result = Regex.Replace(inn, @"\p{Cs}", "_");            return result;        }
        public static string removeemoji(string inn)        {            string result = Regex.Replace(inn, @"\p{Cs}", "");            return result;        }        public static string V_NameConverter(string input)        {            string output =                "v_" +                input.                ToLower().                Replace(" ", "_").                Replace(":", "").                Replace(@"/", "_").                Replace(".", "_").                Replace("#", "_").                Replace("(", "_").                Replace(")", "_").                Replace("!", "_").                Replace("|", "_").                Replace("@", "_").                Replace("£", "_").                Replace("\"", "_").                Replace("{", "_").                Replace("}", "_").                Replace("%", "_").                Replace("$", "_").                Replace("¤", "_").                Replace("&", "_").                Replace("\\", "_").                Replace("[", "_").                Replace("]", "_").                Replace("=", "_").                Replace("+", "_").                Replace("?", "_").                Replace("`", "_").                Replace("¨", "_").                Replace("^", "_").                Replace("~", "_").                Replace("*", "_").                Replace("*", "_").                Replace("'", "_").                Replace(",", "_").                Replace(".", "_").                Replace(":", "_").                Replace(";", "_").                Replace("<", "_").                Replace(">", "_");            return Replaceemoji(output);        }        public static string NameConverter(string input)        {            string output =
                input.                ToLower().                Replace(" ", "_").                Replace(":", "").                Replace(@"/", "_").                Replace(".", "_").                Replace("#", "_").                Replace("(", "_").                Replace(")", "_").                Replace("!", "_").                Replace("|", "_").                Replace("@", "_").                Replace("£", "_").                Replace("\"", "_").                Replace("{", "_").                Replace("}", "_").                Replace("%", "_").                Replace("$", "_").                Replace("¤", "_").                Replace("&", "_").                Replace("\\", "_").                Replace("[", "_").                Replace("]", "_").                Replace("=", "_").                Replace("+", "_").                Replace("?", "_").                Replace("`", "_").                Replace("¨", "_").                Replace("^", "_").                Replace("~", "_").                Replace("*", "_").                Replace("*", "_").                Replace("'", "_").                Replace(",", "_").                Replace(".", "_").                Replace(":", "_").                Replace(";", "_").                Replace("<", "_").                Replace(">", "_");            return removeemoji(output);        }

}
}
