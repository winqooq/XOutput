using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using XOutput.Devices;
using XOutput.Devices.Input;
using XOutput.Devices.Input.DirectInput;
using XOutput.Devices.Mapper;
using XOutput.Devices.XInput;
using XOutput.Devices.XInput.SCPToolkit;
using XOutput.Devices.XInput.Vigem;
using XOutput.Diagnostics;
using XOutput.Logging;
using XOutput.Tools;
using XOutput.UI.Component;
using XOutput.Versioning;

namespace XOutput.UI.Windows
{
    public class MainWindowViewModel : ViewModelBase<MainWindowModel>
    {

        private readonly int pid = Process.GetCurrentProcess().Id;
        private const string SettingsFilePath = "config/general.json";
        private const string GameControllersSettings = "joy.cpl";

        private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(MainWindowViewModel));

        private readonly Dispatcher dispatcher;
        private readonly SettingsManager settingsManager;
        private readonly NotificationService notificationService;
        private readonly UpdateChecker updateChecker;
        private readonly LanguageManager languageManager;
        private readonly HidGuardianManager hidGuardianManager;
        private readonly CommandRunner commandRunner;
        private readonly XOutputManager xOutputManager;

        private readonly DispatcherTimer timer = new DispatcherTimer();
        private readonly DirectInputDevices directInputDevices = new DirectInputDevices();
        private GeneralSettings settings;
        private readonly Dictionary<ControllerView, GameController> controllerMap = new Dictionary<ControllerView, GameController>();

        [ResolverMethod(Scope.Prototype)]
        public MainWindowViewModel(MainWindowModel model, Dispatcher dispatcher, SettingsManager settingsManager, NotificationService notificationService,
            UpdateChecker updateChecker, LanguageManager languageManager, HidGuardianManager hidGuardianManager, CommandRunner commandRunner,
            XOutputManager xOutputManager) : base(model)
        {
            this.dispatcher = dispatcher;
            this.settingsManager = settingsManager;
            this.notificationService = notificationService;
            this.updateChecker = updateChecker;
            this.languageManager = languageManager;
            this.hidGuardianManager = hidGuardianManager;
            this.commandRunner = commandRunner;
            this.xOutputManager = xOutputManager;

            timer.Interval = TimeSpan.FromMilliseconds(10000);
            timer.Tick += Timer_Tick;
            timer.Start();
            notificationService.NotificationAdded += NotificationAdded;
        }

        private void Timer_Tick(object sender, EventArgs e)
        { 
             RefreshGameControllers();
        }

        public override void CleanUp()
        {
            timer.Tick -= Timer_Tick;
            timer.Stop();
            directInputDevices.Dispose();
            InputDevices.Instance.Dispose();
            Controllers.Instance.Dispose();
            foreach (var notification in Model.Notifications)
            {
                notification.CloseRequested -= NotificationCloseRequested;
            }
            base.CleanUp();
        }

