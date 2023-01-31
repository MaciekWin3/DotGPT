using DotGPT.Models.Requests;
using Spectre.Console;

namespace DotGPT
{
    public class Cli
    {
        private readonly ChatGpt3Client chatGpt3Client;
        public Cli(ChatGpt3Client chatGpt3Client)
        {
            this.chatGpt3Client = chatGpt3Client;
        }

        public async Task Run(string[] arguments)
        {
            string prompt = string.Empty;
            if (arguments.Length == 0)
            {
                await RunInteractiveMode();
            }
            if (arguments.Length == 1 && arguments.First() == "--advanced")
            {
                prompt = arguments.First();
                var request = BuildChatGptRequestWithUserParameters();
                var response = await GetResponseAsync(request);
                AnsiConsole.MarkupLine($"[darkslategray1]{response}[/]");

            }
            else if (arguments.Length == 1)
            {
                Console.Write(arguments[0]);
            }
            //AnsiConsole.WriteLine(await chatGpt3Client.GetResponseAsync(BuildChatGptRequestWithDefaultValues(prompt)));
        }

        public async Task RunInteractiveMode()
        {
            while (true)
            {
                string prompt = AnsiConsole.Ask<string>("[seagreen2]Prompt:[/] ");
                if (string.IsNullOrEmpty(prompt))
                {
                    AnsiConsole.WriteLine("[red] Error! Please write something to text prompt before submiting!");
                    continue;
                }
                var request = new ChatGptRequest(prompt);
                var response = await GetResponseAsync(request);

                AnsiConsole.MarkupLine($"[darkslategray1]{response}[/]");
            }
        }

        public async Task PrintAnswerToConsole()
        {

        }

        public async Task<string> GetResponseAsync(ChatGptRequest request)
        {
            string response = string.Empty;
            await AnsiConsole.Status()
                .StartAsync("Waiting for ChatGPT response...", async ctx =>
                {
                    response = await chatGpt3Client.GetResponseAsync(request);
                    response = response.Trim();
                });

            if (string.IsNullOrEmpty(response))
            {
                return string.Empty;
            }
            return response;
        }

        public ChatGptRequest BuildChatGptRequestWithUserParameters()
        {
            var models = new string[]
            {
                "text-ada-01",
                "text-babbage-001",
                "text-curie-001",
                "text-davinci-003"
            };

            string model = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[seagreen2]Model[/]")
                    .AddChoices(models));
            string prompt = AnsiConsole.Ask<string>("[seagreen2]Prompt:[/] ");

            double temperature = AnsiConsole.Prompt(
                new TextPrompt<double>("Temperature:")
                    .PromptStyle("seagreen2")
                    .ValidationErrorMessage("[red]Temperature should be value between 0.0 and 1.0[/]")
                    .Validate(t =>
                    {
                        return t switch
                        {
                            < 0 => ValidationResult.Error("[red]Temperature value is too low[/]"),
                            > 1 => ValidationResult.Error("[red]Temperature value is too high[/]"),
                            _ => ValidationResult.Success()
                        };
                    }));

            int maxTokens = AnsiConsole.Prompt(new TextPrompt<int>("[seagreen2]Prompt:[/] "));
            int topP = AnsiConsole.Prompt(new TextPrompt<int>("[seagreen2]Prompt:[/] "));
            int frequency = AnsiConsole.Prompt(new TextPrompt<int>("[seagreen2]Prompt:[/] "));
            int presence = AnsiConsole.Prompt(new TextPrompt<int>("[seagreen2]Prompt:[/] "));

            return new ChatGptRequest(model, prompt, temperature, maxTokens, topP, frequency, presence);
        }

        public ChatGptRequest BuildChatGptRequestWithDefaultValues(string prompt)
        {
            return new ChatGptRequest(prompt);
        }
    }
}
