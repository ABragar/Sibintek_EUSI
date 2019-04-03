using System;

namespace Base.Utils.Common
{
    /// <summary>
    /// преобразует число в текст
    /// </summary>
    public class NumberToRussianString
    {
        // Род единицы измерения
        public enum WordGender
        {
            Masculine, // мужской
            Feminine, // женский
            Neuter // средний
        };

        // Варианты написания единицы измерения 
        private enum WordMode
        {
            Mode1, // рубль
            Mode2_4, // рубля
            Mode0_5 // рублей
        };

        private static readonly string[] rub = { "рубль", "рубля", "рублей" };
        private static readonly string[] kop = { "копейка", "копейки", "копеек" };

        // Строковые представления чисел
        private const string number0 = "ноль";
        private static readonly string[] number1 =
        {"один", "одна", "одно"};
        private static readonly string[] number2 =
        {"два", "две", "два"};
        private static readonly string[] number3_9 =
        {"три", "четыре", "пять", "шесть", "семь", "восемь", "девять"};
        private static readonly string[] number10_19 =
        {
            "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать",
            "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"
        };
        private static readonly string[] number20_90 =
        {"двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто"};
        private static readonly string[] number100_900 =
        {"сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот"};
        private static readonly string[,] ternaries =
        {
            {"тысяча", "тысячи", "тысяч"},
            {"миллион", "миллиона", "миллионов"},
            {"миллиард", "миллиарда", "миллиардов"},
            {"триллион", "триллиона", "триллионов"},
            {"биллион", "биллиона", "биллионов"}
        };
        private static readonly WordGender[] TernaryGenders =
        {
            WordGender.Feminine, // тысяча - женский
            WordGender.Masculine, // миллион - мужской
            WordGender.Masculine, // миллиард - мужской
            WordGender.Masculine, // триллион - мужской
            WordGender.Masculine // биллион - мужской
        };

        private static string TernaryToString(long ternary, WordGender gender)
        {
            string s = "";

            long digit2 = ternary / 100;
            long digit1 = (ternary % 100) / 10;
            long digit0 = ternary % 10;
            // сотни
            while (digit2 >= 10) digit2 %= 10;
            if (digit2 > 0)
                s = number100_900[digit2 - 1] + " ";
            if (digit1 > 1)
            {
                s += number20_90[digit1 - 2] + " ";
                if (digit0 >= 3)
                    s += number3_9[digit0 - 3] + " ";
                else
                {
                    if (digit0 == 1) s += number1[(int)gender] + " ";
                    if (digit0 == 2) s += number2[(int)gender] + " ";
                }
            }
            else if (digit1 == 1)
                s += number10_19[digit0] + " ";
            else
            {
                if (digit0 >= 3)
                    s += number3_9[digit0 - 3] + " ";
                else if (digit0 > 0)
                {
                    if (digit0 == 1) s += number1[(int)gender] + " ";
                    if (digit0 == 2) s += number2[(int)gender] + " ";
                }
                else { }
            }
            return s.TrimEnd();
        }

        private static string TernaryToString(long value, byte ternaryIndex)
        {
            for (byte i = 0; i < ternaryIndex; i++)
                value /= 1000;
            // учитываются только последние 3 разряда, т.е. 0..999 
            int ternary = (int)(value % 1000);
            if (ternary == 0)
                return "";
            else
            {
                ternaryIndex--;
                return TernaryToString(ternary, TernaryGenders[ternaryIndex]) + " " + ternaries[ternaryIndex, (int)GetWordMode(ternary)] + " ";
            }
        }

        // Определение варианта написания единицы измерения по 3-х значному числу
        private static WordMode GetWordMode(long number)
        {
            int digit1 = (int)(number % 100) / 10;
            int digit0 = (int)(number % 10);
            if (digit1 == 1)
                return WordMode.Mode0_5;
            else if (digit0 == 1)
                return WordMode.Mode1;
            else if (2 <= digit0 && digit0 <= 4)
                return WordMode.Mode2_4;
            else
                return WordMode.Mode0_5;
        }

        // Функция возвращает число прописью
        public static string NumberToString(long value, WordGender gender)
        {
            if (value < 0)
                return "";

            else if (value == 0)
                return number0;
            else
                return TernaryToString(value, 5) +
                TernaryToString(value, 4) +
                TernaryToString(value, 3) +
                TernaryToString(value, 2) +
                TernaryToString(value, 1) +
                TernaryToString(value, gender);
        }

        //Преобразование для денежных единиц
        public static string DecimalToString(decimal value)
        {
            long rubles = (long)Decimal.Truncate(value);
            long fractPart = (long)(100.0m * (value - Math.Floor(value)));

            return $"{NumberToString(rubles, WordGender.Masculine)} {rub[(int)GetWordMode(rubles)]} {NumberToString(fractPart, WordGender.Feminine)} {kop[(int)GetWordMode(fractPart)]}";
        }
    }
}
