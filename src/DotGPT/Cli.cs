using DotGPT.Models.Requests;
using Spectre.Console;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

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
            if (arguments.Length == 0)
            {
                await RunInteractiveMode();
            }
            else if (arguments.Length == 1 && arguments.First() is "--advanced" or "-a")
            {
                var request = BuildChatGptRequestWithUserParameters();
                var response = await GetResponseAsync(request);
                AnsiConsole.MarkupLine($"[darkslategray1]{response}[/]");
            }
            else if (arguments.Length == 1 && (arguments.First() is "--hel" or "-h"))
            {
                DisplayHelp();
                return;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Wrong command! Try again with diffrent command or use --help argument![/]");
            }
        }

        public static string GetOpenAiToken()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Currently we are only supporting Windows. Feel free to contribute to our project!");
            }
            try
            {
                byte[] encryptedSecretFromFile = File.ReadAllBytes("dotgptsecrets.bin");
                string decryptedSecret = System.Text.Encoding.Unicode
                    .GetString(ProtectedData.Unprotect(encryptedSecretFromFile, null, DataProtectionScope.LocalMachine));
                if (string.IsNullOrEmpty(decryptedSecret))
                {
                    return string.Empty;
                }
                return decryptedSecret;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string SetOpenAiToken()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Currently we are only supporting Windows. Feel free to contribute to our project!");
            }
            Console.Write("Place your api token here:");
            string secret = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(secret))
            {
                Console.WriteLine("Error");
                return string.Empty;
            }
            byte[] encryptedSecret = ProtectedData
                .Protect(System.Text.Encoding.Unicode.GetBytes(secret), null, DataProtectionScope.LocalMachine);
            File.WriteAllBytes("dotgptsecrets.bin", encryptedSecret);
            return secret;
        }

        public void DisplayHelp()
        {
            Console.WriteLine("Usage: dotgpt [OPTIONS] INPUT");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("-h, --help       Display this message");
            Console.WriteLine("-a, --advanced   Run advanced mode");
        }

        public async Task RunInteractiveMode()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Prompt: ");
                Console.ResetColor();
                string prompt = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(prompt))
                {
                    PrintColoredTextToConsole("Error! Please write something to text prompt before submiting!", ConsoleColor.Red);
                    continue;
                }
                var request = new ChatGptRequest(prompt);
                var response = await GetResponseAsync(request);
                PrintColoredTextToConsole(response, ConsoleColor.Gray);
            }
        }

        public void PrintColoredTextToConsole(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
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
            double temperature = AnsiConsole.Prompt(new TextPrompt<double>("[seagreen2]Temperature:[/] "));
            int maxTokens = AnsiConsole.Prompt(new TextPrompt<int>("[seagreen2]Max tokens:[/] "));
            int topP = AnsiConsole.Prompt(new TextPrompt<int>("[seagreen2]Top P:[/] "));
            int frequency = AnsiConsole.Prompt(new TextPrompt<int>("[seagreen2]Frequency:[/] "));
            int presence = AnsiConsole.Prompt(new TextPrompt<int>("[seagreen2]Presence:[/] "));

            return new ChatGptRequest(model, prompt, temperature, maxTokens, topP, frequency, presence);
        }



        public ChatGptRequest BuildChatGptRequestWithDefaultValues(string prompt)
        {
            return new ChatGptRequest(prompt);
        }
    }
}
