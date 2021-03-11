using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using media_bot.Models;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Args;

namespace media_bot.Workers
{
    class MainWorker : BackgroundService
    {
        static ITelegramBotClient botClient;
        private readonly ILogger<MainWorker> _logger;
        private TelegramBotConfig _telegramBotConfig;
        private IMediaRepository _tvShows;
        public MainWorker(ILogger<MainWorker> logger)
        {
            _logger = logger;
            logger.LogInformation("Generating config from the appsettings config file");
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(TelegramBotConfig));
            _telegramBotConfig = section.Get<TelegramBotConfig>();

            botClient = new TelegramBotClient(_telegramBotConfig.ApiKey);

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            botClient.StopReceiving();
        }
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            
            if (e.Message.Text != null && e.Message.Text.Length > 1 && e.Message.Text.IndexOf(" ") >= 0)
            {
                var command = e.Message.Text.Substring(0, e.Message.Text.IndexOf(" "));
                switch (command.Substring(0, 1)) {

                    case "/": 
                        Console.Write("Command sent");
                        await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Your command was:\n" + command
                        );
                        break;
                    default:
                        Console.Write("Something else sent");
                        break;
                }
            }
        }
    }
}
