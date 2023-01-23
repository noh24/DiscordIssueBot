using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TestDiscord.Repository;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Runtime.ExceptionServices;



public class Program
{
    private readonly IServiceProvider _serviceProvider; 
    private DiscordSocketClient _client;


    public Program()
    {
        _serviceProvider = CreateProvider();
        
    }

    static void Main(string[] args)
        => new Program().RunAsync(args).GetAwaiter().GetResult();

    static IServiceProvider CreateProvider()
    {
        var config = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true).Build();
        var collection = new ServiceCollection()
            .AddSingleton<DiscordSocketClient>().AddDbContext<ApplicationContext>(
                        dbContextOptions => dbContextOptions
                                        .UseMySql(config.GetRequiredSection("ConnectionStrings")["DefaultConnection"],
                                        ServerVersion.AutoDetect(config.GetRequiredSection("ConnectionStrings")["DefaultConnection"])
                                          )
                                        );


        return collection.BuildServiceProvider();
    }

    async Task RunAsync(string[] args)
    {
        // Request the instance from the client.
        // Because we're requesting it here first, its targetted constructor will be called and we will receive an active instance.
        _client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
        //_client = _serviceProvider.GetRequiredService
        var config = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true).Build();
        _client.Ready += Client_Ready;
        _client.SlashCommandExecuted += SlashCommandHandler;
        _client.Log += async (msg) =>
        {
            await Task.CompletedTask;
            Console.WriteLine(msg);
        };

        await _client.LoginAsync(TokenType.Bot, config.GetRequiredSection("ConnectionStrings")["DefaultConnection"]);
        await _client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }

    public async Task Client_Ready()
    {
        // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
        var guild = _client.GetGuild(1067135346892099594);

        // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
        var guildCommand = new SlashCommandBuilder();

        // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
        guildCommand.WithName("first-command");

        // Descriptions can have a max length of 100.
        guildCommand.WithDescription("This is my first guild slash command!");

        // Let's do our global command
        var globalCommand = new SlashCommandBuilder();
        globalCommand.WithName("first-command");
        globalCommand.WithDescription("This is my first global slash command");

        try
        {
            // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
            await guild.CreateApplicationCommandAsync(guildCommand.Build());

            // With global commands we don't need the guild.
            await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
            // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
        }
        catch (ApplicationCommandException exception)
        {
            // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
            var json = JsonConvert.SerializeObject(exception.Message, Formatting.Indented);

            // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
            Console.WriteLine(json);
        }
    }

    private void CreateIssue(string name, string description, string connStr)
    {
        //string connStr = "server=localhost;user=root;database=sakila;port=3306;password=your_password";   
        using(SqlConnection connection = new SqlConnection(connStr))
        {
            string cmdText = "INSERT INTO discord_issue_bot.Issues (IssueId, Name, Description) VALUES (@IssueId, @Name, @Description)";

            using(SqlCommand command = new SqlCommand(cmdText, connStr))
            {
                command.Parameters.AddWithValue("@IssueId", null);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Description", description);
                
                connection.Open();
                int result = command.ExecuteNonQuery();

                if(result < 0)
                Console.WriteLine($"Error inserting {cmdText} into database");
            }
            
        }

        

    }
    //create solution
    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        CreateIssue()
        await command.RespondAsync($"You executed {command.Data.Name}");
    }

}