using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using RussianPostClassLibrary;

namespace PackageTracking.Droid
{
    class DataProcessing//Класс, который получает массив данных от веб-сервиса и преобразует его к типы ParcelDescription
    {
        public static RussianPostClassLibrary.ParcelDescription DataProcessingBegin(ru.russianpost.tracking.OperationHistoryRecord[] operationHistory, string barcode)//Начинаем обработку данных
        {
            RussianPostClassLibrary.ParcelDescription parcelDescription;
            if (operationHistory.Length == 0)//Если данные отсутствую
            {
                parcelDescription = new ParcelDescription();
                parcelDescription.Barcode = barcode;
                parcelDescription.ProcessStatus = false;
                parcelDescription.ProcessStatusString = "Ошибка: данные отсутствуют (проверьте трек-код)";
                parcelDescription.ColorOfText = "Red";
                return parcelDescription;
            }
            parcelDescription = ReturnProcessedInfoAboutParcel(operationHistory);
            parcelDescription.Barcode = barcode;
            parcelDescription.ProcessStatus = true;
            parcelDescription.ProcessStatusString = "Данные получены";
            parcelDescription.ColorOfText = "Green";
            if (parcelDescription.OperationsInfos.Last().NameOperationCode == '8' && parcelDescription.OperationsInfos.Last().NameOperationAttributeCode == '2') parcelDescription.StatusParcel = "Доставлено";//Если посылка доставлена в почтовое отделение
            else if (parcelDescription.OperationsInfos.Last().NameOperationCode == '2') parcelDescription.StatusParcel = "Вручено";//Если посылка вручена адресату
            else parcelDescription.StatusParcel = "В пути";
            return parcelDescription;
        }

