using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newTGBotForSEO
{
    internal class Constants
    {
        public const string BOT_HTTP_TOKEN = "6458280053:AAHUgIn1lIDExHn2shQvHxqtrfIaQLCQPt8";
        public const string INPUT_KEYS = "Введите ключи⬇";
        public const string url = "https://api.openai.com/v1/chat/completions";
        public const string OPEN_AI_API_KEY = "YOUR_OPEN_AI_KEY";
        public const string FOR_REQUEST = "Ты SEO специалист. Твоя задача создать описание" +
            " продукту используя данные ниже слова-ключи," +
            "Постарайся сделать описание ёмким и понятным." +
            "Вставь ключевые слова внутрь описания" +
            "Дополнительные условия: если в ключах есть города," +
            "то это место где продаётся продукт," +
            "сделай так, чтобы по этому описанию было проще найти продукт в" +
            "гугле, то есть поисковую оптимизацию" +
            "Не пиши ничего кроме описания +-25-30 слов. Первым " +
            "словом является название продукта остальное ключи." +
            " \n Данные: \n";

        public const string NEXT = "Далее";
    }
}
