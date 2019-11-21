using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using XOutput.Logging;

namespace XOutput.UI.Component
{
    public abstract class ComponentBase<VM, M, C> : UserControl, IInitializableViewBase<VM, M, C> where VM : ViewModelBase<M> where M : ModelBase
    {
        private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(ComponentBase<VM, M, C>));

        public VM ViewModel { get; private set; }

        protected ComponentBase(VM viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
        }

        public virtual void CleanUp()
        {
            ViewModel.CleanUp();
        }

        public abstract void Initialize(C context);
    }
}
