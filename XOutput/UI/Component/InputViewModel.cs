using System;
using System.Windows.Media;
using System.Windows.Threading;
using XOutput.Devices;
using XOutput.Devices.Input;
using XOutput.Tools;
using XOutput.UI.Windows;

namespace XOutput.UI.Component
{
    public class InputViewModel : ViewModelBase<InputModel>
    {
        private const int BackgroundDelayMS = 500;
        private readonly DispatcherTimer timer = new DispatcherTimer();

        [ResolverMethod(Scope.Prototype)]
        public InputViewModel(InputModel model) : base(model)
        {
            Model.Background = Brushes.White;
            timer.Interval = TimeSpan.FromMilliseconds(BackgroundDelayMS);
            timer.Tick += Timer_Tick;
        }

        public void Initialize(IInputDevice device)
        {
            Model.Device = device;
            Model.DisplayName = string.Format("{0} ({1})", device.DisplayName, device.UniqueId);
            Model.Device.InputChanged += InputDevice_InputChanged;
        }

        public override void CleanUp()
        {
            timer.Tick -= Timer_Tick;
            Model.Device.InputChanged -= InputDevice_InputChanged;
            base.CleanUp();
        }

        public void Edit()
        {
            var controllerSettingsWindow = ApplicationContext.Global.Resolve<InputSettingsWindow>();
            controllerSettingsWindow.ShowAndWait(new InputSettingsContext { InputDevice = Model.Device });
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
