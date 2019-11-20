using System.Collections.ObjectModel;
using System.Windows.Controls;
using XOutput.Tools;
using XOutput.UI.Component;

namespace XOutput.UI.Windows
{
    public class MainWindowModel : ModelBase
    {
        public ObservableCollection<NotificationView> Notifications { get; private set; }
        public ObservableCollection<InputView> Inputs { get; private set; }
        public ObservableCollection<ControllerView> Controllers { get; private set; }

        private bool isOpen;

        public bool IsOpen
        {
            get => isOpen;
            set { Set(value, ref isOpen, nameof(IsOpen)); }
        }

        private int notificationCount;

        public int NotificationCount
        {
            get => notificationCount;
            set { Set(value, ref notificationCount, nameof(NotificationCount)); }
        }



        [ResolverMethod(Scope.Prototype)]
        public MainWindowModel()
        {
            Notifications = new ObservableCollection<NotificationView>();
            Inputs = new ObservableCollection<InputView>();
            Controllers = new ObservableCollection<ControllerView>();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            Notifications.ClearView();
            Inputs.ClearView();
            Controllers.ClearView();
        }
    }
}
