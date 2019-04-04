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
using Plugin.Connectivity;

[assembly: Xamarin.Forms.Dependency(typeof(PackageTracking.Droid.GettingDataFromMailService_Android))]
namespace PackageTracking.Droid
{
    public class GettingDataFromMailService_Android: IReturnData
    {

        public RussianPostClassLibrary.ParcelDescription ParcelDescription(string barcode)//Получаем массив данных и отправляем их на обработку
        {
            RussianPostClassLibrary.ParcelDescription parcelDescription;
            var operationHistory = RussianPostGettingData(barcode);
            parcelDescription = DataProcessing.DataProcessingBegin(operationHistory, barcode);
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
                if(response != null) return response;
            }
            catch (Exception) { }
            return new ru.russianpost.tracking.OperationHistoryRecord[0];
        }

    }
}