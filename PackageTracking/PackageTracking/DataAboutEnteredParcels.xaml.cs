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
            Parcels = new ObservableCollection<GroupingParcels<string, RussianPostClassLibrary.ParcelDescription>>();
            this.BindingContext = this;
		}
        
        public ObservableCollection<GroupingParcels<string,RussianPostClassLibrary.ParcelDescription>> Parcels { get; set; }
        public void AddNewParcels(RussianPostClassLibrary.ParcelDescription[] descriptions)
        {
            var groups = descriptions.Where(x=> x.ProcessStatus==true).GroupBy(x => x.StatusParcel).Select(g => new GroupingParcels<string, RussianPostClassLibrary.ParcelDescription>(g.Key, g));
            foreach(var item in groups)
            {
                Parcels.Add(item);
            }
        }
    }
}