using System;
using System.Windows;
using System.Windows.Threading;
using XOutput.Devices.Input;
using XOutput.Tools;

namespace XOutput.UI.Windows
{
    public partial class InputSettingsWindow : WindowBase<InputSettingsViewModel, InputSettingsModel>
    {
        private readonly SettingsManager settingsManager;

        private IInputDevice device;

        [ResolverMethod(Scope.Prototype)]
        public InputSettingsWindow(InputSettingsViewModel viewModel, SettingsManager settingsManager) : base(viewModel)
        {
            this.settingsManager = settingsManager;
            InitializeComponent();
        }

        public void Initialize(IInputDevice device, bool isAdmin)
        {
            this.device = device;
            device.Disconnected += Disconnected;
            ViewModel.Initialize(device, isAdmin);
        }

        public override void CleanUp()
        {
            device.Disconnected -= Disconnected;
            base.CleanUp();
        }

        void Disconnected(object sender, DeviceDisconnectedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Close();
            });
        }

        private void ForceFeedbackButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.TestForceFeedback();
        }

        private void ForceFeedbackCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.SetForceFeedbackEnabled();
        }

        private void AddHidGuardianButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.AddHidGuardian();
        }

        private void RemoveHidGuardianButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveHidGuardian();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            settingsManager.SaveInputSettings(device.InputConfiguration);
            Close();
        }
    }
}