        public void Initialize()
        {
            try
            {
                settings = settingsManager.LoadGeneralSettings();
                languageManager.Language = settings.Language;
                logger.Info("Loading settings was successful."); 
                notificationService.Add("LoadSettingsSuccess", new string[] { SettingsFilePath }, TimeSpan.FromSeconds(10));
            }
            catch (Exception ex)
            {
                logger.Warning("Loading settings was unsuccessful." + ex.Message);
                notificationService.Add("LoadSettingsError", new string[] { SettingsFilePath }, true);
            }
            if (settings.HidGuardianEnabled)
            {
                try
                {
                    hidGuardianManager.ResetPid(pid);
                    logger.Info("HidGuardian registry is set");
                    notificationService.Add("HidGuardianEnabledSuccessfully", new string[] { pid.ToString() }, TimeSpan.FromSeconds(10));
                }
                catch (UnauthorizedAccessException)
                {
                    logger.Warning("Not running in elevated mode.");
                    notificationService.Add("HidGuardianNotAdmin");
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    MessageBox.Show(ex.ToString());
                }
            }
            if (xOutputManager.IsVigem)
            {
                if (xOutputManager.IsScp)
                {
                    logger.Info("SCPToolkit is obsolete.");
                    notificationService.Add("ScpInstalled", true);
                }
                else
                {
                    logger.Info("ViGEm is installed.");
                }
            }
            else if (xOutputManager.IsScp)
            {
                    logger.Info("ScpToolkit is installed only.");
                    notificationService.Add("VigemNotInstalled", true);
            }
            else
            {
                logger.Error("Neither ViGEm nor SCPToolkit is installed.");
                notificationService.Add("VigemAndScpNotInstalled", true);
            }
            RefreshGameControllers();

            logger.Debug("Creating keyboard controller");
            Devices.Input.Keyboard.Keyboard keyboard = new Devices.Input.Keyboard.Keyboard();
            InputDevices.Instance.Add(keyboard);
            InputView keyboardInputView = ApplicationContext.Global.Resolve<InputView>();
            keyboardInputView.ViewModel.Initialize(keyboard);
            Model.Inputs.Add(keyboardInputView);
            logger.Debug("Creating mouse controller");
            Devices.Input.Mouse.Mouse mouse = new Devices.Input.Mouse.Mouse();
            InputDevices.Instance.Add(mouse);
            InputView mouseInputView = ApplicationContext.Global.Resolve<InputView>();
            mouseInputView.ViewModel.Initialize(mouse);
            Model.Inputs.Add(mouseInputView);
            foreach (var mappingId in settingsManager.ListMappingSettingsIds())
            {
                var mapping = settingsManager.LoadMappingConfig(mappingId);
                AddController(mapping);
            }
            logger.Info("MainWindowViewModel is initialized");
        }

