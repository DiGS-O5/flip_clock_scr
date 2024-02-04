using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace clock_scr
{
    /// <summary>
    /// Settings.xaml の相互作用ロジック
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            MainContentFrame.Content = new MainPage();

            sliderOffsetHM.Value = Properties.Settings.Default.offsetHM;
            sliderWatchSize.Value = Properties.Settings.Default.watchSize;
            sliderGradientBorder.Value = Properties.Settings.Default.gradientBorder;
            cmbTimeFormat.SelectedIndex = Properties.Settings.Default.timeFormat;
            sliderOffsetTF.Value = Properties.Settings.Default.offsetTF;
            cmbDateIndication.SelectedIndex = Properties.Settings.Default.dateIndication;
            selectDisplayColor.Color = Properties.Settings.Default.displayColor;
            selectBackFrameColor.Color = Properties.Settings.Default.backFrameColor;
            selectWindow.Value = Properties.Settings.Default.checkBit;
            exitSetting.Value = Properties.Settings.Default.exitBit;

            WpfColorPicker.ConfirmColor += ColorChanged;
        }

        void Sendsettings()
        {
            var mainPage = MainContentFrame.Content as MainPage;
            if (mainPage != null)
            {
                mainPage.SetSettings(sliderOffsetHM.Value, sliderWatchSize.Value, sliderGradientBorder.Value, cmbTimeFormat.SelectedIndex, sliderOffsetTF.Value, cmbDateIndication.SelectedIndex, selectDisplayColor.Color, selectBackFrameColor.Color);
                mainPage.InitializeWatch();
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

        void Save_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = MainContentFrame.Content as MainPage;
            if (mainPage != null)
            {
                Properties.Settings.Default.rotateAngel = mainPage.GetMatrix();
            }
            Properties.Settings.Default.offsetHM = sliderOffsetHM.Value;
            Properties.Settings.Default.watchSize = sliderWatchSize.Value;
            Properties.Settings.Default.gradientBorder = sliderGradientBorder.Value;
            Properties.Settings.Default.timeFormat = cmbTimeFormat.SelectedIndex;
            Properties.Settings.Default.offsetTF = sliderOffsetTF.Value;
            Properties.Settings.Default.displayColor = selectDisplayColor.Color;
            Properties.Settings.Default.backFrameColor = selectBackFrameColor.Color;
            Properties.Settings.Default.checkBit = selectWindow.Value;
            Properties.Settings.Default.exitBit = exitSetting.Value;
            Properties.Settings.Default.Save();
        }

        void Reset_Click(object sender, RoutedEventArgs e)
        {
            sliderOffsetHM.Value = Convert.ToDouble(Properties.Settings.Default.Properties["OffsetHM"].DefaultValue);
            sliderWatchSize.Value = Convert.ToDouble(Properties.Settings.Default.Properties["watchSize"].DefaultValue);
            sliderGradientBorder.Value = Convert.ToDouble(Properties.Settings.Default.Properties["gradientBorder"].DefaultValue);
            cmbTimeFormat.SelectedIndex = Convert.ToInt16(Properties.Settings.Default.Properties["timeFormat"].DefaultValue);
            sliderOffsetTF.Value = Convert.ToDouble(Properties.Settings.Default.Properties["offsetTF"].DefaultValue);
            selectDisplayColor.Color = (string)Properties.Settings.Default.Properties["displayColor"].DefaultValue;
            selectBackFrameColor.Color = (string)Properties.Settings.Default.Properties["backFrameColor"].DefaultValue;
            selectWindow.Value = Convert.ToInt16(Properties.Settings.Default.Properties["checkBit"].DefaultValue);
            exitSetting.Value = Convert.ToInt16(Properties.Settings.Default.Properties["exitBit"].DefaultValue);
            var mainPage = MainContentFrame.Content as MainPage;
            if (mainPage != null)
            {
                mainPage.SetMatrix((string)Properties.Settings.Default.Properties["rotateAngel"].DefaultValue);
            }
        }
    }
}
