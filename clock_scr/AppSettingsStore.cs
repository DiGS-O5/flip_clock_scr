using System;
using System.IO;
using System.Text.Json;

namespace clock_scr
{
    public static class AppSettingsStore
    {
        private static readonly string BaseDirectory =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DiGS-O5",
                "Flip_Clock");

        private static readonly string SettingsFilePath =
            Path.Combine(BaseDirectory, "settings.json");

        public static void Save()
        {
            Directory.CreateDirectory(BaseDirectory);

            string json = JsonSerializer.Serialize(Properties.Settings.Default, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(SettingsFilePath, json);
        }

        public static void Load()
        {
            if (!File.Exists(SettingsFilePath))
            {
                return;
            }

            string json = File.ReadAllText(SettingsFilePath);
            var loaded = JsonSerializer.Deserialize<SettingsData>(json);

            if (loaded == null)
            {
                return;
            }

            Properties.Settings.Default.cameraDistance = loaded.cameraDistance;
            Properties.Settings.Default.offsetHM = loaded.offsetHM;
            Properties.Settings.Default.gradientBorder = loaded.gradientBorder;
            Properties.Settings.Default.watchFontSize = loaded.watchFontSize;
            Properties.Settings.Default.timeFormat = loaded.timeFormat;
            Properties.Settings.Default.offsetTF = loaded.offsetTF;
            Properties.Settings.Default.dateIndication = loaded.dateIndication;
            Properties.Settings.Default.displayColor = loaded.displayColor ?? Properties.Settings.Default.displayColor;
            Properties.Settings.Default.backFrameColor = loaded.backFrameColor ?? Properties.Settings.Default.backFrameColor;
            Properties.Settings.Default.selectTimeFont = loaded.selectTimeFont ?? Properties.Settings.Default.selectTimeFont;
            Properties.Settings.Default.dateIndicationLanguage = loaded.dateIndicationLanguage ?? Properties.Settings.Default.dateIndicationLanguage;
            Properties.Settings.Default.selectIndicationFont = loaded.selectIndicationFont ?? Properties.Settings.Default.selectIndicationFont;
            Properties.Settings.Default.offsetDI = loaded.offsetDI;
            Properties.Settings.Default.gradientBorderDI = loaded.gradientBorderDI;
            Properties.Settings.Default.checkBit = loaded.checkBit;
            Properties.Settings.Default.exitBit = loaded.exitBit;
            Properties.Settings.Default.rotateAngel = loaded.rotateAngel ?? Properties.Settings.Default.rotateAngel;
            Properties.Settings.Default.hideCursor = loaded.hideCursor;
        }

        private sealed class SettingsData
        {
            public double cameraDistance { get; set; }
            public double offsetHM { get; set; }
            public double gradientBorder { get; set; }
            public double watchFontSize { get; set; }
            public int timeFormat { get; set; }
            public double offsetTF { get; set; }
            public int dateIndication { get; set; }
            public string? displayColor { get; set; }
            public string? backFrameColor { get; set; }
            public string? selectTimeFont { get; set; }
            public string? dateIndicationLanguage { get; set; }
            public string? selectIndicationFont { get; set; }
            public double offsetDI { get; set; }
            public double gradientBorderDI { get; set; }
            public int checkBit { get; set; }
            public int exitBit { get; set; }
            public string? rotateAngel { get; set; }
            public int hideCursor { get; set; }
        }
    }
}