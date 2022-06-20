using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core
{
    /// <summary>
    /// List Extensions.
    /// From https://stackoverflow.com/questions/1766328/can-linq-use-binary-search-when-the-collection-is-ordered
    /// </summary>
    public static class ListExtensions
    {
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var sortableList = new List<T>(collection);
            sortableList.Sort(comparison);

            for (int i = 0; i < sortableList.Count; i++)
            {
                collection.Move(collection.IndexOf(sortableList[i]), i);
            }
        }
    }
}
