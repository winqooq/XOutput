using System.Collections.ObjectModel;
using System.Windows.Controls;
using XOutput.Tools;
using XOutput.UI.Component;

namespace XOutput.UI.Windows
{
    public class ControllerSettingsModel : ModelBase
    {
        public ObservableCollection<MappingView> MapperAxisViews { get; private set; }
        public ObservableCollection<MappingView> MapperDPadViews { get; private set; }
        public ObservableCollection<MappingView> MapperButtonViews { get; private set; }

        public ObservableCollection<IUpdatableView> XInputAxisViews { get; private set; }
        public ObservableCollection<IUpdatableView> XInputDPadViews { get; private set; }
        public ObservableCollection<IUpdatableView> XInputButtonViews { get; private set; }

        public ObservableCollection<ComboBoxItem> ForceFeedbacks { get; private set; }

        private ComboBoxItem forceFeedback;
        public ComboBoxItem ForceFeedback
        {
            get => forceFeedback;
            set { Set(value, ref forceFeedback, nameof(ForceFeedback)); }
        }

        private string title = "";
        public string Title
        {
            get => title;
            set { Set(value, ref title, nameof(Title)); }
        }

        private bool startWhenConnected;
        public bool StartWhenConnected
        {
            get => startWhenConnected;
            set { Set(value, ref startWhenConnected, nameof(StartWhenConnected)); }
        }

        [ResolverMethod(Scope.Prototype)]
        public ControllerSettingsModel()
        {
            XInputAxisViews = new ObservableCollection<IUpdatableView>();
            XInputButtonViews = new ObservableCollection<IUpdatableView>();
            XInputDPadViews = new ObservableCollection<IUpdatableView>();
            MapperAxisViews = new ObservableCollection<MappingView>();
            MapperButtonViews = new ObservableCollection<MappingView>();
            MapperDPadViews = new ObservableCollection<MappingView>();
            ForceFeedbacks = new ObservableCollection<ComboBoxItem>();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            XInputAxisViews.ClearView();
            XInputButtonViews.ClearView();
            XInputDPadViews.ClearView();
            MapperAxisViews.ClearView();
            MapperButtonViews.ClearView();
            MapperDPadViews.ClearView();
            ForceFeedbacks.Clear();
        }
    }
}
