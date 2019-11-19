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
using XOutput.Devices.XInput.SCPToolkit;
using XOutput.Devices.XInput.Vigem;
using XOutput.Diagnostics;
using XOutput.Logging;
using XOutput.Tools;
using XOutput.UI.Component;
using XOutput.Versioning;

namespace XOutput.UI.Windows
{
    public class MainWindowViewModel : ViewModelBase<MainWindowModel>, IDisposable
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

        private readonly DispatcherTimer timer = new DispatcherTimer();
        private readonly DirectInputDevices directInputDevices = new DirectInputDevices();
        private Action<string> log;
        private GeneralSettings settings;
        private bool installed;
        private bool isAdmin;

        [ResolverMethod(Scope.Prototype)]
        public MainWindowViewModel(MainWindowModel model, Dispatcher dispatcher, SettingsManager settingsManager, NotificationService notificationService,
            UpdateChecker updateChecker, LanguageManager languageManager, HidGuardianManager hidGuardianManager, CommandRunner commandRunner) : base(model)
        {
            this.dispatcher = dispatcher;
            this.settingsManager = settingsManager;
            this.notificationService = notificationService;
            this.updateChecker = updateChecker;
            this.languageManager = languageManager;
            this.hidGuardianManager = hidGuardianManager;
            this.commandRunner = commandRunner;

            timer.Interval = TimeSpan.FromMilliseconds(10000);
            timer.Tick += (object sender1, EventArgs e1) => { RefreshGameControllers(); };
            timer.Start();
            notificationService.NotificationAdded += NotificationAdded; 
        }

        public override void CleanUp()
        {
            timer.Stop();
            foreach (var notification in Model.Notifications)
            {
                notification.CloseRequested -= NotificationCloseRequested;
            }
            base.CleanUp();
        }

        public void Dispose()
        {
            foreach (var device in Model.Inputs.Select(x => x.ViewModel.Model.Device))
            {
                device.Dispose();
            }
            foreach (var controller in Model.Controllers.Select(x => x.ViewModel.Model.Controller))
            {
                controller.Dispose();
            }
            directInputDevices.Dispose();
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
                    isAdmin = true;
                    logger.Info("HidGuardian registry is set");
                    notificationService.Add("HidGuardianEnabledSuccessfully", new string[] { pid.ToString() }, TimeSpan.FromSeconds(10));
                }
                catch (UnauthorizedAccessException)
                {
                    isAdmin = false;
                    logger.Warning("Not running in elevated mode.");
                    notificationService.Add("HidGuardianNotAdmin");
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    MessageBox.Show(ex.ToString());
                }
            }
            bool vigem = VigemDevice.IsAvailable();
            bool scp = ScpDevice.IsAvailable();
            if (vigem)
            {
                if (scp)
                {
                    logger.Info("SCPToolkit is installed only.");
                    notificationService.Add("ScpInstalled", true);
                }
                installed = true;
            }
            else
            {
                if (scp)
                {
                    logger.Info("ViGEm is installed.");
                    notificationService.Add("VigemNotInstalled", true);
                    installed = true;
                }
                else
                {
                    logger.Error("Neither ViGEm nor SCPToolkit is installed.");
                    notificationService.Add("VigemAndScpNotInstalled", true);
                    installed = false;
                }
            }
            RefreshGameControllers();

            logger.Debug("Creating keyboard controller");
            Devices.Input.Keyboard.Keyboard keyboard = new Devices.Input.Keyboard.Keyboard();
            InputDevices.Instance.Add(keyboard);
            Model.Inputs.Add(new InputView(new InputViewModel(new InputModel(), keyboard, false)));
            logger.Debug("Creating mouse controller");
            Devices.Input.Mouse.Mouse mouse = new Devices.Input.Mouse.Mouse();
            InputDevices.Instance.Add(mouse);
            Model.Inputs.Add(new InputView(new InputViewModel(new InputModel(), mouse, false)));
            foreach (var mappingId in settingsManager.ListMappingSettingsIds())
            {
                var mapping = settingsManager.LoadMappingConfig(mappingId);
                AddController(mapping);
            }
            logger.Info("MainWindowViewModel is initialized");
        }

        private void NotificationAdded(NotificationData data)
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
            Model.Notifications.Remove(view);
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
                    await logger.Warning("Failed to check latest version");
                    notificationService.Add("VersionCheckError", true);
                    break;
                case VersionCompare.NeedsUpgrade:
                    await logger.Info("New version is available");
                    notificationService.Add("VersionCheckNeedsUpgrade", true);
                    break;
                case VersionCompare.NewRelease:
                    notificationService.Add("VersionCheckNewRelease", TimeSpan.FromSeconds(10), true);
                    break;
                case VersionCompare.UpToDate:
                    await logger.Info("Version is up-to-date");
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
                    Model.Inputs.Remove(inputView);
                    InputDevices.Instance.Remove(device);
                    inputView.ViewModel.Dispose();
                    device.Dispose();
                }
            }
            foreach (var instance in instances)
            {
                if (!Model.Inputs.Select(c => c.ViewModel.Model.Device).OfType<DirectDevice>().Any(d => d.Id == instance.InstanceGuid))
                {
                    var device = directInputDevices.CreateDirectDevice(instance);
                    device.InputConfiguration = settingsManager.LoadInputConfig(device.UniqueId);
                    if (device == null)
                    {
                        continue;
                    }
                    device.Disconnected -= DispatchRefreshGameControllers;
                    device.Disconnected += DispatchRefreshGameControllers;
                    Model.Inputs.Add(new InputView(new InputViewModel(new InputModel(), device, isAdmin)));
                }
            }
        }

        public void AddController(InputMapper mapper)
        {
            var gameController = new GameController(mapper ?? new InputMapper(Guid.NewGuid().ToString()));
            Controllers.Instance.Add(gameController);

            var controllerView = new ControllerView(new ControllerViewModel(new ControllerModel(), notificationService, gameController, isAdmin));
            controllerView.ViewModel.Model.CanStart = installed;
            controllerView.RemoveClicked += RemoveController;
            Model.Controllers.Add(controllerView);
            notificationService.Add("ControllerConnected", new string[] { gameController.DisplayName }, TimeSpan.FromSeconds(10));
            if (mapper?.StartWhenConnected == true)
            {
                controllerView.ViewModel.Start();
                logger.Info($"{mapper.Name} controller is started automatically.");
            }
        }

        public void RemoveController(ControllerView controllerView)
        {
            var controller = controllerView.ViewModel.Model.Controller;
            controllerView.ViewModel.Dispose();
            controller.Dispose();
            Model.Controllers.Remove(controllerView);
            logger.Info($"{controller.ToString()} is disconnected.");
            notificationService.Add("ControllerDisconnected", new string[] { controller.DisplayName }, TimeSpan.FromSeconds(5));
            Controllers.Instance.Remove(controller);
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
            SettingsResult result = settingsWindow.ShowAndWaitResult();
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
            Thread delayThread = new Thread(() =>
            {
                Thread.Sleep(1000);
                dispatcher.Invoke(RefreshGameControllers);
            })
            {
                Name = "Device list refresh delay",
                IsBackground = true
            };
            delayThread.Start();
        }
    }
}
