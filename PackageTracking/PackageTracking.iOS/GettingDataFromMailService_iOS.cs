using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PackageTracking.iOS.GettingDataFromMailService_iOS))]
namespace PackageTracking.iOS
{
    class GettingDataFromMailService_iOS: IReturnData
    {
        public RussianPostClassLibrary.ParcelDescription ParcelDescription(string barcode)
        {

        }
    }
}