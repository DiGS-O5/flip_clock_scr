using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace clock_scr
{
    /// <summary>
    /// wpfExitSetting.xaml の相互作用ロジック
    /// </summary>
    public partial class WpfExitSetting : UserControl
    {
        private int bitValue = 0;

        public int Value
        {
            get
            {
                CheckingBox();
                return bitValue;
            }
            set
            {
                int index = 0;
                foreach (var item in listStackPanel.Children)
                {
                    if (item is CheckBox checkBox)
                    {
                        bool isChecked = (value & (1 << index)) != 0;
                        checkBox.IsChecked = isChecked;
                        index++;
                    }
                }

            }
        }
        public WpfExitSetting()
        {
            InitializeComponent();
        }

        private void CheckingBox()
        {
            bitValue = 0;
            int index = 0;
            foreach (var item in listStackPanel.Children)
            {
                if (item is CheckBox checkBox)
                {
                    if (checkBox.IsChecked ?? false)
                    {
                        bitValue |= (1 << index);
                    }
                    index++;
                }
            }
        }

        private async void Popup_Closed(object sender, EventArgs e)
        {
            PART_ToggleButton.IsEnabled = false;
            await Task.Delay(10);
            PART_ToggleButton.IsChecked = false;
            await Task.Delay(10);
            PART_ToggleButton.IsEnabled = true;
        }
        void Confirm_Click(object sender, RoutedEventArgs e)
        {
            CheckingBox();
            Debug.WriteLine(bitValue);
            if (PART_ToggleButton != null)
            {
                PART_ToggleButton.IsChecked = false;
            }
        }
    }
}
