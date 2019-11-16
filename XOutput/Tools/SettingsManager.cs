using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XOutput.Devices.Input;
using XOutput.Devices.Mapper;
using XOutput.Devices.XInput;
using XOutput.Logging;

namespace XOutput.Tools
{
    public class SettingsManager
    {
        private const string SETTINGS_DIRECTORY = "config";
        private const string INPUT_DIRECTORY = SETTINGS_DIRECTORY + "\\" + "input";
        private const string CONTROLLER_DIRECTORY = SETTINGS_DIRECTORY + "\\" + "controller";
        private const string EXTENSION = ".json";
        private const string GENERAL_SETTINGS = "general" + EXTENSION;

        [ResolverMethod]
        public SettingsManager()
        {

        }

        public GeneralSettings GeneralSettings { get; set; }

        public GeneralSettings LoadGeneralSettings()
        {
            GeneralSettings = Load(() => new GeneralSettings(), SETTINGS_DIRECTORY, GENERAL_SETTINGS);
            return GeneralSettings;
        }

        public void SaveGeneralSettings()
        {
            Save(GeneralSettings, SETTINGS_DIRECTORY, GENERAL_SETTINGS);
        }

        public IEnumerable<string> ListInputConfiIds()
        {
            return List(INPUT_DIRECTORY);
        }

        public InputConfig LoadInputConfig(string id)
        {
            return Load(() => new InputConfig(id), INPUT_DIRECTORY, id + EXTENSION);
        }

        public void SaveGeneralSettings(InputConfig settings)
        {
            Save(settings, INPUT_DIRECTORY, settings.Id + EXTENSION);
        }

        public IEnumerable<string> ListMappingSettingsIds()
        {
            return List(CONTROLLER_DIRECTORY);
        }

        public InputMapper LoadMappingConfig(string id)
        {
            return Load(() => new InputMapper(id), CONTROLLER_DIRECTORY, id + EXTENSION);
        }

        public void SaveMappingSettings(InputMapper settings)
        {
            Save(settings, CONTROLLER_DIRECTORY, settings.Id + EXTENSION);
        }

        public void DeleteMappingSettings(InputMapper settings)
        {
            Delete(CONTROLLER_DIRECTORY, settings.Id + EXTENSION);
        }

        private string[] List(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return new string[0];
            }
            return Directory.GetFiles(directory, "*" + EXTENSION);
        }

        private T Load<T>(Func<T> defaultGenerator, string directory, string file)
        {
            string filePath = directory + "\\" + file;
            if (File.Exists(filePath))
            {
                var text = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(text);
            }
            return defaultGenerator();
        }

        private void Save(object setting, string directory, string file)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filePath = directory + "\\" + file;
            File.WriteAllText(filePath, JsonConvert.SerializeObject(setting, Formatting.Indented));
        }

        private void Delete(string directory, string file)
        {
            string filePath = directory + "\\" + file;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
