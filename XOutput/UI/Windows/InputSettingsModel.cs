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
            set { Set(value, title, v => title = v, nameof(Title)); }
        }

        private string forceFeedbackText = "";
        public string ForceFeedbackText
        {
            get => forceFeedbackText;
            set { Set(value, forceFeedbackText, v => forceFeedbackText = v, nameof(ForceFeedbackText)); }
        }

        private string testButtonText = "";
        public string TestButtonText
        {
            get => testButtonText;
            set { Set(value, testButtonText, v => testButtonText = v, nameof(TestButtonText)); }
        }

        private bool forceFeedbackEnabled;
        public bool ForceFeedbackEnabled
        {
            get => forceFeedbackEnabled;
            set { Set(value, forceFeedbackEnabled, v => forceFeedbackEnabled = v, nameof(ForceFeedbackEnabled)); }
        }

        private bool forceFeedbackAvailable;
        public bool ForceFeedbackAvailable
        {
            get => forceFeedbackAvailable;
            set { Set(value, forceFeedbackAvailable, v => forceFeedbackAvailable = v, nameof(ForceFeedbackAvailable)); }
        }

        private bool isAdmin;
        public bool IsAdmin
        {
            get => isAdmin;
            set { Set(value, isAdmin, v => isAdmin = v, nameof(IsAdmin)); }
        }

        private bool hidGuardianAdded;
        public bool HidGuardianAdded
        {
            get => hidGuardianAdded;
            set { Set(value, hidGuardianAdded, v => hidGuardianAdded = v, nameof(HidGuardianAdded)); }
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
            InputAxisViews.Clear();
            InputSliderViews.Clear();
            InputDPadViews.Clear();
            InputButtonViews.Clear();
        }

        public void RefreshVisiblity()
        {
            OnPropertyChanged(nameof(InputAxisViews), nameof(InputSliderViews), nameof(InputDPadViews), nameof(InputButtonViews));
        }
    }
}
