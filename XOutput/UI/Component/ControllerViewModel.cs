using System;
using System.Windows.Media;
using System.Windows.Threading;
using XOutput.Devices;
using XOutput.Tools;
using XOutput.UI.Windows;

namespace XOutput.UI.Component
{
    public class ControllerViewModel : ViewModelBase<ControllerModel>
    {
        private const int BackgroundDelayMS = 500;

        private readonly NotificationService notificationService;

        private readonly DispatcherTimer timer = new DispatcherTimer();
        private GameController controller;

        [ResolverMethod(Scope.Prototype)]
        public ControllerViewModel(ControllerModel model, NotificationService notificationService) : base(model)
        {
            this.notificationService = notificationService;

            Model.ButtonText = "Start";
            Model.Background = Brushes.White;
            timer.Interval = TimeSpan.FromMilliseconds(BackgroundDelayMS);
            timer.Tick += Timer_Tick;
        }

        public void Initialize(GameController controller, bool canStart)
        {
            this.controller = controller;
            Model.CanStart = canStart;
            Model.DisplayName = controller.DisplayName;
            controller.XInput.InputChanged += InputDevice_InputChanged;
        }

        public override void CleanUp()
        {
            timer.Tick -= Timer_Tick;
            controller.XInput.InputChanged -= InputDevice_InputChanged;
            base.CleanUp();
        }

        public void Edit()
        {
            var window = ApplicationContext.Global.Resolve<ControllerSettingsWindow>();
            window.Initialize(controller);
            window.ShowAndWait();
            Model.DisplayName = controller.DisplayName;
        }

        public void StartStop()
        {
            if (!Model.Started)
            {
                Start();
            }
            else
            {
                controller.Stop();
            }
        }

        public void Start()
        {
            if (!Model.Started)
            {
                int controllerCount = 0;
                controllerCount = controller.Start(() =>
                {
                    Model.ButtonText = "Start";
                    notificationService.Add("EmulationStopped", new string[] { controller.DisplayName }, TimeSpan.FromSeconds(10));
                    Model.Started = false;
                });
                if (controllerCount != 0)
                {
                    Model.ButtonText = "Stop";
                    notificationService.Add("EmulationStarted", new string[] { controller.DisplayName, controllerCount.ToString() }, TimeSpan.FromSeconds(10));
                }
                Model.Started = controllerCount != 0;
            }
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
