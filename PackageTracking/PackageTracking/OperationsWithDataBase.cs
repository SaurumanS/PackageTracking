using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Realms;
using Xamarin.Forms;

namespace PackageTracking
{
    class OperationsWithDataBase
    {
        public static void AddElementInDataBase(params RussianPostClassLibrary.ParcelDescription[] parcelsDescriptions)//Добавление элемента в базу
        {
            Realm dataBase = Realm.GetInstance();
            dataBase.Write(() =>
            {
                foreach (var item in parcelsDescriptions)
                {
                    if (item.ProcessStatus)
                    {
                        var operations = dataBase.Add(item);
                        operations.SenderInfo = dataBase.Add(item.SenderInfo);
                        operations.RecipientInfo = dataBase.Add(item.RecipientInfo);
                        dataBase.Add(new DataBaseModel { Id = item.Barcode, Status = item.StatusParcel, ParcelDescription = operations, StatusParcelColor = item.StatusParcelColor });
                    }
                }
            });
        }
        public static bool CheckOnDuplicationTrackCodeInDataBase(params string[] tracks)//Проверка на попытку повторно занести трек-код в базу данных
        {
            Realm dataBase = Realm.GetInstance();
            foreach (var track in tracks)
            {
                if (dataBase.Find<DataBaseModel>(track) != null)
                {
                    return false;
                }
            }
            return true;
        }
        public static void DeleteParcelFromDataBase(DataBaseModel dataBaseModel)//Удаление посылки из базы данных
        {
            Realm dataBase = Realm.GetInstance();
            Transaction transaction = dataBase.BeginWrite();
            dataBase.Remove(dataBaseModel);
            transaction.Commit();
        }
        public static void UpdateInfoAboutParcels()//Обновление данных
        {
            Realm dataBase = Realm.GetInstance();
            var dataBaseElements = dataBase.All<DataBaseModel>();
            foreach (var item in dataBaseElements)
            {
                ReturnDataAboutOneParcel(item);
            }
        }
        private static void ReturnDataAboutOneParcel(DataBaseModel item)//Асинхронный метод для заполнения массива из GettingData()
        {
            Realm dataBase = Realm.GetInstance();
            using (var transaction = dataBase.BeginWrite())
            {
                item.Status = "Обновление данных";
                item.StatusParcelColor = "#304cdb";
                transaction.Commit();
            }
            using (var transaction = dataBase.BeginWrite())
            {
                RussianPostClassLibrary.ParcelDescription parcelDescription = DependencyService.Get<IReturnData>().ParcelDescription(item.ParcelDescription.Barcode);
                if (parcelDescription.ProcessStatus)
                {
                    item.ParcelDescription.RecipientInfo = parcelDescription.RecipientInfo;
                    for (int index = item.ParcelDescription.OperationsInfos.Count; index < parcelDescription.OperationsInfos.Count; index++)
                    {
                        item.ParcelDescription.OperationsInfos.Add(parcelDescription.OperationsInfos[index]);
                    }
                    item.ParcelDescription.SenderInfo = parcelDescription.SenderInfo;
                    item.ParcelDescription = parcelDescription;
                    item.Status = item.ParcelDescription.StatusParcel;
                    item.StatusParcelColor = item.ParcelDescription.StatusParcelColor;
                }
                transaction.Commit();
            }

        }
    }
}
