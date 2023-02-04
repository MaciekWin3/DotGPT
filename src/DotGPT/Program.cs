using DotGPT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = new ConfigurationBuilder();

string token = Cli.GetOpenAiToken();
if (string.IsNullOrEmpty(token))
{
    token = Cli.SetOpenAiToken();
}

var configuration = builder.Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<ChatGpt3Client>();
        services.AddTransient<Cli>();

        services.AddHttpClient("chatgptapi", client =>
        {
            client.BaseAddress = new Uri("https://api.openai.com/");
            client.DefaultRequestHeaders.Add("authorization", $"Bearer {token}");
        });

        services.AddLogging(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("NToastNotify", LogLevel.Warning)
                .AddConsole();
        });
    }).Build();

var svc = ActivatorUtilities.CreateInstance<Cli>(host.Services);
await svc.Run(args);