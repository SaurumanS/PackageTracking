using System;
using System.Drawing;
using System.Threading.Tasks;

namespace RussianPostClassLibrary
{
    public class ParcelDescription
    {
        string barcode;//Трек-код
        string mass = "Не указано";//Масса посылки
        bool processStatus;//Указывает были ли получены данные от веб-сервиса по данному трек-коду
        string processStatusString;
        RecipientInfo recipientInfo = new RecipientInfo();
        SenderInfo senderInfo= new SenderInfo();
        OperationsInfo[] operationsInfos;

        //Свойства для строк
        public SenderInfo SenderInfo
        {
            get { return senderInfo; }
            set { senderInfo = value;}
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
        public bool ProcessStatus
        {
            get { return processStatus; }
            set
            {
                processStatus = value;
                if (value)
                {
                    ProcessStatusString = "Данные получены";
                    ColorOfText = Color.Green;
                }
                else
                {
                    ProcessStatusString = "Ошибка: данные отсутствуют (проверьте трек-код)";
                    ColorOfText = Color.Red;
                }
            }
        }
        public string ProcessStatusString
        {
            get { return processStatusString; }
            set { processStatusString = value; }
        }
        public Color ColorOfText//Цвет текста в окне отправки (получены ли данные)
        {
            get; set;
        }
        public string StatusParcel//Показывает статус посылки (доставлена, в пути, вручена адресату)
        {
            get;
            set;
        }
    }
}
