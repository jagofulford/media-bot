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
using media_bot.Repositories;
using Telegram.Bot.Types.ReplyMarkups;

namespace media_bot.Workers
{
    class MainWorker : BackgroundService
    {
        static ITelegramBotClient botClient;
        private readonly ILogger<MainWorker> _logger;
        private TelegramBotConfig _telegramBotConfig;
        static private IMediaRepository _tvShows;
        public MainWorker(ILogger<MainWorker> logger)
        {
            _logger = logger;
            _tvShows = new SonarrRepository();
            logger.LogInformation("Generating config from the appsettings config file");
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(TelegramBotConfig));
            _telegramBotConfig = section.Get<TelegramBotConfig>();

            botClient = new TelegramBotClient(_telegramBotConfig.ApiKey);
            botClient.OnMessage += Bot_OnMessage;
            botClient.OnCallbackQuery += Bot_OnCallback;
 
        }

        static async private void Bot_OnCallback(object sender, CallbackQueryEventArgs e)
        {
            await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Test");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            botClient.StartReceiving();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            botClient.StopReceiving();
        }
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            
            if (e.Message.Text != null && e.Message.Text.Length > 1)
            {
                var firstWordEndPosition = e.Message.Text.IndexOf(" ") >= 0 ? e.Message.Text.IndexOf(" ") : e.Message.Text.Length;
                var command = e.Message.Text.Substring(0, firstWordEndPosition).ToLower();
                var commandText = e.Message.Text[firstWordEndPosition..];
                switch (command[..1]) {

                    case "/": 
                        switch (command)
                        {
                            case "/help":
                                await botClient.SendTextMessageAsync(
                                    chatId: e.Message.Chat,
                                    text: "Welcome to the Media Bot, you can use the following commands:\n /Search - finds movies or tv"
                                );
                                break;
                            case "/search":
                                if (e.Message.Text[firstWordEndPosition..].Trim().Length > 0)
                                {
                                    var results = _tvShows.SearchMedia(commandText);
                                    var buttons = new List<InlineKeyboardButton>();
                                    
                                    foreach (var result in results)
                                    {
                                        InlineKeyboardButton resultButton = InlineKeyboardButton.WithCallbackData(result.Value.GetMediaDisplayTitle(), result.Key);
                                        buttons.Add(resultButton);
                                    }
                                    InlineKeyboardButton button = InlineKeyboardButton.WithCallbackData("END", "END");
                                    buttons.Add(button);
                                    InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);
                                    await botClient.SendTextMessageAsync(
                                        chatId: e.Message.Chat,
                                        text: "You searched for: " + commandText + "\nI found " + results.Count.ToString() + " TV show" + (results.Count > 1 ? "s" : ""), 
                                        replyMarkup: keyboard
                                    );
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(
                                        chatId: e.Message.Chat,
                                        text: "A search term is needed to search"
                                    );
                                }
                                break;
                            default:
                                await botClient.SendTextMessageAsync(
                                    chatId: e.Message.Chat,
                                    text: "There is no command available for:\n" + command
                                );
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
