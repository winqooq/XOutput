using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using XOutput.Devices;
using XOutput.Tools;

namespace XOutput.UI.Component
{
    public partial class NotificationView : UserControl, IViewBase<NotificationViewModel, NotificationModel>
    {
        public event Action<NotificationView> CloseRequested;

        private DispatcherTimer timer;

        public NotificationViewModel ViewModel { get; private set; }

        [ResolverMethod(Scope.Prototype)]
        public NotificationView(NotificationViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        public void CleanUp()
        {
            if(timer != null && timer.IsEnabled)
            {
                timer.Tick -= Timer_Tick;
                timer.Stop();
            }
            ViewModel.CleanUp();
        }

        public void SetNotificationData(NotificationData data)
        {
            ViewModel.Model.Text = data.Text;
            ViewModel.Model.TextArguments = data.TextArguments;
            ViewModel.Model.Important = data.Important;
            if (data.Duration != TimeSpan.MaxValue)
            {
                timer = new DispatcherTimer();
                timer.Tick += Timer_Tick;
                timer.Interval = data.Duration;
                timer.Start();
                TimerGrid.BeginAnimation(WidthProperty, new DoubleAnimation(TimerGrid.Width, 0, new Duration(data.Duration)));
            } 
            else
            {
                TimerGrid.Visibility = Visibility.Hidden;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Tick -= Timer_Tick;
            timer.Stop();
            CloseRequested?.Invoke(this);
        }

        private void ExitClick(object sender, System.Windows.RoutedEventArgs e)
        {
            CloseRequested?.Invoke(this);
        }
    }
}
