using Discord;
using Discord.Interactions;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql;
using System;
using System.Threading;
using System.Threading.Tasks;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;

namespace InteractionFramework
{
    public class Program
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _services;

        private readonly DiscordSocketConfig _socketConfig = new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged,
            AlwaysDownloadUsers = true,
        };

        public Program()
        {
            _configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "DC_")
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            _services = new ServiceCollection()
                .AddSingleton(_configuration)
                .AddSingleton(_socketConfig)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionHandler>()
                .AddDbContext<ApplicationContext>(
                        dbContextOptions => dbContextOptions.UseMySql(_configuration.GetRequiredSection("ConnectionStrings")["DefaultConnection"],
                                        ServerVersion.AutoDetect(_configuration.GetRequiredSection("ConnectionStrings")["DefaultConnection"])
                                        )
                                    )
                .BuildServiceProvider();

                //Add command context


                // _services.AddSingleton(new CommandService());

                // var provider = _services.BuildServiceProvider();
                // provider.GetRequiredService<CommandService>();
                // _client.AddSingleton(provider);
        }

        static void Main(string[] args)
            => new Program().RunAsync()
                .GetAwaiter()
                .GetResult();

        public async Task RunAsync()
        {
            var client = _services.GetRequiredService<DiscordSocketClient>();

            client.Log += LogAsync;

            // Here we can initialize the service that will register and execute our commands
            await _services.GetRequiredService<InteractionHandler>()
                .InitializeAsync();
            //stuff
            // Bot token can be provided from the Configuration object we set up earlier
            await client.LoginAsync(TokenType.Bot, _configuration["token"]);
            await client.StartAsync();

            // Never quit the program until manually forced to.
            await Task.Delay(Timeout.Infinite);
        }

        private async Task LogAsync(LogMessage message)
            => Console.WriteLine(message.ToString());

        public static bool IsDebug()
        {
            #if DEBUG
                return true;
            #else
                return false;
            #endif
        }
    }
}