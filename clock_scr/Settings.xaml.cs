using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static clock_scr.Settings;
using FontFamily = System.Windows.Media.FontFamily;

namespace clock_scr
{
    /// <summary>
    /// Settings.xaml の相互作用ロジック
    /// </summary>
    public class ExComboBox : ComboBox
    {
        public new string Tag
        {
            get {
                return SelectedItem != null && SelectedItem is ComboBoxItem
            ? ((ComboBoxItem)SelectedItem).Tag?.ToString() ?? Properties.Settings.Default.dateIndicationLanguage
            : Properties.Settings.Default.dateIndicationLanguage;
            }
        }
        public new string Text
        {
            get
            {
                return SelectedItem != null && SelectedItem is MyFont
                ? ((MyFont)SelectedItem).ToString()
                : Properties.Settings.Default.selectTimeFont;
            }
        }
    }
    public partial class Settings : Window
    {
        bool initialized = false;
        public Settings()
        {
            InitializeComponent();
            AppSettingsStore.Load();
            MainContentFrame.Content = new MainPage();
            cmbSelectTimeFont.ItemsSource = GetFontList();
            cmbSelectIndicationFont.ItemsSource = GetFontList();

            string currentLanguage = CultureInfo.CurrentCulture.Name;
            ComboBoxItem systemLanguageItem = new ComboBoxItem();
            systemLanguageItem.Content = "System Language (" + currentLanguage + ")";
            systemLanguageItem.Tag = currentLanguage;
            cmbDateIndicationLanguage.Items.Add(systemLanguageItem);


            sliderOffsetHM.Value = Properties.Settings.Default.offsetHM;
            sliderWatchSize.Value = Properties.Settings.Default.cameraDistance;
            sliderGradientBorder.Value = Properties.Settings.Default.gradientBorder;
            sliderWatchFontSize.Value = Properties.Settings.Default.watchFontSize;
            cmbTimeFormat.SelectedIndex = Properties.Settings.Default.timeFormat;
            sliderOffsetTF.Value = Properties.Settings.Default.offsetTF;
            cmbDateIndication.SelectedIndex = Properties.Settings.Default.dateIndication;
            selectDisplayColor.Color = Properties.Settings.Default.displayColor;
            selectBackFrameColor.Color = Properties.Settings.Default.backFrameColor;
            cmbHideCursor.SelectedIndex = Properties.Settings.Default.hideCursor;
            selectWindow.Value = Properties.Settings.Default.checkBit;
            exitSetting.Value = Properties.Settings.Default.exitBit;

            WpfColorPicker.ConfirmColor += ColorChanged;

            
            cmbSelectTimeFont.SelectedItem = cmbSelectTimeFont.Items.OfType<MyFont>().FirstOrDefault(f => f.LocalFontName.Contains(Properties.Settings.Default.selectTimeFont) || f.FontFamily.Source.Contains(Properties.Settings.Default.selectTimeFont));

            cmbSelectIndicationFont.SelectedItem = cmbSelectIndicationFont.Items.OfType<MyFont>().FirstOrDefault(f => f.LocalFontName.Contains(Properties.Settings.Default.selectIndicationFont) || f.FontFamily.Source.Contains(Properties.Settings.Default.selectIndicationFont));

            cmbDateIndicationLanguage.SelectedItem = cmbDateIndicationLanguage.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Tag?.ToString() == Properties.Settings.Default.dateIndicationLanguage)
                                           ?? cmbDateIndicationLanguage.Items[0];

            sliderGradientBorderDI.Value = Properties.Settings.Default.gradientBorderDI;
            sliderOffsetDI.Value = Properties.Settings.Default.offsetDI;
            initialized = true;
        }

        public class MyFont
        {
            public FontFamily FontFamily { get; set; } = default!;
            public string LocalFontName { get; set; } = string.Empty;

            public override string ToString()
            {
                return LocalFontName;
            }
        }

        private MyFont[] GetFontList()
        {
            this.Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
            var fonts = Fonts.SystemFontFamilies.Select(i => new MyFont() { FontFamily = i, LocalFontName = i.Source }).ToArray();
            fonts.Select(i => i.LocalFontName = i.FontFamily.FamilyNames
                .FirstOrDefault(j => j.Key == this.Language).Value ?? i.FontFamily.Source).ToArray();

            return fonts;
        }

        void Sendsettings()
        {
            if (initialized)
            {
                var mainPage = MainContentFrame.Content as MainPage;
                if (mainPage != null)
                {
                    mainPage.SetSettings(sliderOffsetHM.Value, sliderWatchSize.Value, sliderGradientBorder.Value, sliderWatchFontSize.Value, cmbTimeFormat.SelectedIndex, sliderOffsetTF.Value, cmbDateIndication.SelectedIndex, selectDisplayColor.Color, selectBackFrameColor.Color, cmbSelectTimeFont.Text, cmbDateIndicationLanguage.Tag, cmbSelectIndicationFont.Text, sliderGradientBorderDI.Value,sliderOffsetDI.Value);
                    mainPage.InitializeWatch();
                }
            }
        }

