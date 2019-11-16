using System.Linq;
using XOutput.Tools;

namespace XOutput.UI.Windows
{
    public class SettingsViewModel : ResultViewModelBase<SettingsModel, SettingsResult>
    {
        private readonly LanguageManager languageManager;
        private readonly SettingsManager settingsManager;

        private GeneralSettings originalSettings;
        private bool confirmed;

        [ResolverMethod(Scope.Prototype)]
        public SettingsViewModel(LanguageManager languageManager, SettingsManager settingsManager, SettingsModel model) : base(model, new SettingsResult())
        {
            this.languageManager = languageManager;
            this.settingsManager = settingsManager;
            confirmed = false;

            var languages = languageManager.GetLanguages().ToList();
            languages.Sort();
            foreach (var language in languages)
            {
                Model.Languages.Add(language);
            }
            originalSettings = settingsManager.GeneralSettings;
            Model.SelectedLanguage = originalSettings.Language;
            Model.CloseToTray = originalSettings.CloseToTray;
            Model.HidGuardianEnabled = originalSettings.HidGuardianEnabled;
            Model.ShowAll = originalSettings.ShowAllDevices;
        }

        public void SetChanged()
        {
            var changed = !createSettingsFromModel().Equals(originalSettings);
            if (!confirmed && changed)
            {
                languageManager.Language = Model.SelectedLanguage;
            }
            Model.Changed = changed;
        }

        public void Close()
        {
            if (Model.Changed)
            {
                languageManager.Language = originalSettings.Language;
            }
            confirmed = true;
            Result.Changed = false;
        }

        public void Save()
        {
            if (Model.Changed)
            {
                languageManager.Language = Model.SelectedLanguage;
            }
            confirmed = true;
            Result.Changed = true;
            settingsManager.GeneralSettings = createSettingsFromModel();
            settingsManager.SaveGeneralSettings();
        }

        public bool NeedConfirmation()
        {
            if(confirmed)
            {
                return false;
            }
            if(Model.Changed)
            {
                return true;
            }
            return false;
        }

        private GeneralSettings createSettingsFromModel()
        {
            return new GeneralSettings
            {
                CloseToTray = Model.CloseToTray,
                HidGuardianEnabled = Model.HidGuardianEnabled,
                Language = Model.SelectedLanguage,
                RunAtStartup = Model.RunAtStartup,
                ShowAllDevices = Model.ShowAll,
            };
        }
    }
}
