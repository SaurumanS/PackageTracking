using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realms;

namespace PackageTracking
{
    public class DataBaseModel: RealmObject //Модель базы данных
    {

        [PrimaryKey]
        public string Id { get; set; }
        public string Status { get; set; }
        
        public string StatusParcelColor { get; set; }
        
        public RussianPostClassLibrary.ParcelDescription ParcelDescription { get; set; }
    }
}
