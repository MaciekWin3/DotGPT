using DotGPT.Models.Requests;
using System.Text;
using System.Text.Json;

namespace DotGPT
{
    public class ChatGpt3Client
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ChatGpt3Client(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
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
            }
            Console.WriteLine(await GetResponseAsync(prompt));
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
                Console.WriteLine(await GetResponseAsync(prompt));
            }
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var httpClient = httpClientFactory.CreateClient("chatgptapi");
            var request = new ChatGptRequest(prompt);
            //var x = JsonSerializer.Serialize(request);
            //var response = await httpClient.PostAsJsonAsync("v1/completions", x);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("v1/completions", content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            string jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ChatGptResponse>(jsonString);
            if (result is null)
            {
                Console.WriteLine("xd");
            }
            return result.Choices.FirstOrDefault().Text;
        }
    }
}