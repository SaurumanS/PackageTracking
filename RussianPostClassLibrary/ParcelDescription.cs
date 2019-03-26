using System;
using System.Threading.Tasks;

namespace RussianPostClassLibrary
{
    public class ParcelDescription
    {
        string barcode;//Трек-код
        string mass = "Не указано";//Масса посылки
        RecipientInfo recipientInfo = new RecipientInfo();
        SenderInfo senderInfo= new SenderInfo();
        OperationsInfo[] operationsInfos;

        //Свойства для строк
        public SenderInfo SenderInfo
        {
            get { return senderInfo; }
            set { senderInfo = value; }
        }
        public OperationsInfo[] OperationsInfo
        {
            get { return operationsInfos; }
            set { operationsInfos = value; }
        }
        public RecipientInfo RecipientInfo
        {
            get { return recipientInfo; }
            set { recipientInfo = value; }
        }
        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }
        public string Mass
        {
            get { return mass; }
            set { mass = value; }
        }

    }
}
