using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using XOutput.Logging;

namespace XOutput.UI.Windows
{
    public abstract class WindowBase<VM, M, C> : MetroWindow, IInitializableViewBase<VM, M, C> where VM : ViewModelBase<M> where M : ModelBase
    {
        private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(WindowBase<VM, M, C>));

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

        public void ShowAndWait(C context)
        {
            try
            {
                Initialize(context);
            }
            catch (Exception ex)
            {
                logger.Error("Error in initialize", ex);
            }
            try
            {
                ShowDialog();
            }
            catch (Exception ex)
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

        public abstract void Initialize(C context);
    }
    public abstract class ResultWindowBase<VM, M, C, R> : WindowBase<VM, M, C> where VM : ResultViewModelBase<M, R> where M : ModelBase
    {
        private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(ResultWindowBase<VM, M, C, R>));

        protected ResultWindowBase(VM viewModel) : base(viewModel)
        {

        }

        public R ShowAndWaitResult(C context)
        {
            try
            {
                Initialize(context);
            }
            catch (Exception ex)
            {
                logger.Error("Error in initialize", ex);
            }
            try
            {
                ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Error("Error in window", ex);
            }
            R result = ViewModel.Result;
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
