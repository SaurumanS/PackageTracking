using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PackageTracking
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DataAboutEnteredParcels : ContentPage
	{
		public DataAboutEnteredParcels ()
		{
			InitializeComponent ();

            //var d = new List<RussianPostClassLibrary.ParcelDescription>
            //{
            //    new RussianPostClassLibrary.ParcelDescription{Barcode="54645",ProcessStatus=true },
            //    new RussianPostClassLibrary.ParcelDescription{Barcode="54665",ProcessStatus=true},
            //    new RussianPostClassLibrary.ParcelDescription{Barcode="5465",ProcessStatus=false },
            //    new RussianPostClassLibrary.ParcelDescription{Barcode="545",ProcessStatus=true }
            //};
            //var groups = d.Where(x => x.ProcessStatus == true).GroupBy(x => x.StatusParcel).Select(g => new GroupingParcels<string, RussianPostClassLibrary.ParcelDescription>(g.Key, g));
            //foreach (var item in groups)
            //{
            //    Parcels.Add(new GroupingParcels<string, RussianPostClassLibrary.ParcelDescription>("DD", item));
            //}
            Parcels = new ObservableCollection<GroupingParcels<string, RussianPostClassLibrary.ParcelDescription>>();
            this.BindingContext = this;
		}
       

        public ObservableCollection<GroupingParcels<string,RussianPostClassLibrary.ParcelDescription>> Parcels { get; set; }
        public void AddNewParcels(RussianPostClassLibrary.ParcelDescription[] descriptions)
        {
            var groups = descriptions.Where(x=> x.ProcessStatus==true).GroupBy(x => x.StatusParcel).Select(g => new GroupingParcels<string, RussianPostClassLibrary.ParcelDescription>(g.Key, g));
            foreach(var item in groups)
            {
                Parcels.Add(new GroupingParcels<string, RussianPostClassLibrary.ParcelDescription>(item.CurrentlyStatusParcel,item));
            }
        }
    }
}