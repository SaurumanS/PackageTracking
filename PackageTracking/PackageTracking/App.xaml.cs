using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RussianPostClassLibrary;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PackageTracking
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
