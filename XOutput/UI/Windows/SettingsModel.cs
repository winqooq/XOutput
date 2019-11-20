using System.Collections.ObjectModel;
using XOutput.Tools;

namespace XOutput.UI.Windows
{
    public class SettingsModel : ModelBase
    {
        public ObservableCollection<string> Languages { get; private set; }

        private string selectedLanguage;
        public string SelectedLanguage
        {
            get => selectedLanguage;
            set { Set(value, ref selectedLanguage, nameof(SelectedLanguage)); }
        }

        private bool closeToTray;
        public bool CloseToTray
        {
            get => closeToTray;
            set { Set(value, ref closeToTray, nameof(CloseToTray)); }
        }

        private bool hidGuardianEnabled;
        public bool HidGuardianEnabled
        {
            get => hidGuardianEnabled;
            set { Set(value, ref hidGuardianEnabled, nameof(HidGuardianEnabled)); }
        }

        private bool showAll;
        public bool ShowAll
        {
            get => showAll;
            set { Set(value, ref showAll, nameof(ShowAll)); }
        }

        private bool runAtStartup;
        public bool RunAtStartup
        {
            get => runAtStartup;
            set { Set(value, ref runAtStartup, nameof(RunAtStartup)); }
        }

        private bool changed;
        public bool Changed
        {
            get => changed;
            set { Set(value, ref changed, nameof(Changed)); }
        }

        [ResolverMethod(Scope.Prototype)]
        public SettingsModel()
        {
            Languages = new ObservableCollection<string>();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            Languages.Clear();
        }
    }
}
