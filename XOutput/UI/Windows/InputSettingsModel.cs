using System.Collections.ObjectModel;
using XOutput.Tools;
using XOutput.UI.Component;

namespace XOutput.UI.Windows
{
    public class InputSettingsModel : ModelBase
    {
        public ObservableCollection<IUpdatableView> InputAxisViews { get; private set; }
        public ObservableCollection<IUpdatableView> InputSliderViews { get; private set; }
        public ObservableCollection<IUpdatableView> InputDPadViews { get; private set; }
        public ObservableCollection<IUpdatableView> InputButtonViews { get; private set; }


        private string title = "";
        public string Title
        {
            get => title;
            set { Set(value, ref title, nameof(Title)); }
        }

        private string forceFeedbackText = "";
        public string ForceFeedbackText
        {
            get => forceFeedbackText;
            set { Set(value, ref forceFeedbackText, nameof(ForceFeedbackText)); }
        }

        private string testButtonText = "";
        public string TestButtonText
        {
            get => testButtonText;
            set { Set(value, ref testButtonText, nameof(TestButtonText)); }
        }

        private bool forceFeedbackEnabled;
        public bool ForceFeedbackEnabled
        {
            get => forceFeedbackEnabled;
            set { Set(value, ref forceFeedbackEnabled, nameof(ForceFeedbackEnabled)); }
        }

        private bool forceFeedbackAvailable;
        public bool ForceFeedbackAvailable
        {
            get => forceFeedbackAvailable;
            set { Set(value, ref forceFeedbackAvailable, nameof(ForceFeedbackAvailable)); }
        }

        private bool hidGuardianAvailable;
        public bool HidGuardianAvailable
        {
            get => hidGuardianAvailable;
            set { Set(value, ref hidGuardianAvailable, nameof(HidGuardianAvailable)); }
        }

        private bool hidGuardianAdded;
        public bool HidGuardianAdded
        {
            get => hidGuardianAdded;
            set { Set(value, ref hidGuardianAdded, nameof(HidGuardianAdded)); }
        }

        [ResolverMethod(Scope.Prototype)]
        public InputSettingsModel()
        {
            InputAxisViews = new ObservableCollection<IUpdatableView>();
            InputSliderViews= new ObservableCollection<IUpdatableView>();
            InputDPadViews = new ObservableCollection<IUpdatableView>();
            InputButtonViews = new ObservableCollection<IUpdatableView>();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            InputAxisViews.ClearView();
            InputSliderViews.ClearView();
            InputDPadViews.ClearView();
            InputButtonViews.ClearView();
        }

        public void RefreshVisiblity()
        {
            OnPropertyChanged(nameof(InputAxisViews), nameof(InputSliderViews), nameof(InputDPadViews), nameof(InputButtonViews));
        }
    }
}
