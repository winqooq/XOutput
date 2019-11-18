using Microsoft.Win32;
using System.Diagnostics;
using XOutput.Logging;

namespace XOutput.Tools
{
    public sealed class RegistryModifier : IRegistryModifierService
    {
        /// <summary>
        /// Startup registry key.
        /// </summary>
        public static readonly string AutostartRegistry = Registry.CurrentUser.ToString() + @"Software\Microsoft\Windows\CurrentVersion\Run";
        /// <summary>
        /// XOutput registry value
        /// </summary>
        public const string AutostartValueKey = "XOutput";
        /// <summary>
        /// Autostart command line parameters.
        /// </summary>
        public const string AutostartParams = " --minimized";

        private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(RegistryModifier));

        private readonly RegistryModifierService registryModifierService;

        [ResolverMethod]
        public RegistryModifier(RegistryModifierService registryModifierService)
        {
            this.registryModifierService = registryModifierService;
        }

        /// <summary>
        /// Activates autostart.
        /// </summary>
        public void SetAutostart()
        {
            var filename = Process.GetCurrentProcess().MainModule.FileName;
            string value = $"\"{filename}\" {AutostartParams}";
            SetValue(AutostartRegistry, AutostartValueKey, value);
        }

        /// <summary>
        /// Deactivates autostart.
        /// </summary>
        public void ClearAutostart()
        {
            DeleteValue(AutostartRegistry, AutostartValueKey);
        }

        public bool KeyExists(string key)
        {
            return registryModifierService.KeyExists(key);
        }

        public void DeleteTree(string key)
        {
            registryModifierService.DeleteTree(key);
        }

        public void CreateKey(string key)
        {
            registryModifierService.CreateKey(key);
        }

        public object GetValue(string key, string value)
        {
            return registryModifierService.GetValue(key, value);
        }

        public void SetValue(string key, string value, object newValue)
        {
            registryModifierService.SetValue(key, value, newValue);
        }

        public void DeleteValue(string key, string value)
        {
            throw new System.NotImplementedException();
        }
    }
}
