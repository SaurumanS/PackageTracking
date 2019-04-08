using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Realms;

namespace RussianPostClassLibrary
{
    public class ParcelDescription: RealmObject
    {
        //Свойства для строк
        public SenderInfo SenderInfo { get; set; } = new SenderInfo();
        public IList<OperationsInfo> OperationsInfos { get; }
        public RecipientInfo RecipientInfo { get; set; } = new RecipientInfo();
        public string Barcode { get; set; }
        public string Mass { get; set; } = "Не указано";
        public bool ProcessStatus { get; set; }
        public string ProcessStatusString { get; set; }
        public string ColorOfText { get; set; }//Цвет текста в окне отправки (получены ли данные)
        public string StatusParcel { get; set; }//Показывает статус посылки (доставлена, в пути, вручена адресату)
        
        public string StatusParcelColor { get; set; }
        
    }
}
