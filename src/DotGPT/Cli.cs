using DotGPT.Models.Requests;

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
            if (arguments.Length > 0)
            {
                prompt = arguments.First();
                if (true)
                {
                    BuildChatGptRequest(prompt);
                }
            }
            Console.WriteLine(await chatGpt3Client.GetResponseAsync(prompt));
        }

        public async Task RunInteractiveMode()
        {
            while (true)
            {
                Console.Write("Prompt: ");
                string prompt = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(prompt))
                {
                    Console.WriteLine("Error!");
                }
                Console.WriteLine(await chatGpt3Client.GetResponseAsync(prompt));
            }
        }
        public async Task RunAdvancedMode()
        {

        }

        public ChatGptRequest BuildChatGptRequest(string prompt)
        {
            return new ChatGptRequest("");
        }
    }
}
