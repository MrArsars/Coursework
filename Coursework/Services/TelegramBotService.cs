using Coursework.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Coursework.Services;

public class TelegramBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TelegramBotService()
    {
        _botClient = new TelegramBotClient("6733501005:AAHx9xCPQ0KOK_KHpR_l0u8rbRl8PrVPack");
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async Task StartAsync()
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: _cancellationTokenSource.Token
        );

        var me = await _botClient.GetMeAsync();
        Console.WriteLine($"Start listening for @{me.Username}");
    }

    const int chatId = 685732650;
    
    public async Task SendMessageAsync()
    {
        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Зафіксовано рух!");
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var buttons = new string[] { "Вимкнути систему", "Увімкнути систему" };
        var state = SystemState.GetState();

        if (messageText == "/start")
        {
            var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { state ? buttons[0] : buttons[1] },
            })
            {
                ResizeKeyboard = true
            };

            var sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Вітаю!",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }

        if (buttons.Any(x => x.Equals(messageText)))
        {
            state = SystemState.SwitchState();
            var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { state ? buttons[0] : buttons[1] },
            })
            {
                ResizeKeyboard = true
            };
            var sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: messageText.Equals(buttons[0]) ? "Вимкнено!" : "Увімкнено!",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }
    }

    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}