using MahApps.Metro.Controls;
using System.ComponentModel;

namespace XOutput.UI
{
    public abstract class ResultWindowBase<VM, M, R> : WindowBase<VM, M> where VM : ResultViewModelBase<M, R> where M : ModelBase
    {
        protected ResultWindowBase(VM viewModel) : base(viewModel)
        {

        }

        public R ShowAndWaitResult()
        {
            ShowDialog();
            var result = ViewModel.Result;
            CleanUp();
            return result;
        }
    }
}
