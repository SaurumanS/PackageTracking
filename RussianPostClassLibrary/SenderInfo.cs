using System;
using System.Collections.Generic;
using System.Text;

namespace RussianPostClassLibrary
{
    public class SenderInfo
    {
        string countrySender = "Не указано";//Страна-отправитель посылки
        string codeCountrySender = "Не указано";//Код 3А страны-отправителя
        string nameSender = "Не указано";//Имя отправителя
        string categorySender = "Не указано";//Название категории отправителя

        public string CountrySender
        {
            get { return countrySender; }
            set { countrySender = value; }
        }
        public string CodeCountrySender
        {
            get { return codeCountrySender; }
            set { codeCountrySender = value; }
        }
        public string NameSender
        {
            get { return nameSender; }
            set { nameSender = value; }
        }
        public string CategorySender
        {
            get { return categorySender; }
            set { categorySender = value; }
        }
    }
}
