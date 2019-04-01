using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Realms;

namespace PackageTracking
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DataAboutEnteredParcels : ContentPage
	{
        Realm realm;
        Transaction transaction;
        public DataAboutEnteredParcels ()
		{
			InitializeComponent ();
            realm = Realm.GetInstance();
            transaction = realm.BeginWrite();
            realm.RemoveAll();
            transaction.Commit();
            //var oldDogs = realm.All<DataBaseModel>();
            //foreach (var item in oldDogs)
            //{
            //    int a = 1 + 5;
            //}
        }

        protected override void OnAppearing()
        {
            DataList.ItemsSource = realm.All<DataBaseModel>();
            base.OnAppearing();
        }
        //public void AddNewParcels(RussianPostClassLibrary.ParcelDescription[] descriptions)
        //{
        //    var groups = descriptions.Where(x=> x.ProcessStatus==true).GroupBy(x => x.StatusParcel).Select(g => new GroupingParcels<string, RussianPostClassLibrary.ParcelDescription>(g.Key, g));
        //    foreach(var item in groups)
        //    {
        //        Parcels.Add(new GroupingParcels<string, RussianPostClassLibrary.ParcelDescription>(item.CurrentlyStatusParcel,item));
        //    }
        //}
    }
}