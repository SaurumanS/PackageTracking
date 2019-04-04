using System;
using System.Collections.Generic;
using System.Text;

using Realms;

namespace RussianPostClassLibrary
{
    public class RecipientInfo: RealmObject
    {
        public string IndexDestination { get; set; } = "Не указано";
        public string AddressDestination { get; set; } = "Не указано";
        public string CountryDestination { get; set; } = "Не указано";
        public string CodeCountryDestination { get; set; } = "Не указано";
        public string NameRecipient { get; set; } = "Не указано";
    }
}
