using BuildABot.Enums;
using BuildABot.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

Guid botId;

IConfigurationRoot configuration;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false);
configuration = builder.Build();

var runnerIp = Environment.GetEnvironmentVariable("RUNNER_IPV4")
 ?? configuration.GetSection("RunnerIP").Value!;

if (!runnerIp.StartsWith("http://"))
{
    runnerIp = $"http://{runnerIp}";
}

var botNickname = Environment.GetEnvironmentVariable("BOT_NICKNAME")
    ?? configuration.GetSection("BotNickname").Value;

var token = Environment.GetEnvironmentVariable("Token");
var port = configuration.GetSection("RunnerPort").Value;
var runnerHubUrl = $"{runnerIp}:{port}/runnerhub";

var connection = new HubConnectionBuilder()
    .WithUrl(runnerHubUrl)
    .ConfigureLogging(logging =>
    {
        logging.SetMinimumLevel(LogLevel.Debug);
    })
    .WithAutomaticReconnect()
    .Build();

connection.StartAsync().Wait();
Console.WriteLine("Connected to Runner!");


connection.On<Guid>("Registered", (id) =>
{
    Console.WriteLine($"Bot Registered with ID: {id}");
    botId = id;

    connection.On<BotStateDTO>("ReceiveBotState", (botState) =>
    {
        Console.WriteLine(botState.ToString());
        BotCommand command = new BotCommand() { Action = (int)BotAction.Right, BotId = botId };
        connection.InvokeAsync("SendPlayerCommand", command).Wait();
    });
});

connection.InvokeAsync("Register", token, botNickname).Wait();

while (connection.State is HubConnectionState.Connected or HubConnectionState.Connecting or HubConnectionState.Reconnecting)
{
    Thread.Sleep(300);
}
