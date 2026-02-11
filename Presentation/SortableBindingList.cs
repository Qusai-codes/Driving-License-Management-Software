using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool isSorted;
        private ListSortDirection sortDirection;
        private PropertyDescriptor sortProperty;

        protected override bool SupportsSortingCore => true;
        protected override bool IsSortedCore => isSorted;

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            var items = (List<T>)Items;
            var comparer = new PropertyComparer<T>(prop, direction);

            items.Sort(comparer);

            sortProperty = prop;
            sortDirection = direction;
            isSorted = true;

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
        }
    }

    public class PropertyComparer<T> : IComparer<T>
    {
        private PropertyDescriptor prop;
        private ListSortDirection direction;

        public PropertyComparer(PropertyDescriptor prop, ListSortDirection direction)
        {
            this.prop = prop;
            this.direction = direction;
        }

        public int Compare(T x, T y)
        {
            var xValue = prop.GetValue(x);
            var yValue = prop.GetValue(y);

            int result = Comparer.Default.Compare(xValue, yValue);

            return direction == ListSortDirection.Ascending ? result : -result;
        }
    }
}
