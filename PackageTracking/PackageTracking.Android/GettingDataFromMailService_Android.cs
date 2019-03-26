using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;

using RussianPostClassLibrary;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(PackageTracking.Droid.GettingDataFromMailService_Android))]
namespace PackageTracking.Droid
{
    public class GettingDataFromMailService_Android: IReturnData
    {
        public RussianPostClassLibrary.ParcelDescription ParcelDescription(string barcode)
        {
            var operationHistory = RussianPostGettingData(barcode);
            RussianPostClassLibrary.ParcelDescription parcelDescription = ReturnProcessedInfoAboutParcel(operationHistory);
            parcelDescription.Barcode = barcode;
            return parcelDescription;
        }
        ru.russianpost.tracking.OperationHistoryRecord[] RussianPostGettingData(string barcode)//Получение данных от Почты России
        {
            var request = new PackageTracking.Droid.ru.russianpost.tracking.OperationHistoryRequest();//Экземпляр класса, формирующего запрос
            PackageTracking.Droid.ru.russianpost.tracking.AuthorizationHeader client = new PackageTracking.Droid.ru.russianpost.tracking.AuthorizationHeader();//Экземпляр класса, отвечающий за передачу входных данных
            client.login = "qDbOSkcVdtXgMR";
            client.password = "ckprFmPYDadL";
            request.Barcode = barcode;
            request.MessageType = 0;
            try
            {
                PackageTracking.Droid.ru.russianpost.tracking.OperationHistory12 transmission = new PackageTracking.Droid.ru.russianpost.tracking.OperationHistory12();
                PackageTracking.Droid.ru.russianpost.tracking.OperationHistoryRecord[] response = transmission.getOperationHistory(request, client);//Получение ответа
                return response;
            }
            catch (Exception exeption) { Console.WriteLine(exeption.Message); }
            return null;
        }

