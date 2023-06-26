using BuildABot.Models;
using BuildABot.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BuildABot
{
    public class Program
    {
        private static IConfigurationRoot Configuration;
        private static BotService botService = new();

        private static void Main(string[] args)
        {
            // Setup configuration builder with appsettings.json
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false);

            // Read appsettings.json into Configuration
            Configuration = builder.Build();

            // Retrieve RUNNER_IPV4 environment variable.
            var environmentIp = Environment.GetEnvironmentVariable("RUNNER_IPV4");
            // Use RUNNER_IPV4 environment variable if it exists,
            // else read the value in appsettings.json
            var runnerIp = !string.IsNullOrWhiteSpace(environmentIp)
                ? environmentIp
                : Configuration.GetSection("RunnerIP").Value!;
            // Add http:// to start if not already present.
            if (!runnerIp.StartsWith("http://"))
            {
                runnerIp = $"http://{runnerIp}";
            }

            // Retrieve bot nickname from either environment variable or appsettings.json
            var botNickname = Environment.GetEnvironmentVariable("BOT_NICKNAME")
                ?? Configuration.GetSection("BotNickname").Value;

            // Get registration token from environment.
            var token = Environment.GetEnvironmentVariable("Token");
            // Get runner hub port
            var port = Configuration.GetSection("RunnerPort").Value;
            // Build runner hub connection URL.
            var runnerHubUrl = $"{runnerIp}:{port}/runnerhub";

            // Build SignalR connection
            var connection = new HubConnectionBuilder()
                .WithUrl(runnerHubUrl)
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .WithAutomaticReconnect()
                .Build();

            // Start connection
            connection.StartAsync().Wait();
            Console.WriteLine("Connected to Runner!");

            // Listen for registered event.
            connection.On<Guid>("Registered", (id) =>
            {
                Console.WriteLine($"Bot Registered with ID: {id}");
                botService.SetBotId(id);
            });

            // Print bot state.
            connection.On<BotStateDTO>("ReceiveBotState", (botState) =>
            {
                Console.WriteLine(botState.ToString());
                // Have bot service process the bot state.
                BotCommand botCommand = botService.ProcessState(botState);
                connection.InvokeAsync("SendPlayerCommand", botCommand);
            });

            // Register bot.
            connection.InvokeAsync("Register", token, botNickname).Wait();

            // Loop while connected.
            while (connection.State == HubConnectionState.Connected || connection.State == HubConnectionState.Connecting || connection.State == HubConnectionState.Reconnecting)
            {
                Thread.Sleep(300);
            }
        }
    }
}