        private void NotificationAdded(NotificationData data)
        {
            if (!dispatcher.HasShutdownStarted)
            {
                dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() => NotificationCreate(data)));
            }
        }

        private void NotificationCreate(NotificationData data)
        {
            var notificationView = ApplicationContext.Global.Resolve<NotificationView>();
            notificationView.SetNotificationData(data);
            Model.Notifications.Add(notificationView);
            notificationView.CloseRequested += NotificationCloseRequested;
            Model.NotificationCount = Model.Notifications.Count;
        }

        private void NotificationCloseRequested(NotificationView view)
        {
            view.CloseRequested -= NotificationCloseRequested;
            Model.Notifications.RemoveView(view);
            Model.NotificationCount = Model.Notifications.Count;
        }

        internal GeneralSettings GetSettings()
        {
            return settings;
        }

        public void AboutPopupShow()
        {
            MessageBox.Show(Translate("AboutContent") + Environment.NewLine + string.Format(Translate("Version"), Versioning.Version.AppVersion), Translate("AboutMenu"));
        }

        public async Task CompareVersion()
        {
            var result = await updateChecker.CompareRelease();
            switch (result)
            {
                case VersionCompare.Error:
                    logger.Warning("Failed to check latest version");
                    notificationService.Add("VersionCheckError", true);
                    break;
                case VersionCompare.NeedsUpgrade:
                    logger.Info("New version is available");
                    notificationService.Add("VersionCheckNeedsUpgrade", true);
                    break;
                case VersionCompare.NewRelease:
                    notificationService.Add("VersionCheckNewRelease", TimeSpan.FromSeconds(10), true);
                    break;
                case VersionCompare.UpToDate:
                    logger.Info("Version is up-to-date");
                    notificationService.Add("VersionCheckUpToDate", TimeSpan.FromSeconds(10));
                    break;
                default:
                    throw new ArgumentException(nameof(result));
            }
        }

        public void RefreshGameControllers()
        {
            IEnumerable<SharpDX.DirectInput.DeviceInstance> instances = directInputDevices.GetInputDevices(settings.ShowAllDevices);

            foreach (var inputView in Model.Inputs.ToArray())
            {
                var device = inputView.ViewModel.Model.Device;
                if (device is DirectDevice && (!instances.Any(x => x.InstanceGuid == ((DirectDevice)device).Id) || !device.Connected))
                {
                    Model.Inputs.RemoveView(inputView);
                    InputDevices.Instance.Remove(device);
                    device.Dispose();
                }
            }
            foreach (var instance in instances)
            {
                if (!Model.Inputs.Select(c => c.ViewModel.Model.Device).OfType<DirectDevice>().Any(d => d.Id == instance.InstanceGuid))
                {
                    var device = directInputDevices.CreateDirectDevice(instance);
                    if (device == null)
                    {
                        continue;
                    }
                    device.InputConfiguration = settingsManager.LoadInputConfig(device.UniqueId);
                    device.Disconnected -= DispatchRefreshGameControllers;
                    device.Disconnected += DispatchRefreshGameControllers;

                    InputView inputView = ApplicationContext.Global.Resolve<InputView>();
                    inputView.ViewModel.Initialize(device);
                    Model.Inputs.Add(inputView);
                }
            }
        }

        public void AddController(InputMapper mapper)
        {
            var gameController = new GameController(mapper ?? new InputMapper(Guid.NewGuid().ToString()));
            Controllers.Instance.Add(gameController);

            ControllerView controllerView = ApplicationContext.Global.Resolve<ControllerView>();
            controllerView.Initialize(new ControllerContext { Controller = gameController });
            controllerView.RemoveClicked += RemoveController;
            Model.Controllers.Add(controllerView);
            notificationService.Add("ControllerConnected", new string[] { gameController.DisplayName }, TimeSpan.FromSeconds(10));
            if (mapper?.StartWhenConnected == true)
            {
                controllerView.ViewModel.Start();
                logger.Info($"{mapper.Name} controller is started automatically.");
            }
        }

        public void RemoveController(ControllerView controllerView, GameController controller)
        {
            Model.Controllers.RemoveView(controllerView);
            logger.Info($"{controller.ToString()} is disconnected.");
            notificationService.Add("ControllerDisconnected", new string[] { controller.DisplayName }, TimeSpan.FromSeconds(5));
            Controllers.Instance.Remove(controller);
            controller.Dispose();
            settingsManager.DeleteMappingSettings(controller.Mapper);
        }

        public void OpenWindowsGameControllerSettings()
        {
            logger.Debug("Starting " + GameControllersSettings);
            commandRunner.StartCmdAsync(GameControllersSettings);
            logger.Debug("Started " + GameControllersSettings);
        }

        public void OpenSettings()
        {
            SettingsWindow settingsWindow = ApplicationContext.Global.Resolve<SettingsWindow>();
            SettingsResult result = settingsWindow.ShowAndWaitResult(new SettingsContext());
        }

        public void OpenDiagnostics()
        {
            IList<IDiagnostics> elements = InputDevices.Instance.GetDevices()
                .Select(d => new InputDiagnostics(d)).OfType<IDiagnostics>().ToList();
            elements.Insert(0, new Devices.XInput.XInputDiagnostics());

            ApplicationContext context = ApplicationContext.Global.WithSingletons(new DiagnosticsModel(elements));
            DiagnosticsWindow diagnosticsWindow = context.Resolve<DiagnosticsWindow>();
            diagnosticsWindow.ShowDialog();
        }

        private string Translate(string key)
        {
            return LanguageModel.Instance.Translate(key);
        }

        private void DispatchRefreshGameControllers(object sender, DeviceDisconnectedEventArgs e)
        {
            Thread delayThread = ThreadHelper.CreateAndStart(new ThreadStartParameters
            {
                Name = "Device list refresh delay",
                IsBackground = true,
                Task = () => {
                    Thread.Sleep(1000);
                    if (!dispatcher.HasShutdownStarted)
                    {
                        dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) RefreshGameControllers);
                    }
                },
            });
        }
    }
}
