using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using XOutput.Logging;

namespace XOutput.UI
{
    public abstract class ModelBase : INotifyPropertyChanged, ICleanUp
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

        protected bool Set<T>(T newValue, ref T currentValue, params string[] propertyNames)
        {
            if (cleanup)
            {
                return false;
            }
            bool changed = false;
            if (!Equals(currentValue, newValue))
            {
                currentValue = newValue;
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

    public static class CollectionHelper
    {
        private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(CollectionHelper));

        public static void RemoveView<T>(this ObservableCollection<T> collection, params T[] values) where T : ICleanUp
        {
            foreach (var value in values)
            {
                if (collection.Remove(value))
                {
                    try
                    {
                        value.CleanUp();
                    }
                    catch(Exception ex)
                    {
                        logger.Error("Error while cleanup view", ex);
                    }
                }
            }
        }
        public static void ClearView<T>(this ObservableCollection<T> collection) where T : ICleanUp
        {
            foreach (var value in collection)
            {
                try
                {
                    value.CleanUp();
                }
                catch (Exception ex)
                {
                    logger.Error("Error while cleanup view", ex);
                }
            }
            collection.Clear();
        }
    }
}
