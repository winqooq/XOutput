using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOutput.UI
{
    public class NotifyingCollection<T> : ObservableCollection<T>
    {

        public NotifyingCollection()
        {

        }

        public NotifyingCollection(IEnumerable<T> collection) : base(collection)
        {

        }
    }
}
