using MahApps.Metro.Controls;
using System.ComponentModel;

namespace XOutput.UI
{
    public abstract class WindowBase<VM, M> : MetroWindow, IViewBase<VM, M> where VM : ViewModelBase<M> where M : ModelBase
    {
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
            ShowDialog();
            CleanUp();
        }
    }
}
