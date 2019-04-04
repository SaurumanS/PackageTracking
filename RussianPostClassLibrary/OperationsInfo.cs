using System;
using System.Collections.Generic;
using System.Text;

using Realms;

namespace RussianPostClassLibrary
{
    public class OperationsInfo:RealmObject
    {
        public string IndexOperation { get; set; } = "";
        public string AddressOperation { get; set; } = "";
        public string CountryOperation { get; set; } = "";
        public string CodeCountryOperation { get; set; } = "";
        public string NameOperation { get; set; } = "";
        public int NameOperationCode { get; set; }
        public string NameOperationAttribute { get; set; } = "";
        public int NameOperationAttributeCode { get; set; }
        public DateTimeOffset DataOperation { get; set; }
        public string DataOperationString { get; set; } = "";
        public string ComplexItemName { get; set; } = "";
    }
}
