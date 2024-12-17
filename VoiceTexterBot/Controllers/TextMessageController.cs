using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VoiceTexterBot.Models;

namespace VoiceTexterBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;

        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            // проверка переключателя

            if (Session.isLenght && message.Text != "/start") {
                await _telegramClient.SendTextMessageAsync(message.From.Id, $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct);
                return;
            }

            if (Session.isSum && message.Text != "/start")
            {
                List<List<Char>> numbers = new List<List<Char>>();
                numbers.Add(new List<Char>());
                foreach (var i in message.Text)
                {
                    if (i == ' ')
                    {
                        numbers.Add(new List<Char>());
                    }
                }
               

                int x = 0;
                foreach (var i in message.Text)
                {
                    if (i != ' ')
                    {
                        numbers[x].Add(i);
                    }
                    else
                    {
                        x++;
                    }
                }

                int sum = 0;

                foreach (var i in numbers) {
                    string a = "";
                    foreach (var j in i) {
                        a += j;
                    }
                    
                    int number = int.Parse(a);
                    sum += number;
                }


                await _telegramClient.SendTextMessageAsync(message.From.Id, $"Cумма чисел {sum}", cancellationToken: ct);
                return;
            }


            switch (message.Text)
            {
                case "/start":
                    //Обьект, предствляющий кнопки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Русский", $"ru"),
                        InlineKeyboardButton.WithCallbackData($"English", $"en"),
                        InlineKeyboardButton.WithCallbackData($"Italiano", $"it"),
                        InlineKeyboardButton.WithCallbackData($"Длина сообщения", $"lenth"),
                        InlineKeyboardButton.WithCallbackData($"Сумма чисел", $"sum"),
                    });
                    var a = new InlineKeyboardMarkup(buttons);

                    //передаем кнопки вместе с сообщением (параметр PeplyMarkup)
                    await
                        _telegramClient.SendTextMessageAsync(message.Chat.Id,
                        $"<b> Наш бот превращает аудио в текст.</b> {Environment.NewLine}" + 
                        $"{Environment.NewLine} Можно записать сообщение и переслать другу, если лень печатать.{Environment.NewLine}", messageThreadId: null,
                        parseMode: ParseMode.Html, cancellationToken: ct, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                default:
                    await
                        _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте аудио для превращения в текст.", cancellationToken: ct);
                    break;
            }

            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            //await
            //    _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено текстовое сообщение", cancellationToken: ct);
        }
    }
}
