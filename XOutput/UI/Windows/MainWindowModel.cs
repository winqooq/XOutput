using System.Collections.ObjectModel;
using System.Windows.Controls;
using XOutput.Tools;
using XOutput.UI.Component;

namespace XOutput.UI.Windows
{
    public class MainWindowModel : ModelBase
    {
        public ObservableCollection<Label> Events { get; private set; }
        public ObservableCollection<InputView> Inputs { get; private set; }
        public ObservableCollection<ControllerView> Controllers { get; private set; }

        private bool isOpen;

        public bool IsOpen
        {
            get => isOpen;
            set { Set(value, isOpen, v => isOpen = v, nameof(IsOpen)); }
        }

        private int notificationCount;

        public int NotificationCount
        {
            get => notificationCount;
            set { Set(value, notificationCount, v => notificationCount = v, nameof(NotificationCount)); }
        }



        [ResolverMethod(Scope.Prototype)]
        public MainWindowModel()
        {
            Events = new ObservableCollection<Label>();
            Inputs = new ObservableCollection<InputView>();
            Controllers = new ObservableCollection<ControllerView>();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            Events.Clear();
            Inputs.Clear();
            Controllers.Clear();
        }
    }
}
