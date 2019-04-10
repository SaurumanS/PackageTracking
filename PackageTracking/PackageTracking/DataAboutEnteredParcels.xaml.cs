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
            try
            {
                realm = Realm.GetInstance();
            }
            catch (Exception)
            {
                var config = RealmConfiguration.DefaultConfiguration;
                config.SchemaVersion=3;  // increment this when your model changes
                realm = Realm.GetInstance();
            }
            var oldDogs = realm.All<DataBaseModel>();
            
        }

        protected override void OnAppearing()
        {
            DataList.ItemsSource = realm.All<DataBaseModel>();
            base.OnAppearing();
        }

        private async void DataList_ItemSelected(object sender, ItemTappedEventArgs e)//Обработчик нажатия на трек-код (получение доп.информации)
        {
            ListView listView = (ListView)sender;
            DataBaseModel dataBaseModel = (DataBaseModel)listView.SelectedItem;
            RussianPostClassLibrary.ParcelDescription parcelDescription = dataBaseModel.ParcelDescription;
            await Navigation.PushAsync(new AdditionalInformationAboutParcel(parcelDescription));
            ((ListView)sender).SelectedItem = null;
        }

        private void UpdateDateAboutParcel_Clicked(object sender, EventArgs e)
        {
            UpdateDateAboutParcel.IsEnabled = false;
            OperationsWithDataBase.UpdateInfoAboutParcels();
            UpdateDateAboutParcel.IsEnabled = true;
            DisplayAlert("Оповещение", "Все данные были обновлены", "OK");
        }

        private async void DeleteOneParcelFromDataBase_Clicked(object sender, EventArgs e)//Нажатия на кнопку для удаления записи из базы данных
        {
            bool answer = await DisplayAlert("Подтверждение", "Вы серьёзно хотите удалить это?", "Just Do it!!!", "OMG, No");
            if (answer)
            {
                var button = sender as Button;
                var parcel = (DataBaseModel)button.BindingContext;
                OperationsWithDataBase.DeleteParcelFromDataBase(parcel);
            }
        }

    }
}