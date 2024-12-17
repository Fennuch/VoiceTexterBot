using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBot.Models;
using VoiceTexterBot.Services;

namespace VoiceTexterBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if(callbackQuery?.Data == null)
                return;
            //Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = callbackQuery.Data;

            //генерим информационное сообщение
            string languageText = callbackQuery.Data switch
            {
                "lenth" => "Длина сообщения",
                "sum" => "Сумма чисел",
                "ru" => "Русский",
                "en" => "Английский",
                "fr"=>"Итальянский",
                _ => String.Empty
            };

            if (languageText != "Длина сообщения" && languageText != "Сумма чисел") {
                //отправляем в ответ уведомление о выборе
                await
                    _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"<b>Язык аудио - {languageText}.{Environment.NewLine}</b>" + $"{Environment.NewLine} Можно поменять в главном меню.", cancellationToken: ct, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            }

            if (languageText == "Длина сообщения")
            {
                Session.isLenght = true;
                Session.isSum = false;
            }
            else if (languageText == "Сумма чисел")
            {
                Session.isLenght = false;
                Session.isSum = true;
            }
            else {
                Session.isLenght = false;
                Session.isSum = false;
            }

            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");

            await
                _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, 
                $"Обнаружено нажатие на кнопку {callbackQuery.Data}", 
                cancellationToken: ct);
        }
    }
}
