using System.Collections.ObjectModel;
using XOutput.Tools;

namespace XOutput.UI.Windows
{
    public class SettingsModel : ModelBase
    {
        private readonly ObservableCollection<string> languages = new ObservableCollection<string>();
        public ObservableCollection<string> Languages => languages;

        private string selectedLanguage;
        public string SelectedLanguage
        {
            get => selectedLanguage;
            set { Set(value, selectedLanguage, (v) => selectedLanguage = v, nameof(SelectedLanguage)); }
        }

        private bool closeToTray;
        public bool CloseToTray
        {
            get => closeToTray;
            set { Set(value, closeToTray, (v) => closeToTray = v, nameof(CloseToTray)); }
        }

        private bool hidGuardianEnabled;
        public bool HidGuardianEnabled
        {
            get => hidGuardianEnabled;
            set { Set(value, hidGuardianEnabled, (v) => hidGuardianEnabled = v, nameof(HidGuardianEnabled)); }
        }

        private bool showAll;
        public bool ShowAll
        {
            get => showAll;
            set { Set(value, showAll, (v) => showAll = v, nameof(ShowAll)); }
        }

        private bool runAtStartup;
        public bool RunAtStartup
        {
            get => runAtStartup;
            set { Set(value, runAtStartup, (v) => runAtStartup = v, nameof(RunAtStartup)); }
        }

        private bool changed;
        public bool Changed
        {
            get => changed;
            set { Set(value, changed, (v) => changed = v, nameof(Changed)); }
        }

        [ResolverMethod(Scope.Prototype)]
        public SettingsModel()
        {

        }

        public override void CleanUp()
        {
            base.CleanUp();
            Languages.Clear();
        }
    }
}
