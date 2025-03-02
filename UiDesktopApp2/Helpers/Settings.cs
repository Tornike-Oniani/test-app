using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.Helpers
{
    public class Settings
    {
        public double ImageTime { get; set; } = 5;
        public double TransitionImageDuration { get; set; } = 1;
        public bool IsTimerVisible { get; set; } = false;

        public Settings()
        {
            // 1. If settings file doesn't exist create it
            string filePath = Path.Combine(Environment.CurrentDirectory, "settings.json");
            
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            else
            {
                // 2. If settings already existed read it and set its values
                JsonWriterReader<SettingsData> _jsonReaderWriter = new JsonWriterReader<SettingsData>(filePath);
                SettingsData savedSettings = _jsonReaderWriter.ReadData();

                if (savedSettings != null)
                {
                    ImageTime = savedSettings.ImageTime;
                    TransitionImageDuration = savedSettings.TransitionImageDuration;
                    IsTimerVisible = savedSettings.IsTimerVisible;
                }
            }
        }
    }
}
