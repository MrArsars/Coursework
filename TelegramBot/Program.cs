using Coursework.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("6733501005:AAHx9xCPQ0KOK_KHpR_l0u8rbRl8PrVPack");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;

    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    var buttons = new string[] { "Disable security", "Enable security" };
    
    using (var httpClient = new HttpClient())
    {
        // Specify the URL you want to make a request to
        const string url = "http://localhost:7002/MotionSensor/get";

        // Make a GET request
        var response = await httpClient.GetAsync(url, cancellationToken);

        // Check if the request was successful (status code 200 OK)
        if (response.IsSuccessStatusCode)
        {
            // Read and output the content of the response
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            Console.WriteLine(content);
        }
        else
        {
            // Handle the error, if any
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }
    }
    

    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
    {
        new KeyboardButton[] { SystemState.GetState() ? buttons[0] : buttons[1] },
    })
    {
        ResizeKeyboard = true
    };

    if (buttons.Any(x => x.Equals(messageText)))
    {
        using (var httpClient = new HttpClient())
        {
            // Specify the URL you want to make a request to
            const string url = "http://localhost:7002/MotionSensor/switch";

            // Make a GET request
            var response = await httpClient.GetAsync(url, cancellationToken);

            // Check if the request was successful (status code 200 OK)
            if (response.IsSuccessStatusCode)
            {
                // Read and output the content of the response
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                Console.WriteLine(content);
            }
            else
            {
                // Handle the error, if any
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        var sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: messageText.Equals(buttons[0]) ? "Disabled!" : "Enabled!",
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