        static RussianPostClassLibrary.ParcelDescription ReturnProcessedInfoAboutParcel(ru.russianpost.tracking.OperationHistoryRecord[] operationHistoryRecords)
        {
            RussianPostClassLibrary.ParcelDescription parcelDescription = new ParcelDescription();
            parcelDescription = ReturnProcessedInfoAboutOperations(parcelDescription, operationHistoryRecords);
            parcelDescription = ReturnProcessedInfoAboutRecipient(parcelDescription, operationHistoryRecords);
            parcelDescription = ReturnProcessedInfoAboutSender(parcelDescription, operationHistoryRecords);
            return parcelDescription;
        }
        static RussianPostClassLibrary.ParcelDescription  ReturnProcessedInfoAboutSender(RussianPostClassLibrary.ParcelDescription parcelDescription, ru.russianpost.tracking.OperationHistoryRecord[] operationHistoryRecords)//Заполняем информацию об отправителе
        {
            for (int index = 0; index < operationHistoryRecords.Length; index++)
            {
                if (!string.IsNullOrEmpty(operationHistoryRecords[index].UserParameters.Sndr))
                {
                    parcelDescription.SenderInfo.NameSender = operationHistoryRecords[index].UserParameters.Sndr;
                }
                if (operationHistoryRecords[index].UserParameters.SendCtg != null && !string.IsNullOrEmpty(operationHistoryRecords[index].UserParameters.SendCtg.Name))
                {
                    parcelDescription.SenderInfo.CategorySender = operationHistoryRecords[index].UserParameters.SendCtg.Name;
                }
                if (operationHistoryRecords[index].AddressParameters.CountryFrom != null)
                {
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.CountryFrom.Code3A)) parcelDescription.SenderInfo.CodeCountrySender = operationHistoryRecords[index].AddressParameters.CountryFrom.Code3A;
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.CountryFrom.NameRU)) parcelDescription.SenderInfo.CountrySender = operationHistoryRecords[index].AddressParameters.CountryFrom.NameRU;
                }
                if (operationHistoryRecords[index].ItemParameters != null && !string.IsNullOrEmpty(operationHistoryRecords[index].ItemParameters.Mass)) parcelDescription.Mass = operationHistoryRecords[index].ItemParameters.Mass;
            }
            return parcelDescription;
        }
        static RussianPostClassLibrary.ParcelDescription ReturnProcessedInfoAboutRecipient(RussianPostClassLibrary.ParcelDescription parcelDescription, ru.russianpost.tracking.OperationHistoryRecord[] operationHistoryRecords)//Заполняем информацию об отправителе
        {
            for (int index = 0; index < operationHistoryRecords.Length; index++)
            {
                if (!string.IsNullOrEmpty(operationHistoryRecords[index].UserParameters.Rcpn))
                {
                    parcelDescription.RecipientInfo.NameRecipient = operationHistoryRecords[index].UserParameters.Rcpn;
                }
                if (operationHistoryRecords[index].AddressParameters.DestinationAddress != null && !string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.DestinationAddress.Description))
                {
                    parcelDescription.RecipientInfo.AddressDestination = operationHistoryRecords[index].AddressParameters.DestinationAddress.Description;
                }
                if (operationHistoryRecords[index].AddressParameters.DestinationAddress != null && !string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.DestinationAddress.Index) && IsDigitsOnly(operationHistoryRecords[index].AddressParameters.DestinationAddress.Index))
                {
                    parcelDescription.RecipientInfo.IndexDestination = operationHistoryRecords[index].AddressParameters.DestinationAddress.Index;
                }
                if (operationHistoryRecords[index].AddressParameters.MailDirect != null && !string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.MailDirect.Code3A))
                {
                    parcelDescription.RecipientInfo.CodeCountryDestination = operationHistoryRecords[index].AddressParameters.MailDirect.Code3A;
                }
                if (operationHistoryRecords[index].AddressParameters.MailDirect != null && !string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.MailDirect.NameRU))
                {
                    parcelDescription.RecipientInfo.CountryDestination = operationHistoryRecords[index].AddressParameters.MailDirect.NameRU;
                }
            }
            return parcelDescription;
        }

        static RussianPostClassLibrary.ParcelDescription ReturnProcessedInfoAboutOperations(RussianPostClassLibrary.ParcelDescription parcelDescription, ru.russianpost.tracking.OperationHistoryRecord[] operationHistoryRecords)//Заполняем информацию об отправителе (+ информацию о массе посылки)
        {
            var operations = new RussianPostClassLibrary.OperationsInfo[operationHistoryRecords.Length];
            for (int index = 0; index < operationHistoryRecords.Length; index++)
            {
                operations[index] = new OperationsInfo();
                if (operationHistoryRecords[index].AddressParameters.CountryOper != null)
                {
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.CountryOper.Code3A)) operations[index].CodeCountryOperation = operationHistoryRecords[index].AddressParameters.CountryOper.Code3A;
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.CountryOper.NameRU)) operations[index].CountryOperation = operationHistoryRecords[index].AddressParameters.CountryOper.NameRU;
                }
                if (operationHistoryRecords[index].AddressParameters.OperationAddress != null)
                {
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.OperationAddress.Index) && IsDigitsOnly(operationHistoryRecords[index].AddressParameters.OperationAddress.Index)) operations[index].IndexOperation = operationHistoryRecords[index].AddressParameters.OperationAddress.Index;
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.OperationAddress.Description)) operations[index].AddressOperation = operationHistoryRecords[index].AddressParameters.OperationAddress.Description;
                }
                if (operationHistoryRecords[index].ItemParameters.MailType != null && !string.IsNullOrEmpty(operationHistoryRecords[index].ItemParameters.MailType.Name)) operations[index].ComplexItemName = operationHistoryRecords[index].ItemParameters.MailType.Name;
                if (operationHistoryRecords[index].OperationParameters.OperDate != null && !string.IsNullOrEmpty(operationHistoryRecords[index].OperationParameters.OperDate.Date.ToString())) operations[index].DataOperation = operationHistoryRecords[index].OperationParameters.OperDate;
                if (operationHistoryRecords[index].OperationParameters.OperType != null)
                {
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].OperationParameters.OperType.Name)) operations[index].NameOperation = operationHistoryRecords[index].OperationParameters.OperType.Name;
                    operations[index].NameOperationCode = operationHistoryRecords[index].OperationParameters.OperType.Id;
                }
                if (operationHistoryRecords[index].OperationParameters.OperAttr != null)
                {
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].OperationParameters.OperAttr.Name)) operations[index].NameOperationAttribute = operationHistoryRecords[index].OperationParameters.OperAttr.Name;
                    operations[index].NameOperationAttributeCode = operationHistoryRecords[index].OperationParameters.OperAttr.Id;
                }
                operations[index].DataOperationString = operations[index].DataOperation.ToLocalTime().ToString();
                if (!string.IsNullOrEmpty(operations[index].IndexOperation)) operations[index].IndexOperation += ", " + operations[index].AddressOperation + ": " + operations[index].CountryOperation;
                else operations[index].IndexOperation = operations[index].AddressOperation + ": " + operations[index].CountryOperation;
            }
            foreach (var item in operations)
            {
                parcelDescription.OperationsInfos.Add(item);
            }
            return parcelDescription;
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
    }
}