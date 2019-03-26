using System;
using System.Collections.Generic;
using System.Text;

namespace RussianPostClassLibrary
{
    public class RecipientInfo
    {
        string indexDestination = "Не указано";//Индекс места назначения
        string addressDestination = "Не указано";//Адрес и/или название места назначения
        string countryDestination = "Не указано";//Страна-получатель (место назначения)
        string codeCountryDestination = "Не указано";//Код 3А страны-получателя
        string nameRecipient = "Не указано";//Содержит данные об получателе (ФИО, если указано)

        public string IndexDestination
        {
            get { return indexDestination; }
            set { indexDestination = value; }
        }
        public string AddressDestination
        {
            get { return addressDestination; }
            set { addressDestination = value; }
        }
        public string CountryDestination
        {
            get { return countryDestination; }
            set { countryDestination = value; }
        }
        public string CodeCountryDestination
        {
            get { return codeCountryDestination; }
            set { codeCountryDestination = value; }
        }
        public string NameRecipient
        {
            get { return nameRecipient; }
            set { nameRecipient = value; }
        }
    }
}
