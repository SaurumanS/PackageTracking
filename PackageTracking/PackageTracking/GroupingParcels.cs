using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PackageTracking
{
    public class GroupingParcels<K,T> : ObservableCollection<T>//Вспомогательный класс для группировки данных о посылках по их состоянию
    {
        public GroupingParcels(K currentlyStatusParcel, IEnumerable<T> parcelDescription)
        {
            CurrentlyStatusParcel = currentlyStatusParcel;
            foreach(var item in parcelDescription)
            {
                Items.Add(item);
            }
        }
        public K CurrentlyStatusParcel { get; private set; }
    }
}
