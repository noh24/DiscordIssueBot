using Discord;
using Discord.Interactivity;
using Discord.Interactions;
using DiscordBot.Models;
using InteractionFramework.Attributes;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Discord.WebSocket;
using System.Reflection.Emit;

namespace InteractionFramework.Modules
{
    // Interation modules must be public and inherit from an IInterationModuleBase
    public class ExampleModule : InteractionModuleBase<SocketInteractionContext>
    {
        // Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
        public InteractionService Commands { get; set; }
        private readonly ApplicationContext _db;
        private InteractionHandler _handler;

        private readonly DiscordSocketClient _client;

        // Constructor injection is also a valid way to access the dependencies
        public ExampleModule(InteractionHandler handler, ApplicationContext db, DiscordSocketClient client)
        {
            _handler = handler;
            _db = db;
            _client = client;
        }

        // You can use a number of parameter types in you Slash Command handlers (string, int, double, bool, IUser, IChannel, IMentionable, IRole, Enums) by default. Optionally,
        // you can implement your own TypeConverters to support a wider range of parameter types. For more information, refer to the library documentation.
        // Optional method parameters(parameters with a default value) also will be displayed as optional on Discord.

        // [Summary] lets you customize the name and the description of a parameter
        [SlashCommand("echo", "Repeat the input")]
        public async Task Echo(string echo, [Summary(description: "mention the user")]bool mention = false)
            => await RespondAsync(echo + (mention ? Context.User.Mention : string.Empty));

        [SlashCommand("ping", "Pings the bot and returns its latency.")]
        public async Task GreetUserAsync()
            => await RespondAsync(text: $":ping_pong: It took me {Context.Client.Latency}ms to respond to you!", ephemeral: true);



        [SlashCommand("solution", "Create a Solution")]
        public async Task AddSolution(string token, string description)
        {   

            // var message = Context.Message
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            token = token.Trim();
            Issue currentIssue = _db.Issues.FirstOrDefault(issue => issue.Token == token);
            if (token != null && token.Length == 10 && currentIssue != null)
            {
                await _db.Solutions.AddAsync( new Solution 
                {
                    Name = Context.User.Username,
                    Description = description,
                    IssueId = currentIssue.IssueId
                    
                });
                await _db.SaveChangesAsync();  
                sb.AppendLine();
                sb.AppendLine("**Solution Description**");
                sb.AppendLine(description);
                sb.AppendLine("**Solution Posted By**");
                sb.AppendLine(Context.User.Username);
                embed.Title = $"Solution to Issue: {currentIssue.Description}";
                embed.Description = sb.ToString();
                //await RespondAsync(text: "Solution created");

            }
            else
            {
                sb.AppendLine();
                sb.AppendLine("**Token:**");
                sb.AppendLine(token);
                sb.Append("** Not Found**");
                embed.Title = $"ERROR";
                embed.Description = sb.ToString();
                //await RespondAsync(text: "Solution not created");

            }
            //await RespondAsync(text: "Solution created", ephemeral: false);
            await RespondAsync(embed: embed.Build());
        }
        

        [MessageCommand("New Solution")]
        public async Task ReplyAsync(IMessage message)       
        {
            IReadOnlyCollection<IEmbed> embeds = message.Embeds;
            await RespondAsync($"WIP:Create a solution via Right click for {message.Author.Username}");
        }

        public async Task<EmbedBuilder> AddSolutionMain(string token, string description)
        {   
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            token = token.Trim();
            Issue currentIssue = _db.Issues.FirstOrDefault(issue => issue.Token == token);
            if (token != null && token.Length == 10 && currentIssue != null)
            {
                await _db.Solutions.AddAsync( new Solution 
                {
                    Name = Context.User.Username,
                    Description = description,
                    IssueId = currentIssue.IssueId
                    
                });
                await _db.SaveChangesAsync();  
                sb.AppendLine();
                sb.AppendLine("**Solution Description**");
                sb.AppendLine(description);
                sb.AppendLine("**Solution Posted By**");
                sb.AppendLine(Context.User.Username);
                embed.Title = $"Solution to Issue: {currentIssue.Description}";
                embed.Description = sb.ToString();
                //await RespondAsync(text: "Solution created");

            }
            else
            {
                sb.AppendLine();
                sb.AppendLine("**Token:**");
                sb.AppendLine(token);
                sb.Append("** Not Found**");
                embed.Title = $"ERROR";
                embed.Description = sb.ToString();
                //await RespondAsync(text: "Solution not created");

            }

        return embed;
        }
        