        RussianPostClassLibrary.ParcelDescription ReturnProcessedInfoAboutParcel(ru.russianpost.tracking.OperationHistoryRecord[] operationHistoryRecords)
        {
            RussianPostClassLibrary.ParcelDescription parcelDescription = new ParcelDescription();
            parcelDescription = ReturnProcessedInfoAboutOperations(parcelDescription, operationHistoryRecords);
            parcelDescription = ReturnProcessedInfoAboutRecipient(parcelDescription, operationHistoryRecords);
            parcelDescription = ReturnProcessedInfoAboutSender(parcelDescription, operationHistoryRecords);
            return parcelDescription;
        }
        RussianPostClassLibrary.ParcelDescription ReturnProcessedInfoAboutSender(RussianPostClassLibrary.ParcelDescription parcelDescription, ru.russianpost.tracking.OperationHistoryRecord[] operationHistoryRecords)//Заполняем информацию об отправителе
        {
            for(int index = 0; index < operationHistoryRecords.Length; index++)
            {
                if(!string.IsNullOrEmpty(operationHistoryRecords[index].UserParameters.Sndr))
                {
                    parcelDescription.SenderInfo.NameSender = operationHistoryRecords[index].UserParameters.Sndr;
                }
                if (operationHistoryRecords[index].UserParameters.SendCtg != null && !string.IsNullOrEmpty(operationHistoryRecords[index].UserParameters.SendCtg.Name))
                {
                    parcelDescription.SenderInfo.CategorySender = operationHistoryRecords[index].UserParameters.SendCtg.Name;
                }
                if (operationHistoryRecords[index].AddressParameters.CountryFrom != null)
                {
                    if(!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.CountryFrom.Code3A)) parcelDescription.SenderInfo.CodeCountrySender = operationHistoryRecords[index].AddressParameters.CountryFrom.Code3A;
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.CountryFrom.NameRU)) parcelDescription.SenderInfo.CountrySender = operationHistoryRecords[index].AddressParameters.CountryFrom.NameRU;
                }
                if (operationHistoryRecords[index].ItemParameters != null && !string.IsNullOrEmpty(operationHistoryRecords[index].ItemParameters.Mass)) parcelDescription.Mass = operationHistoryRecords[index].ItemParameters.Mass;
            }
            return parcelDescription;
        }
        RussianPostClassLibrary.ParcelDescription ReturnProcessedInfoAboutRecipient(RussianPostClassLibrary.ParcelDescription parcelDescription, ru.russianpost.tracking.OperationHistoryRecord[] operationHistoryRecords)//Заполняем информацию об отправителе
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
                if (operationHistoryRecords[index].AddressParameters.MailDirect!=null && !string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.MailDirect.Code3A))
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

        RussianPostClassLibrary.ParcelDescription ReturnProcessedInfoAboutOperations(RussianPostClassLibrary.ParcelDescription parcelDescription, ru.russianpost.tracking.OperationHistoryRecord[] operationHistoryRecords)//Заполняем информацию об отправителе (+ информацию о массе посылки)
        {
            parcelDescription.OperationsInfo = new OperationsInfo[operationHistoryRecords.Length];
            for (int index = 0; index < operationHistoryRecords.Length; index++)
            {
                parcelDescription.OperationsInfo[index] = new OperationsInfo();
                if (operationHistoryRecords[index].AddressParameters.CountryOper != null)
                {
                    if(!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.CountryOper.Code3A)) parcelDescription.OperationsInfo[index].CodeCountryOperation = operationHistoryRecords[index].AddressParameters.CountryOper.Code3A;
                    if(!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.CountryOper.NameRU)) parcelDescription.OperationsInfo[index].CountryOperation = operationHistoryRecords[index].AddressParameters.CountryOper.NameRU;
                }
                if(operationHistoryRecords[index].AddressParameters.OperationAddress != null)
                {
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.OperationAddress.Index) && IsDigitsOnly(operationHistoryRecords[index].AddressParameters.OperationAddress.Index)) parcelDescription.OperationsInfo[index].IndexOperation = operationHistoryRecords[index].AddressParameters.OperationAddress.Index;
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].AddressParameters.OperationAddress.Description)) parcelDescription.OperationsInfo[index].AddressOperation = operationHistoryRecords[index].AddressParameters.OperationAddress.Description;
                }
                if (operationHistoryRecords[index].ItemParameters.MailType != null && !string.IsNullOrEmpty(operationHistoryRecords[index].ItemParameters.MailType.Name)) parcelDescription.OperationsInfo[index].ComplexItemName = operationHistoryRecords[index].ItemParameters.MailType.Name;
                if (operationHistoryRecords[index].OperationParameters.OperDate != null && !string.IsNullOrEmpty(operationHistoryRecords[index].OperationParameters.OperDate.Date.ToString())) parcelDescription.OperationsInfo[index].DataOperation = operationHistoryRecords[index].OperationParameters.OperDate;
                if (operationHistoryRecords[index].OperationParameters.OperType != null)
                {
                    if(!string.IsNullOrEmpty(operationHistoryRecords[index].OperationParameters.OperType.Name))parcelDescription.OperationsInfo[index].NameOperation = operationHistoryRecords[index].OperationParameters.OperType.Name;
                    parcelDescription.OperationsInfo[index].NameOperationCode = operationHistoryRecords[index].OperationParameters.OperType.Id;
                }
                if (operationHistoryRecords[index].OperationParameters.OperAttr != null)
                {
                    if (!string.IsNullOrEmpty(operationHistoryRecords[index].OperationParameters.OperAttr.Name)) parcelDescription.OperationsInfo[index].NameOperationAttribute = operationHistoryRecords[index].OperationParameters.OperAttr.Name;
                    parcelDescription.OperationsInfo[index].NameOperationAttributeCode = operationHistoryRecords[index].OperationParameters.OperAttr.Id;
                }
            }
            return parcelDescription;
        }
        bool IsDigitsOnly(string str)//Проверяет содержит ли строка только цифры
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