using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using XOutput.Logging;

namespace XOutput.UI
{
    public abstract class WindowBase<VM, M> : MetroWindow, IViewBase<VM, M> where VM : ViewModelBase<M> where M : ModelBase
    {
        private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(WindowBase<VM, M>));

        public VM ViewModel { get; private set; }

        protected WindowBase(VM viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
        }

        public virtual void CleanUp()
        {
            ViewModel.CleanUp();
        }

        public void ShowAndWait()
        {
            try
            {
                ShowDialog();
            }
            catch(Exception ex)
            {
                logger.Error("Error in window", ex);
            }
            try
            {
                CleanUp();
            }
            catch (Exception ex)
            {
                logger.Error("Error cleaning up window", ex);
            }
        }
    }
}
