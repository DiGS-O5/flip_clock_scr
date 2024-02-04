using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace clock_scr
{
    /// <summary>
    /// WpfSelectWindowxaml.xaml の相互作用ロジック
    /// </summary>
    public partial class WpfSelectWindow : UserControl
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
        public WpfSelectWindow()
        {
            InitializeComponent();
            CreateDynamicCheckboxes();
        }

        private void CreateDynamicCheckboxes()
        {
            var screens = System.Windows.Forms.Screen.AllScreens;
            int checkboxCount = screens.Length;

            for (int i = 0; i < checkboxCount; i++)
            {
                CheckBox checkBox = new()
                {
                    Content = $"Monitor {i + 1}",
                    Margin = new Thickness(5),
                    Tag = i
                };
                listStackPanel.Children.Add(checkBox);
            }
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
            if (PART_ToggleButton != null)
            {
                PART_ToggleButton.IsChecked = false;
            }
        }
    }
}
