using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using XOutput.Logging;

namespace XOutput.UI
{
    public abstract class ResultWindowBase<VM, M, R> : WindowBase<VM, M> where VM : ResultViewModelBase<M, R> where M : ModelBase
    {
        private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(ResultWindowBase<VM, M, R>));

        protected ResultWindowBase(VM viewModel) : base(viewModel)
        {

        }

        public R ShowAndWaitResult()
        {
            try
            {
                ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Error("Error in window", ex);
            }
            var result = ViewModel.Result;
            try
            {
                CleanUp();
            }
            catch (Exception ex)
            {
                logger.Error("Error cleaning up window", ex);
            }
            return result;
        }
    }
}
