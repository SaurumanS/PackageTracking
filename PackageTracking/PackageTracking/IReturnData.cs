using System;
using System.Collections.Generic;
using System.Text;

using RussianPostClassLibrary;

namespace PackageTracking
{
    public interface IReturnData //Интерфейс для реализации DepencyProperty
    {
        RussianPostClassLibrary.ParcelDescription ParcelDescription(string barcode);
    }
}
