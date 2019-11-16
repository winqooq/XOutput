using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XOutput.Devices.Input;
using XOutput.Devices.Mapper;
using XOutput.Devices.XInput;
using XOutput.Logging;

namespace XOutput.Tools
{
    /// <summary>
    /// Contains the settings that are persisted.
    /// </summary>
    public sealed class GeneralSettings
    {
        public bool CloseToTray { get; set; }
        public bool ShowAllDevices { get; set; }
        public bool HidGuardianEnabled { get; set; }
        public string Language { get; set; }
        public bool RunAtStartup { get; set; }

        public GeneralSettings()
        {
            CloseToTray = false;
            ShowAllDevices = false;
            HidGuardianEnabled = false;
            RunAtStartup = false;
            Language = "English";
        }

        public override int GetHashCode()
        {
            var hashCode = -1595055210;
            hashCode = hashCode * -1521134295 + CloseToTray.GetHashCode();
            hashCode = hashCode * -1521134295 + ShowAllDevices.GetHashCode();
            hashCode = hashCode * -1521134295 + HidGuardianEnabled.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Language);
            hashCode = hashCode * -1521134295 + RunAtStartup.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is GeneralSettings settings &&
                   CloseToTray == settings.CloseToTray &&
                   ShowAllDevices == settings.ShowAllDevices &&
                   HidGuardianEnabled == settings.HidGuardianEnabled &&
                   Language == settings.Language &&
                   RunAtStartup == settings.RunAtStartup;
        }
    }
}
