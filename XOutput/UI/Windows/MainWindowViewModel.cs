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
        private const string SettingsFilePath = "settings.json";
        private const string GameControllersSettings = "joy.cpl";

        private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(MainWindowViewModel));

        private readonly LanguageManager languageManager;
        private readonly HidGuardianManager hidGuardianManager;
        private readonly SettingsManager settingsManager;
        private readonly Dispatcher dispatcher;
        private readonly UpdateChecker updateChecker;

        private readonly DispatcherTimer timer = new DispatcherTimer();
        private readonly DirectInputDevices directInputDevices = new DirectInputDevices();
        private Action<string> log;
        private GeneralSettings settings;
        private bool installed;
        private bool isAdmin;

        [ResolverMethod(Scope.Prototype)]
        public MainWindowViewModel(MainWindowModel model, Dispatcher dispatcher, SettingsManager settingsManager,
            UpdateChecker updateChecker, LanguageManager languageManager, HidGuardianManager hidGuardianManager) : base(model)
        {
            this.languageManager = languageManager;
            this.hidGuardianManager = hidGuardianManager;
            this.settingsManager = settingsManager;
            this.dispatcher = dispatcher;
            this.updateChecker = updateChecker;

            timer.Interval = TimeSpan.FromMilliseconds(10000);
            timer.Tick += (object sender1, EventArgs e1) => { RefreshGameControllers(); };
            timer.Start();
        }

        public override void CleanUp()
        {
            timer.Stop();
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

        public void Initialize(Action<string> log)
        {
            this.log = log;
            try
            {
                settings = settingsManager.LoadGeneralSettings();
                languageManager.Language = settings.Language;
                logger.Info("Loading settings was successful.");
                log(string.Format(Translate("LoadSettingsSuccess"), SettingsFilePath));
            }
            catch (Exception ex)
            {
                logger.Warning("Loading settings was unsuccessful.");
                string error = string.Format(Translate("LoadSettingsError"), SettingsFilePath) + Environment.NewLine + ex.Message;
                log(error);
                MessageBox.Show(error, Translate("Warning"));
            }
            if (settings.HidGuardianEnabled)
            {
                try
                {
                    hidGuardianManager.ResetPid(pid);
                    isAdmin = true;
                    logger.Info("HidGuardian registry is set");
                    log(string.Format(Translate("HidGuardianEnabledSuccessfully"), pid.ToString()));
                }
                catch (UnauthorizedAccessException)
                {
                    isAdmin = false;
                    logger.Warning("Not running in elevated mode.");
                    log(Translate("HidGuardianNotAdmin"));
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
                    log(Translate("ScpInstalled"));
                }
                installed = true;
            }
            else
            {
                if (scp)
                {
                    logger.Info("ViGEm is installed.");
                    log(Translate("VigemNotInstalled"));
                    installed = true;
                }
                else
                {
                    logger.Error("Neither ViGEm nor SCPToolkit is installed.");
                    string error = Translate("VigemAndScpNotInstalled");
                    log(error);
                    installed = false;
                    MessageBox.Show(error, Translate("Error"));
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
            log(string.Format(LanguageModel.Instance.Translate("ControllerConnected"), LanguageModel.Instance.Translate("Keyboard")));
            logger.Info("Keyboard controller is connected");
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
            var result = await new UpdateChecker().CompareRelease();
            switch (result)
            {
                case VersionCompare.Error:
                    logger.Warning("Failed to check latest version");
                    log(Translate("VersionCheckError"));
                    break;
                case VersionCompare.NeedsUpgrade:
                    logger.Info("New version is available");
                    log(Translate("VersionCheckNeedsUpgrade"));
                    break;
                case VersionCompare.NewRelease:
                    log(Translate("VersionCheckNewRelease"));
                    break;
                case VersionCompare.UpToDate:
                    logger.Info("Version is up-to-date");
                    log(Translate("VersionCheckUpToDate"));
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

            var controllerView = new ControllerView(new ControllerViewModel(new ControllerModel(), gameController, isAdmin, log));
            controllerView.ViewModel.Model.CanStart = installed;
            controllerView.RemoveClicked += RemoveController;
            Model.Controllers.Add(controllerView);
            log(string.Format(LanguageModel.Instance.Translate("ControllerConnected"), gameController.DisplayName));
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
            log(string.Format(LanguageModel.Instance.Translate("ControllerDisconnected"), controller.DisplayName));
            Controllers.Instance.Remove(controller);
            settingsManager.DeleteMappingSettings(controller.Mapper);
        }

        public void OpenWindowsGameControllerSettings()
        {
            logger.Debug("Starting " + GameControllersSettings);
            new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C " + GameControllersSettings,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                },
            }.Start();
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
