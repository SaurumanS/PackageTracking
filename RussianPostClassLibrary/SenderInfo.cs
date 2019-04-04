using System;
using System.Collections.Generic;
using System.Text;

using Realms;

namespace RussianPostClassLibrary
{
    public class SenderInfo:RealmObject
    {
        public string CountrySender { get; set; } = "Не указано";
        public string CodeCountrySender { get; set; } = "Не указано";
        public string NameSender { get; set; } = "Не указано";
        public string CategorySender { get; set; } = "Не указано";
    }
}
