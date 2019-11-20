using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using XOutput.Logging;
using XOutput.Tools;

namespace XOutput.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase<MainWindowViewModel, MainWindowModel>
    {
        private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(MainWindow));

        private bool hardExit = false;
        private WindowState restoreState = WindowState.Normal;

        [ResolverMethod(Scope.Prototype)]
        public MainWindow(MainWindowViewModel viewModel, ArgumentParser argumentParser) : base(viewModel)
        {
            if (argumentParser.Minimized)
            {
                Visibility = Visibility.Hidden;
                ShowInTaskbar = false;
                logger.Info("Starting XOutput in minimized to taskbar");
            }
            else
            {
                ShowInTaskbar = true;
                logger.Info("Starting XOutput in normal window");
            }
            new WindowInteropHelper(this).EnsureHandle();
            InitializeComponent();
            Closed += WindowClosed;
            Closing += WindowClosing;
            viewModel.Initialize();
            Dispatcher.Invoke(Initialize);
            logger.Info("The application has started.");
        }

        public override void CleanUp()
        {
            base.CleanUp();
            Closed -= WindowClosed;
            Closing -= WindowClosing;
        }

        private async Task Initialize()
        {
            await ViewModel.CompareVersion();
        }

        private void AddControllerClick(object sender, RoutedEventArgs e)
        {
            ViewModel.AddController(null);
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            ViewModel.RefreshGameControllers();
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            hardExit = true;
            if (IsLoaded)
            {
                Close();
            }
            else
            {
                logger.Info("The application will exit.");
                Application.Current.Shutdown();
            }

        }
        private void GameControllersClick(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenWindowsGameControllerSettings();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenSettings();
        }

        private void DiagnosticsClick(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenDiagnostics();
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            ViewModel.AboutPopupShow();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ViewModel.GetSettings().CloseToTray && !hardExit)
            {
                e.Cancel = true;
                restoreState = WindowState;
                Visibility = Visibility.Hidden;
                ShowInTaskbar = false;
                logger.Info("The application is closed to tray.");
            }
        }

        private async void WindowClosed(object sender, EventArgs e)
        {
            logger.Info("The application will exit.");
        }

        private void TaskbarIconTrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = restoreState;
            }
            else if (Visibility == Visibility.Hidden)
            {
                if (!IsLoaded)
                {
                    Show();
                }
                ShowInTaskbar = true;
                Visibility = Visibility.Visible;
            }
            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        public void ForceShow()
        {
            Dispatcher.Invoke(() => {
                TaskbarIconTrayMouseDoubleClick(this, null);
            });
        }

        private void OpenFlyout(object sender, RoutedEventArgs e)
        {
            ViewModel.Model.IsOpen = true;
        }
    }
}
