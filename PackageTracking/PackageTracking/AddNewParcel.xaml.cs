using Plugin.Connectivity;
using System;
using System.Collections.Generic;
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
        string checkEntry = null;
        public AddNewParcel()
        {
            InitializeComponent();
            TrackInput.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeCharacter);
        }

        

        private void TrackInput_TextChanged(object sender, TextChangedEventArgs e)//Событие на ввод текста в поле (проверка на корректность ввода)
        {
            if (checkEntry == e.NewTextValue) return;
            checkEntry = e.OldTextValue;
            if (string.IsNullOrEmpty(e.NewTextValue)) return;
            if (e.NewTextValue.Length > 130)
            {
                ((Entry)sender).Text = e.OldTextValue;
                TrackInputLabel.IsVisible = true;
                TrackInputLabel.Text = "Поле ввода не должно содержать больше 130 символов";
            }
            else if (!IsDigitsOnly(e.NewTextValue.Last().ToString()) && !IsLettersOnly(e.NewTextValue.Last().ToString()) && e.NewTextValue.Last() != '+' && e.NewTextValue.Last() != '|')
            {
                ((Entry)sender).Text = e.OldTextValue;
                TrackInputLabel.IsVisible = true;
                TrackInputLabel.Text = "Данный символ запрещен для ввода";
            }
            else TrackInputLabel.IsVisible = false;
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
        bool IsLettersOnly(string str)//Проверяет содержит ли строка только буквы
        {
            foreach (char c in str)
            {
                if (c < 'A' || c > 'Z')
                    return false;
            }

            return true;
        }
        private void TrackInput_Completed(object sender, EventArgs e)
        {
            SendTrackButton_Clicked(sender, e);
        }

        private void SendTrackButton_Clicked(object sender, EventArgs e)
        {
            GettingData();
        }
        private async void GettingData()
        {
            bool isOnline = CrossConnectivity.Current.IsConnected;
            if (isOnline && CheckOnCorrectSpelling())
            {
                string[] tracks = TrackInput.Text.ToUpper().Split('|', '+');
                var parcelsDescriptions = await Task.WhenAll(tracks.Select(x => ReturnDataAboutOneParcel(x)));
            }
            else
            {
                await DisplayAlert("Ошибка сети", "Проверьте подключение к сети", "OK");
            }
        }
        private Task<RussianPostClassLibrary.ParcelDescription> ReturnDataAboutOneParcel(string barcode)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = DependencyService.Get<IReturnData>().ParcelDescription(barcode);
                return result;
            });
        }
        private bool CheckOnCorrectSpelling()
        {
            if (string.IsNullOrEmpty(TrackInput.Text))
            {
                TrackInputLabel.IsVisible = true;
                TrackInputLabel.Text = "Хмм...кажется поле для ввода пусто.";
                return false;
            }
            else
            {
                try
                {
                    string[] tracks = TrackInput.Text.ToUpper().Split('|', '+');
                    for (int index = 0; index < tracks.Length; index++)
                    {
                        if (tracks[index].First() >= 'A' && tracks[index].First() <= 'Z')
                        {
                            if (tracks[index].Length != 13) throw new ArgumentException("Длина трек-кода (" + tracks[index] + ") неверная");
                            string temp = tracks[index];
                            temp = temp.TrimStart('C', 'E', 'L', 'R', 'S', 'V', 'Z');
                            if (temp.Length == tracks[index].Length) throw new ArgumentException("Данный трек-код (" + tracks[index] + ") не отслеживается смотри пример");
                            bool check = true;
                            for (int counter = 1; counter < tracks[index].Length; counter++)
                            {
                                if (counter == 1 || counter == 11 || counter == 12) check = IsLettersOnly(tracks[index][counter].ToString());
                                else check = IsDigitsOnly(tracks[index][counter].ToString());
                                if (!check) throw new ArgumentException("Ошибка ввода трек-кода (" + tracks[index] + "), смотри пример трек-кода");
                            }
                        }
                    }
                    return true;
                }
                catch (ArgumentException exeption) { DisplayAlert("Ошибка ввода", exeption.Message, "OK"); }
                return false;
            }


        }
    }
}