        [Group("issue", "This is a command group")]
        public class IssueCommands : InteractionModuleBase<SocketInteractionContext>
        {            
            private readonly ApplicationContext _db;
            public IssueCommands(InteractionHandler handler, ApplicationContext db)
            {
                _db = db;
            }
            // You can create command choices either by using the [Choice] attribute or by creating an enum. Every enum with 25 or less values will be registered as a multiple
            // choice option
            [SlashCommand("show-all", "list all questions")]
            public async Task ListIssueAsync()
            {            
                var sb = new StringBuilder();
                var embed = new EmbedBuilder();

                // get user info from the Context
                var user = Context.User;
                
                
                var issues = await _db.Issues.ToListAsync();
                if (issues.Count > 0)
                {
                    foreach (var issue in issues)
                    {
                        sb.AppendLine($":small_blue_diamond: [{issue.Token}]  **{issue.Name}**  asked: {issue.Subject}");
                    }
                }
                else
                {
                    sb.AppendLine("No answers found!");
                }

                // set embed
                embed.Title = "Issue List";
                embed.Description = sb.ToString();
                
                // send embed reply
                await RespondAsync(embed: embed.Build());
            }
        
            [SlashCommand("new", "Add a new issue")]
            public async Task AddIssue(string subject, string description)
            {            
                var sb = new StringBuilder();
                var embed = new EmbedBuilder();

                // get user info from the Context
                var user = Context.User;

                //token generation
                bool uniqueToken = false;
                string token = "";
                while (!uniqueToken)
                {
                    var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var random = new Random();
                    var resultToken = new string(
                        Enumerable.Repeat(allChar , 10)  
                        .Select(token => token[random.Next(token.Length)]).ToArray());
                    token = resultToken.ToString();
                    if(_db.Issues.Where(issue => issue.Token == token).Count() == 0)
                    {
                        uniqueToken = true;
                    }    

                }

                
                // add answer/color to table
                await _db.AddAsync(new Issue
                    {
                        Name  = user.Username,
                        Subject = subject,
                        Token = token,
                        Description = description                     
                    }
                );
                // save changes to database
                await _db.SaveChangesAsync();                
                sb.AppendLine();
                sb.AppendLine("**Subject:**");
                sb.AppendLine(subject);
                sb.AppendLine("**Issue Description:**");
                sb.AppendLine(description);
                sb.AppendLine();
                sb.AppendLine("**Asked By:**");
                sb.AppendLine(user.Username);
                sb.AppendLine("**Issue Token:**");
                sb.AppendLine(token);                

                // set embed
                embed.Title = "Issue Added";
                embed.Description = sb.ToString();
                
                // send embed reply
                await RespondAsync(embed: embed.Build());
                //await ReplyAsync(null, false, embed.Build());
            }

