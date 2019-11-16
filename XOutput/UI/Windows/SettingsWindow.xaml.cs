using MahApps.Metro.Controls;
using System.Windows;
using XOutput.Tools;

namespace XOutput.UI.Windows
{
    public partial class SettingsWindow : ResultWindowBase<SettingsViewModel, SettingsModel, SettingsResult>
    {
        [ResolverMethod(Scope.Prototype)]
        public SettingsWindow(SettingsViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
            Closing += Window_Closing;
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Close();
            Close();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Save();
            Close();
        }

        private void Changed(object sender, RoutedEventArgs e)
        {
            ViewModel.SetChanged();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ViewModel.NeedConfirmation())
            {
                e.Cancel = true;
                ViewModel.Close(); // TODO ask Yes/No/Cancel
            }
        }

        public override void CleanUp()
        {
            base.CleanUp();
            Closing -= Window_Closing;
        }
    }
}
