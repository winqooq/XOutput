using System;
using System.ComponentModel;
using System.Reflection;

namespace XOutput.UI
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool cleanup = false;

        /// <summary>
        /// Invokes the property changed event
        /// </summary>
        /// <param name="name">Name of the property that changed</param>
        protected void OnPropertyChanged(params string[] propertyNames)
        {
            if (cleanup)
            {
                return;
            }
            foreach (var propertyName in propertyNames)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool Set<T>(T newValue, T currentValue, Action<T> setter, params string[] propertyNames)
        {
            if (cleanup)
            {
                return false;
            }
            bool changed = false;
            if (!Equals(currentValue, newValue))
            {
                setter(newValue);
                OnPropertyChanged(propertyNames);
                changed = true;
            }
            return changed;
        }

        public virtual void CleanUp()
        {
            cleanup = true;
        }
    }
}
