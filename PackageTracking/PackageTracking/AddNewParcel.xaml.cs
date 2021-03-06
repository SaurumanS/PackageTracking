﻿using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PackageTracking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewParcel : ContentPage
    {
        bool checkOnSameSymbol = false;//Изменяется, когда был произведён ввод символа или вставка строки (чтобы обработчик не выполнялся дважды)
        public AddNewParcel()
        {
            InitializeComponent();
            ParcelDescriptionsBinding = new ObservableCollection<RussianPostClassLibrary.ParcelDescription>();
            TrackInput.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeCharacter);
            this.BindingContext = this;
        }
        public ObservableCollection<RussianPostClassLibrary.ParcelDescription> ParcelDescriptionsBinding { get; set; }//Коллекция для привязки к ListView


        private void TrackInput_TextChanged(object sender, TextChangedEventArgs e)//Событие на ввод текста в поле (проверка на корректность ввода)
        {
            if (checkOnSameSymbol) checkOnSameSymbol = false;
            else TrackInputLabel.IsVisible = false;
            string oldTextValue;
            string newTextValue;
            if (string.IsNullOrEmpty(e.OldTextValue)) oldTextValue = "";
            else oldTextValue = e.OldTextValue.ToUpper();
            if (string.IsNullOrEmpty(e.NewTextValue)) newTextValue = "";
            else newTextValue = e.NewTextValue.ToUpper();
            
            if (newTextValue.Length - oldTextValue.Length > 1)//Если была произведена вставка
            {
                try
                {
                    RussianPostClassLibrary.ValidationCheck.TrackCodeCheck.CheckTrackCode(newTextValue, true, true);
                    ((Entry)sender).Text = newTextValue;
                }
                catch (ArgumentException exeption)
                {
                    TrackInputLabel.IsVisible = true;
                    ((Entry)sender).Text = exeption.ParamName;
                    int index = exeption.Message.IndexOf("P");
                    TrackInputLabel.Text = exeption.Message.Remove(index-1);
                    checkOnSameSymbol = true;
                }
            }
            else if (newTextValue.Last() == oldTextValue.Last())
            {
                try
                {
                    RussianPostClassLibrary.ValidationCheck.TrackCodeCheck.CheckTrackCode(newTextValue, true, true);
                    ((Entry)sender).Text = newTextValue;
                }
                catch (ArgumentException exeption)
                {
                    TrackInputLabel.IsVisible = true;
                    ((Entry)sender).Text = oldTextValue;
                    int index = exeption.Message.IndexOf("P");
                    TrackInputLabel.Text = exeption.Message.Remove(index - 1);
                    checkOnSameSymbol = true;
                }
            }
            else if (newTextValue.Length - oldTextValue.Length == 1)//Если введён один символ
            {
                try
                {
                    RussianPostClassLibrary.ValidationCheck.TrackCodeCheck.CheckTrackCode(newTextValue, false, false);
                    ((Entry)sender).Text = newTextValue;
                }
                catch (ArgumentException exeption)
                {
                    TrackInputLabel.IsVisible = true;
                    ((Entry)sender).Text = exeption.ParamName;
                    int index = exeption.Message.IndexOf("P");
                    if(index!=-1)TrackInputLabel.Text = exeption.Message.Remove(index - 1);
                    else TrackInputLabel.Text = exeption.Message;
                    checkOnSameSymbol = true;
                }
            }
        }
        private void TrackInput_Completed(object sender, EventArgs e)
        {
            SendTrackButton_Clicked(sender, e);
        }

        private void SendTrackButton_Clicked(object sender, EventArgs e)//Отправка трек-кода(ов) 
        {
            try
            {
                RussianPostClassLibrary.ValidationCheck.TrackCodeCheck.CheckTrackCode(TrackInput.Text, true, false);
                GettingData();
            }
            catch (ArgumentException exeption)
            {
                DisplayAlert("Странные дела", exeption.Message, "Ой, ошибочка вышла");
            }

        }
        private async void GettingData()//Получение данных от веб-сервиса и передача их другой странице
        {
            bool isOnline = CrossConnectivity.Current.IsConnected;//Проверка на подключение к сети
            if (isOnline)
            {
                string[] tracks = TrackInput.Text.ToUpper().Split('|', '+');
                RussianPostClassLibrary.ParcelDescription[] parcelsDescriptions = await Task.WhenAll(tracks.Select(x => ReturnDataAboutOneParcel(x)));
                foreach (var item in parcelsDescriptions)
                {
                    ParcelDescriptionsBinding.Add(item);
                }
                TransfetData(parcelsDescriptions);
            }
            else
            {
                await DisplayAlert("Ошибка сети", "Проверьте подключение к сети", "OK");
            }
        }
        private async void TransfetData(RussianPostClassLibrary.ParcelDescription[] parcelsDescriptions)//Передаём данные на другую страницу
        {
            DataAboutEnteredParcels dataAboutEnteredParcels = new DataAboutEnteredParcels();
            await Navigation.PushAsync(dataAboutEnteredParcels);
            dataAboutEnteredParcels.AddNewParcels(parcelsDescriptions);
        }
        private Task<RussianPostClassLibrary.ParcelDescription> ReturnDataAboutOneParcel(string barcode)//Асинхронный метод для заполнения массива из GettingData()
        {
            return Task.Factory.StartNew(() =>
            {
                var result = DependencyService.Get<IReturnData>().ParcelDescription(barcode);
                return result;
            });
        }
    }
}