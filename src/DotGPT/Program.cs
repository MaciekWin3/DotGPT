using DotGPT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = new ConfigurationBuilder();

builder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builder.Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<ChatGpt3Client>();
        services.AddTransient<Cli>();

        services.AddHttpClient("chatgptapi", client =>
        {
            client.BaseAddress = new Uri("https://api.openai.com/");
            client.DefaultRequestHeaders.Add("authorization", $"Bearer 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");
        });

#if DEBUG
        services.AddLogging(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Debug)
                .AddFilter("NToastNotify", LogLevel.Warning)
                .AddConsole();
        });
#endif
    }).Build();

var svc = ActivatorUtilities.CreateInstance<Cli>(host.Services);
await svc.Run(args);