        void SliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Sendsettings();
        }

        private void ComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            Sendsettings();
        }

        private void ColorChanged(object? sender, EventArgs e)
        {
            Sendsettings();
        }


        void SaveAngle_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = MainContentFrame.Content as MainPage;
            if (mainPage != null)
            {
                Properties.Settings.Default.rotateAngel = mainPage.GetMatrix();
                AppSettingsStore.Save();
            }
        }

        void ResetAngle_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = MainContentFrame.Content as MainPage;
            if (mainPage != null)
            {
                mainPage.SetMatrix((string)Properties.Settings.Default.Properties["rotateAngel"].DefaultValue);
            }
        }


        void Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.offsetHM = sliderOffsetHM.Value;
            Properties.Settings.Default.cameraDistance = sliderWatchSize.Value;
            Properties.Settings.Default.gradientBorder = sliderGradientBorder.Value;
            Properties.Settings.Default.watchFontSize = sliderWatchFontSize.Value;
            Properties.Settings.Default.timeFormat = cmbTimeFormat.SelectedIndex;
            Properties.Settings.Default.offsetTF = sliderOffsetTF.Value;
            Properties.Settings.Default.dateIndication = cmbDateIndication.SelectedIndex;
            Properties.Settings.Default.displayColor = selectDisplayColor.Color;
            Properties.Settings.Default.backFrameColor = selectBackFrameColor.Color;
            Properties.Settings.Default.checkBit = selectWindow.Value;
            Properties.Settings.Default.exitBit = exitSetting.Value;
            Properties.Settings.Default.selectTimeFont = cmbSelectTimeFont.Text;
            Properties.Settings.Default.dateIndicationLanguage = cmbDateIndicationLanguage.Tag;
            Properties.Settings.Default.selectIndicationFont = cmbSelectIndicationFont.Text;
            Properties.Settings.Default.gradientBorderDI = sliderGradientBorderDI.Value;
            Properties.Settings.Default.offsetDI = sliderOffsetDI.Value;
            Properties.Settings.Default.hideCursor = cmbHideCursor.SelectedIndex;
            AppSettingsStore.Save();
        }

        void Reset_Click(object sender, RoutedEventArgs e)
        {
            sliderOffsetHM.Value = Convert.ToDouble(Properties.Settings.Default.Properties["OffsetHM"].DefaultValue);
            sliderWatchSize.Value = Convert.ToDouble(Properties.Settings.Default.Properties["cameraDistance"].DefaultValue);
            sliderGradientBorder.Value = Convert.ToDouble(Properties.Settings.Default.Properties["gradientBorder"].DefaultValue);
            sliderWatchFontSize.Value = Properties.Settings.Default.watchFontSize;
            cmbTimeFormat.SelectedIndex = Convert.ToInt16(Properties.Settings.Default.Properties["timeFormat"].DefaultValue);
            sliderOffsetTF.Value = Convert.ToDouble(Properties.Settings.Default.Properties["offsetTF"].DefaultValue);
            cmbDateIndication.SelectedIndex = Convert.ToInt16(Properties.Settings.Default.Properties["dateIndication"].DefaultValue);
            selectDisplayColor.Color = (string)Properties.Settings.Default.Properties["displayColor"].DefaultValue;
            selectBackFrameColor.Color = (string)Properties.Settings.Default.Properties["backFrameColor"].DefaultValue;
            selectWindow.Value = Convert.ToInt16(Properties.Settings.Default.Properties["checkBit"].DefaultValue);
            exitSetting.Value = Convert.ToInt16(Properties.Settings.Default.Properties["exitBit"].DefaultValue);
            cmbSelectTimeFont.SelectedItem = cmbSelectTimeFont.Items.OfType<MyFont>().FirstOrDefault(f => f.LocalFontName.Contains((string)Properties.Settings.Default.Properties["selectTimeFont"].DefaultValue) || f.FontFamily.Source.Contains((string)Properties.Settings.Default.Properties["selectTimeFont"].DefaultValue));
            cmbDateIndicationLanguage.SelectedIndex = 0;
            cmbSelectIndicationFont.SelectedItem = cmbSelectIndicationFont.Items.OfType<MyFont>().FirstOrDefault(f => f.LocalFontName.Contains((string)Properties.Settings.Default.Properties["selectIndicationFont"].DefaultValue) || f.FontFamily.Source.Contains((string)Properties.Settings.Default.Properties["selectIndicationFont"].DefaultValue));
            sliderGradientBorderDI.Value = Convert.ToDouble(Properties.Settings.Default.Properties["gradientBorderDI"].DefaultValue);
            sliderOffsetDI.Value = Convert.ToDouble(Properties.Settings.Default.Properties["OffsetDI"].DefaultValue);
        }
    }
}
