using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceTexterBot.Configuration;
using VoiceTexterBot.Controllers;
using VoiceTexterBot.Services;

namespace VoiceTexterBot
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            //Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build();//Собираем

            Console.WriteLine("Сервис запущен");
            //// Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");

            static void ConfigureServices(IServiceCollection services)
            {
                AppSettings appSettings = BuildAppSettings();
                services.AddSingleton(appSettings);

                services.AddSingleton<IStorage, MemoryStorage>();
                services.AddSingleton<IFileHandler, AudioFileHandler>();

                //Подключаем контроллеры сообщений и кнопок
                services.AddTransient<DefaultMessageController>();
                services.AddTransient<VoiceMessageController>();
                services.AddTransient<TextMessageController>();
                services.AddTransient<InlineKeyboardController>();

                

                //Регистрируем объект TelegramBotClient c токеном подключения
                services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("7737505519:AAGKz4ERxIlcU7_OapDzT1ggmgI-ldbGA9M"));
                //Регистрируем постоянно активный сервис бота
                services.AddHostedService<Bot>();
            }

            static AppSettings BuildAppSettings()
            {
                return new AppSettings()
                {
                    DownloadFolder = "C:\\Users\\Feona\\source\\repos\\VoiceTexterBot",
                    BotToken = "7737505519:AAGKz4ERxIlcU7_OapDzT1ggmgI-ldbGA9M",
                    AudioFileName = "audio",
                    InputAudioFormat = "ogg",
                    OutputAudioFormat = "wav",
                };
            }

            

        }
    }
}