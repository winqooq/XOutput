using System;
using System.Windows.Media;
using System.Windows.Threading;
using XOutput.Devices;
using XOutput.UI.Windows;

namespace XOutput.UI.Component
{
    public class ControllerViewModel : ViewModelBase<ControllerModel>, IDisposable
    {
        private const int BackgroundDelayMS = 500;

        private readonly NotificationService notificationService;

        private readonly DispatcherTimer timer = new DispatcherTimer();

        public ControllerViewModel(ControllerModel model, NotificationService notificationService, GameController controller) : base(model)
        {
            this.notificationService = notificationService;

            Model.Controller = controller;
            Model.ButtonText = "Start";
            Model.Background = Brushes.White;
            Model.Controller.XInput.InputChanged += InputDevice_InputChanged;
            timer.Interval = TimeSpan.FromMilliseconds(BackgroundDelayMS);
            timer.Tick += Timer_Tick;
        }

        public void Edit()
        {
            var controllerSettingsWindow = new ControllerSettingsWindow(new ControllerSettingsViewModel(new ControllerSettingsModel(), Model.Controller), Model.Controller);
            controllerSettingsWindow.ShowDialog();
            Model.RefreshName();
        }

        public void StartStop()
        {
            if (!Model.Started)
            {
                Start();
            }
            else
            {
                Model.Controller.Stop();
            }
        }

        public void Start()
        {
            if (!Model.Started)
            {
                int controllerCount = 0;
                controllerCount = Model.Controller.Start(() =>
                {
                    Model.ButtonText = "Start";
                    notificationService.Add("EmulationStopped", new string[] { Model.Controller.DisplayName }, TimeSpan.FromSeconds(10));
                    Model.Started = false;
                });
                if (controllerCount != 0)
                {
                    Model.ButtonText = "Stop";
                    notificationService.Add("EmulationStarted", new string[] { Model.Controller.DisplayName, controllerCount.ToString() }, TimeSpan.FromSeconds(10));
                }
                Model.Started = controllerCount != 0;
            }
        }

        public void Dispose()
        {
            timer.Tick -= Timer_Tick;
            Model.Controller.XInput.InputChanged -= InputDevice_InputChanged;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Model.Background = Brushes.White;
        }

        private void InputDevice_InputChanged(object sender, DeviceInputChangedEventArgs e)
        {
            Model.Background = Brushes.LightGreen;
            timer.Stop();
            timer.Start();
        }
    }
}
