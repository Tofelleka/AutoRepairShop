namespace AutoRepairShop
{
    internal static class Accents
    {
        public static string GetFriendlyGreeting(Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Ukrainian:
                    return "О, хлопцы, заходите!";

                case Nationality.Belarusian:
                    return "Заходзь, брат!";

                case Nationality.Tatar:
                    return "Исәнмесез, друг, заходи!";

                case Nationality.Armenian:
                    return "Вай, дорогой, заходи, садись!";

                case Nationality.Georgian:
                    return "Гамарджоба, брат, заходи!";

                case Nationality.Uzbek:
                    return "Салом, брат, заходи!";

                case Nationality.German:
                    return "Guten Tag! Заходи, всё готово.";

                case Nationality.Chechen:
                    return "Де дика хуьлда, брат, заходи!";

                case Nationality.Yakut:
                    return "Ой, заходи, однако!";

                default:
                    return "Братан, заходи!";
            }
        }

        public static string GetColdGreeting(Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Ukrainian:
                    return "Ну, побачим, шо скажешь.";

                case Nationality.Belarusian:
                    return "Ну, пабачым, што будзе.";

                case Nationality.Tatar:
                    return "Ну, посмотрим, да.";

                case Nationality.Armenian:
                    return "Вай, ну посмотрим, дорогой.";

                case Nationality.Georgian:
                    return "Вах, ну посмотрим, брат.";

                case Nationality.Uzbek:
                    return "Ну, посмотрим, брат.";

                case Nationality.German:
                    return "Na, посмотрим. Ordnung ist Ordnung.";

                case Nationality.Chechen:
                    return "Ну, посмотрим, брат.";

                case Nationality.Yakut:
                    return "Ну, посмотрим, однако.";

                default:
                    return "Ну, посмотрим, что скажете.";
            }
        }

        public static string GetFriendlyFarewell(Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Ukrainian:
                    return "Бувай, хлопцы, ще зайду!";

                case Nationality.Belarusian:
                    return "Да пабачэння, брат!";

                case Nationality.Tatar:
                    return "Сау бул, друг, ещё приеду!";

                case Nationality.Armenian:
                    return "До встречи, дорогой, спасибо!";

                case Nationality.Georgian:
                    return "Вах, до встречи, брат!";

                case Nationality.Uzbek:
                    return "Хайр, брат, ещё приеду!";

                case Nationality.German:
                    return "Auf Wiedersehen! Всё по порядку.";

                case Nationality.Chechen:
                    return "Маршалла, брат, до встречи!";

                case Nationality.Yakut:
                    return "Көрсүөххэ, однако!";

                default:
                    return "До встречи, брат!";
            }
        }

        public static string GetColdFarewell(Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Ukrainian:
                    return "Ну, бувай, якщо шо.";

                case Nationality.Belarusian:
                    return "Ну, бывай.";

                case Nationality.Tatar:
                    return "Ну, сау бул, да.";

                case Nationality.Armenian:
                    return "Ну, давай, дорогой.";

                case Nationality.Georgian:
                    return "Ну, давай, брат.";

                case Nationality.Uzbek:
                    return "Ну, хайр, брат.";

                case Nationality.German:
                    return "Na, до свидания.";

                case Nationality.Chechen:
                    return "Ну, бывай, брат.";

                case Nationality.Yakut:
                    return "Ну, бывай, однако.";

                default:
                    return "Ну, бывайте.";
            }
        }

        public static string GetDiscountThanks(Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Ukrainian:
                    return "Оце по-нашему, спасибо, хлопцы!";

                case Nationality.Belarusian:
                    return "Дзякуй, гэта па-людску!";

                case Nationality.Tatar:
                    return "Рәхмәт, уважил, да!";

                case Nationality.Armenian:
                    return "Вай, спасибо, дорогой, уважил!";

                case Nationality.Georgian:
                    return "Вах, спасибо, брат, уважил!";

                case Nationality.Uzbek:
                    return "Рахмат, брат, уважил!";

                case Nationality.German:
                    return "Danke, это по-человечески.";

                case Nationality.Chechen:
                    return "Баркалла, брат, уважил!";

                case Nationality.Yakut:
                    return "Ой, спасибо, однако, уважил!";

                default:
                    return "Спасибо, уважили!";
            }
        }

        public static string GetGreeting(Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Ukrainian:
                    return "Здоровеньки булы!";

                case Nationality.Belarusian:
                    return "Добры дзень!";

                case Nationality.Tatar:
                    return "Исәнмесез!";

                case Nationality.Armenian:
                    return "Слушай, дорогой, здравствуй!";

                case Nationality.Georgian:
                    return "Гамарджоба, брат!";

                case Nationality.Uzbek:
                    return "Салом, брат!";

                case Nationality.German:
                    return "Guten Tag!";

                case Nationality.Chechen:
                    return "Де дика хуьлда!";

                case Nationality.Yakut:
                    return "Эҕэрдэ!";

                default:
                    return "Здравствуйте.";
            }
        }

        public static string GetSuffix(Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Ukrainian:
                    return "От такэ, хлопцы.";

                case Nationality.Belarusian:
                    return "Вось такія справы.";

                case Nationality.Tatar:
                    return "Әйбәт, да.";

                case Nationality.Armenian:
                    return "Вай, какой машина, да!";

                case Nationality.Georgian:
                    return "Вах, хорошо сказал!";

                case Nationality.Uzbek:
                    return "Якши, да?";

                case Nationality.German:
                    return "Ordnung muss sein.";

                case Nationality.Chechen:
                    return "Баркалла, брат.";

                case Nationality.Yakut:
                    return "Ой, хорошо, однако.";

                default:
                    return string.Empty;
            }
        }

        public static string GetModelAttitude(Nationality nationality, string model)
        {
            switch (nationality)
            {
                case Nationality.Ukrainian:
                    return "А вот про " + model + ", хлопцы:";

                case Nationality.Belarusian:
                    return "Што да " + model + ", скажу так:";

                case Nationality.Tatar:
                    return model + "? Вот что скажу:";

                case Nationality.Armenian:
                    return "Слушай, дорогой, " + model + " — это:";

                case Nationality.Georgian:
                    return "Вах, про " + model + " скажу:";

                case Nationality.Uzbek:
                    return model + ", брат, вот такое дело:";

                case Nationality.German:
                    return "Про " + model + ":";

                case Nationality.Chechen:
                    return "Про " + model + ", брат:";

                case Nationality.Yakut:
                    return model + "... однако:";

                default:
                    return "Про " + model + ":";
            }
        }

        public static string Distort(string text, Nationality nationality)
        {
            string result = Boldify(text, nationality);

            switch (nationality)
            {
                case Nationality.Ukrainian:
                    result = result.Replace("что", "шо").Replace("это", "цэ").Replace("если", "якщо");
                    result = result.Replace("очень", "дуже").Replace("сейчас", "зараз");
                    break;

                case Nationality.Belarusian:
                    result = result.Replace("что", "што").Replace("это", "гэта").Replace("если", "калі");
                    result = result.Replace("очень", "ведзьма");
                    break;

                case Nationality.Tatar:
                    result = result.Replace("хорошо", "яхшы").Replace("спасибо", "рәхмәт");
                    result = result.Replace("машина", "машина, да");
                    break;

                case Nationality.Armenian:
                    result = result.Replace("машина", "машина, да").Replace("хорошо", "хорошо, дорогой");
                    break;

                case Nationality.Georgian:
                    result = result.Replace("машина", "машина, брат").Replace("хорошо", "хорошо, вах");
                    break;

                case Nationality.Uzbek:
                    result = result.Replace("хорошо", "яхши").Replace("машина", "машина, брат");
                    break;

                case Nationality.German:
                    result = result.Replace("хорошо", "sehr gut").Replace("машина", "Auto");
                    break;

                case Nationality.Chechen:
                    result = result.Replace("спасибо", "баркалла").Replace("машина", "машина, брат");
                    break;

                case Nationality.Yakut:
                    result = result.Replace("хорошо", "хорошо, однако");
                    break;
            }

            return result;
        }

        private static string Boldify(string text, Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Armenian:
                case Nationality.Georgian:
                    return SwapFirstTwoWords(text);

                case Nationality.Tatar:
                case Nationality.Uzbek:
                    return text.Replace("тся", "тца").Replace("ться", "тца").Replace("его", "ево");

                case Nationality.Ukrainian:
                    return InsertAfterComma(text, "та");

                case Nationality.German:
                    return text + " Punkt.";

                case Nationality.Chechen:
                    return text.Replace("очень", "шибко");

                case Nationality.Yakut:
                    return InsertAfterComma(text, "однако");

                default:
                    return text;
            }
        }

        private static string SwapFirstTwoWords(string text)
        {
            string[] words = text.Split(' ');

            if (words.Length < 2)
            {
                return text;
            }

            string first = words[0];

            words[0] = words[1];
            words[1] = first;

            return string.Join(" ", words);
        }

        private static string InsertAfterComma(string text, string particle)
        {
            int index = text.IndexOf(',');

            if (index < 0)
            {
                return text + " " + particle + ".";
            }

            return text.Substring(0, index + 1) + " " + particle + text.Substring(index + 1);
        }

        public static string GetPraise(Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Ukrainian:
                    return "Оце добре, спасибо, хлопцы!";

                case Nationality.Belarusian:
                    return "Дзякуй, усё добра!";

                case Nationality.Tatar:
                    return "Рәхмәт, хорошо сделал!";

                case Nationality.Armenian:
                    return "Спасибо, дорогой, хороший работа!";

                case Nationality.Georgian:
                    return "Вах, молодец, салам!";

                case Nationality.Uzbek:
                    return "Рахмат, брат, хорошо!";

                case Nationality.German:
                    return "Sehr gut. Всё по порядку.";

                case Nationality.Chechen:
                    return "Баркалла, мастер!";

                case Nationality.Yakut:
                    return "Ой, спасибо, однако!";

                default:
                    return "Спасибо, хорошая работа.";
            }
        }

        public static string GetComplaint(Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Ukrainian:
                    return "Та як же так, не годится!";

                case Nationality.Belarusian:
                    return "Гэта не вельмі добра.";

                case Nationality.Tatar:
                    return "Ну что такое, да? Нехорошо.";

                case Nationality.Armenian:
                    return "Вай, дорогой, так нельзя!";

                case Nationality.Georgian:
                    return "Вах, зачем так, брат? Обидно.";

                case Nationality.Uzbek:
                    return "Плохо, брат, совсем плохо.";

                case Nationality.German:
                    return "Das ist nicht gut. Ошибка.";

                case Nationality.Chechen:
                    return "Нехорошо, брат. Зачем так?";

                case Nationality.Yakut:
                    return "Ой, плохо, однако.";

                default:
                    return "Ну что же вы так, нехорошо.";
            }
        }

        public static string DecorateStory(string story, Nationality nationality)
        {
            string suffix = GetSuffix(nationality);

            return string.IsNullOrEmpty(suffix) ? story : story + " " + suffix;
        }

        public static string DecorateFact(string fact, Nationality nationality)
        {
            switch (nationality)
            {
                case Nationality.Armenian:
                    return fact + " Вот так, да?";

                case Nationality.Georgian:
                    return fact + " Хорошо, да?";

                case Nationality.German:
                    return fact + " Genau.";

                case Nationality.Tatar:
                    return fact + " Вот так, да?";

                case Nationality.Yakut:
                    return fact + " Однако.";

                default:
                    return fact;
            }
        }
    }
}
