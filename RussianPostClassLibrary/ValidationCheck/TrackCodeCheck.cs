using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RussianPostClassLibrary.ValidationCheck
{
    public class TrackCodeCheck//Класс для проверки строки ввода трек-кода на корректность набора
    {
        public static bool CheckTrackCode(string trackCode, bool allString, bool insert)//Проверка всей строки или отдельного символа на корректность
        {
            if (string.IsNullOrEmpty(trackCode) && allString) throw new ArgumentException("Хмм...кажется строка пустая","");
            CheckOnCorrectLength(trackCode);
            if (allString)
            {
                for (int index = 0; index < trackCode.Length; index++)
                {
                    if (!CheckOnCorrectSymbol(trackCode[index].ToString())) throw new ArgumentException($"Некорректный символ \"{trackCode[index].ToString()}\"", trackCode.Remove(index));
                }
                if(!insert) CheckOnCorrectString(trackCode);
            }
            else
            {
                if (!CheckOnCorrectSymbol(trackCode[trackCode.Length-1].ToString())) throw new ArgumentException($"Некорректный символ \"{trackCode[trackCode.Length - 1].ToString()}\"", trackCode.Remove(trackCode.Length - 1));
            }
            return true;

        }
        private static void CheckOnCorrectLength(string trackCode)//Проверка строки на длину
        {
            if (trackCode.Length > 130) throw new ArgumentException("Максимальная длина 130 символов",trackCode.Remove(130));
        }
        private static bool CheckOnCorrectSymbol(string symbol)//Проверяет отдельно взятый символ
        {
            if (IsDigitsOnly(symbol) || IsLettersOnly(symbol) || symbol == "+" || symbol == "|") return true;
            return false;
        }
        private static bool CheckOnCorrectString(string trackCode)//Проверяет всю строку на корректность
        {

            string[] tracks = trackCode.ToUpper().Split('|', '+');
            for (int index = 0; index < tracks.Length; index++)
            {
                if (IsLettersOnly(tracks[index].First().ToString()))
                {
                    if (tracks[index].Length != 13) throw new ArgumentException("Длина трек-кода (" + tracks[index] + ") неверная");
                    string temp = tracks[index];
                    temp = temp.TrimStart('C', 'E', 'L', 'R', 'S', 'V', 'Z');
                    if (temp.Length == tracks[index].Length) throw new ArgumentException("Данный трек-код (" + tracks[index] + ") не отслеживается смотри пример");
                    bool check = true;
                    for (int counter = 1; counter < tracks[index].Length; counter++)
                    {
                        if (counter == 1 || counter == 11 || counter == 12) check = IsLettersOnly(tracks[index][counter].ToString());
                        else check = IsDigitsOnly(tracks[index][counter].ToString());
                        if (!check) throw new ArgumentException("Ошибка ввода трек-кода (" + tracks[index] + "), смотри пример трек-кода");
                    }
                }
                else if (IsDigitsOnly(tracks[index].First().ToString()))
                {
                    if (tracks[index].Length != 14) throw new ArgumentException("Длина трек-кода (" + tracks[index] + ") неверная");
                    if(!IsDigitsOnly(tracks[index])) throw new ArgumentException("Ошибка ввода трек-кода (" + tracks[index] + "), смотри пример трек-кода");
                }
            }
            return true;
        }
        static bool IsDigitsOnly(string str)//Проверяет содержит ли строка только цифры
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        static bool IsLettersOnly(string str)//Проверяет содержит ли строка только буквы
        {
            foreach (char c in str)
            {
                if (c < 'A' || c > 'Z')
                    return false;
            }

            return true;
        }
    }
}
