using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace newTGBotForSEO
{
    class Program
    {
        static string botTokenHTTP = "6458280053:AAHUgIn1lIDExHn2shQvHxqtrfIaQLCQPt8";
        static TelegramBotClient botClient = new TelegramBotClient(botTokenHTTP);
        static bool CHECKER = false;
        static string CALL_BACK = "";
        static async Task Main(string[] args)
        {
            var me = await botClient.GetMeAsync();
            botClient.StartReceiving(Update, Error);
            Console.ReadLine();
        }
        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (CALL_BACK == "new request")
            {
                await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: Constants.INPUT_KEYS
                        );
                CALL_BACK = "start";
                return;
            }

            if (message.Text == null) return;

            switch (message.Text)
            {
                case "/start":
                    CALL_BACK = "start";
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: Constants.INPUT_KEYS
                        );
                    return;
            }

            if (CALL_BACK == "start")
            {
                var messages = new List<dynamic>
                {
                    new {role = "system",
                        content = $"{Constants.FOR_REQUEST}"},
                    new {role = "assistant",
                        content = "Подождите секунду... \n" +
                                    "Формирую ответ...."}
                };

                string answer = messages[1].content;

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: answer
                    );

                // Capture the users messages and add to
                // messages list for submitting to the chat API
                var userMessage = message.Text;
                messages.Add(new { role = "user", content = userMessage });

                // Create the request for the API sending the
                // latest collection of chat messages
                var request = new
                {
                    messages,
                    model = "gpt-3.5-turbo",
                    max_tokens = 150,
                };

                // Send the request and capture the response
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Constants.OPEN_AI_API_KEY}");
                var requestJson = JsonConvert.SerializeObject(request);
                var requestContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                var httpResponseMessage = await httpClient.PostAsync(Constants.url, requestContent);
                var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeAnonymousType(jsonString, new
                {
                    choices = new[] { new { message = new { role = string.Empty, content = string.Empty } } },
                    error = new { message = string.Empty }
                });


                if (!string.IsNullOrEmpty(responseObject?.error?.message))  // Check for errors
                {
                    await botClient.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: responseObject?.error.message
                   );
                }
                else  // Add the message object to the message collection
                {
                    var messageObject = responseObject?.choices[0].message;
                    await botClient.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: messageObject.content,
                   replyMarkup: new ReplyKeyboardMarkup(Constants.NEXT) { ResizeKeyboard = true }
                   );
                }
                await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                CALL_BACK = "new request";
                return;
            }
        }
        private static Task Error(ITelegramBotClient botClient, Exception update, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}