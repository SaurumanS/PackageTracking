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
	public partial class AdditionalInformationAboutParcel : ContentPage
	{
        public ObservableCollection<RussianPostClassLibrary.OperationsInfo> operationsInfos { get; set; }
        public AdditionalInformationAboutParcel(RussianPostClassLibrary.ParcelDescription parcelDescription)
		{
			InitializeComponent();
            this.Title = "Отправление: " + parcelDescription.Barcode;
            Sender.Text = parcelDescription.SenderInfo.NameSender + ", страна: " + parcelDescription.SenderInfo.CodeCountrySender;
            Recipient.Text = parcelDescription.RecipientInfo.NameRecipient + ", страна: " + parcelDescription.RecipientInfo.CodeCountryDestination;
            Mass.Text = parcelDescription.Mass;
            TypeParcel.Text = parcelDescription.OperationsInfos.Last().ComplexItemName;
            ListDataAboutParcel.ItemsSource = parcelDescription.OperationsInfos.Select(x=>x).ToArray().Reverse();
            DateTimeOffset dateTimeNow = DateTimeOffset.Now;
            TimeSpan difference;
            if(parcelDescription.StatusParcel=="В пути") difference = dateTimeNow.Subtract(parcelDescription.OperationsInfos.First().DataOperation);
            else difference = parcelDescription.OperationsInfos.Last().DataOperation.Subtract(parcelDescription.OperationsInfos.First().DataOperation);
            TimeHasPassedLabel.Text = $"В пути  {difference.Days} дней, {difference.Hours} часов, {difference.Minutes} минут.";
            this.BindingContext = this;
        }
        protected override void OnDisappearing()
        {
            Navigation.PopAsync();
            base.OnDisappearing();
        }
    }
}