            [SlashCommand("show-by-description", "Search for issues by description")]
            public async Task ShowIssue(string description)
            {
                string[] descriptionArray = description.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                //show all issues
                // List<Issue> allIssues = _db.Issues.ToList();
                // List<Issue> issues = new List<Issue>();
                // int counter = 0 ;
                // foreach (Issue entry in allIssues)
                // {
                //     foreach (String descriptionEntry in descriptionArray)
                //     {
                //         if (entry.Description.Contains(descriptionEntry))
                //         {
                //             counter++;
                //         }
                //     }
                //     counters.Add(counter);
                //     counter = 0;
                // }
                
                

                Issue issue = new Issue();
                if (descriptionArray.Length == 1)
                {
                    issue = _db.Issues.FirstOrDefault(x => x.Description.Contains(descriptionArray[0]));

                }
                if (descriptionArray.Length > 1)
                {
                    List<Issue> issues = _db.Issues.ToList();
                    int counter = 0;
                    List<int> counters = new List<int>();
                foreach (Issue entry in issues)
                {
                    foreach (String descriptionEntry in descriptionArray)
                    {
                        if (entry.Description.Contains(descriptionEntry))
                        {
                            counter++;
                        }
                    }
                    counters.Add(counter);
                    counter = 0;
                }
                    int index = 0;
                    for (int i = 0; i < counters.Count; i++)
                    {
                        if (counter < counters[i])
                        {
                            counter = counters[i];
                            index = i;
                        }
                    }
                    issue = issues[index];
                }
                var sb = new StringBuilder();
                var embed = new EmbedBuilder();
                sb.AppendLine();
                sb.AppendLine("**Subject:**");
                sb.AppendLine(issue.Subject);
                sb.AppendLine("**Issue Description:**");
                sb.AppendLine(issue.Description);
                sb.AppendLine();
                sb.AppendLine("**Asked By:**");
                sb.AppendLine(issue.Name);
                sb.AppendLine("**Issue Token:**");
                sb.AppendLine(issue.Token);      

                embed.Title = $"This is the most relatable issue based on your description {description} entered";
                embed.Description = sb.ToString();
                await RespondAsync(embed: embed.Build());
            }
        }

        // // Use [ComponentInteraction] to handle message component interactions. Message component interaction with the matching customId will be executed.
        // // Alternatively, you can create a wild card pattern using the '*' character. Interaction Service will perform a lazy regex search and capture the matching strings.
        // // You can then access these capture groups from the method parameters, in the order they were captured. Using the wild card pattern, you can cherry pick component interactions.
        // [ComponentInteraction("musicSelect:*,*")]
        // public async Task ButtonPress(string id, string name)
        // {
        //     // ...
        //     await RespondAsync($"Playing song: {name}/{id}");
        // }

        // // Select Menu interactions, contain ids of the menu options that were selected by the user. You can access the option ids from the method parameters.
        // // You can also use the wild card pattern with Select Menus, in that case, the wild card captures will be passed on to the method first, followed by the option ids.
        // [ComponentInteraction("roleSelect")]
        // public async Task RoleSelect(string[] selections)
        // {
        //     throw new NotImplementedException();
        // }

        // // With the Attribute DoUserCheck you can make sure that only the user this button targets can click it. This is defined by the first wildcard: *.
        // // See Attributes/DoUserCheckAttribute.cs for elaboration.
        // [DoUserCheck]
        // [ComponentInteraction("myButton:*")]
        // public async Task ClickButtonAsync(string userId)
        //     => await RespondAsync(text: ":thumbsup: Clicked!");

        // // This command will greet target user in the channel this was executed in.
        // [UserCommand("greet")]
        // public async Task GreetUserAsync(IUser user)
        //     => await RespondAsync(text: $":wave: {Context.User} said hi to you, <@{user.Id}>!");

        // // Pins a message in the channel it is in.
        // [MessageCommand("pin")]
        // public async Task PinMessageAsync(IMessage message)
        // {
        //     // make a safety cast to check if the message is ISystem- or IUserMessage
        //     if (message is not IUserMessage userMessage)
        //         await RespondAsync(text: ":x: You cant pin system messages!");

        //     // if the pins in this channel are equal to or above 50, no more messages can be pinned.
        //     else if ((await Context.Channel.GetPinnedMessagesAsync()).Count >= 50)
        //         await RespondAsync(text: ":x: You cant pin any more messages, the max has already been reached in this channel!");

        //     else
        //     {
        //         await userMessage.PinAsync();
        //         await RespondAsync(":white_check_mark: Successfully pinned message!");
        //     }
        // }
    }
}