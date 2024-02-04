using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;

namespace clock_scr
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class WpfColorPicker : UserControl
    {
        public static event EventHandler? ConfirmColor;
        public string Color
        {
            get
            {
                if (selectedColor.Background is SolidColorBrush solidColorBrush)
                {
                    return $"#{solidColorBrush.Color.A:X2}{solidColorBrush.Color.R:X2}{solidColorBrush.Color.G:X2}{solidColorBrush.Color.B:X2}";
                }
                return "#FF000000";
            }
            set
            {
                SolidColorBrush? brush = new BrushConverter().ConvertFrom(value) as SolidColorBrush;
                if (brush != null)
                {
                    selectedColor.Background = brush;
                    //gray
                    selectingColorGray.Background = brush;
                    sliderGrayScale.Value = brush.Color.R;
                    sliderAlpha.Value = brush.Color.A;
                    //rgb
                    selectingColorRGB.Background = brush;
                    sliderA.Value = brush.Color.A;
                    sliderR.Value = brush.Color.R;
                    sliderG.Value = brush.Color.G;
                    sliderB.Value = brush.Color.B;
                }
                else
                {
                    selectedColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));
                }
                
                ConfirmColor?.Invoke(this, EventArgs.Empty);
            }
        }

        public WpfColorPicker()
        {
            InitializeComponent();
        }
        void SliderGrayChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //gray
            double valueGray = sliderGrayScale.Value;
            double valueAlpha = sliderAlpha.Value;

            byte grayValue = (byte)valueGray;
            byte alphaValue = (byte)valueAlpha;
            Color color = System.Windows.Media.Color.FromArgb(alphaValue,grayValue, grayValue, grayValue);
            selectingColorGray.Background = new SolidColorBrush(color);
        }

        void SliderRGBChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //rgb
            double valueA = sliderA.Value;
            double valueR = sliderR.Value;
            double valueG = sliderG.Value;
            double valueB = sliderB.Value;

            byte aValue = (byte)valueA;
            byte rValue = (byte)valueR;
            byte gValue = (byte)valueG;
            byte bValue = (byte)valueB;
            Color color = System.Windows.Media.Color.FromArgb(aValue, rValue, gValue, bValue);
            selectingColorRGB.Background = new SolidColorBrush(color);
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
            var button = (Button)sender;
            var command = (string)button.CommandParameter;

            if(command == "gray")
            {
                selectedColor.Background = selectingColorGray.Background;
            }
            else
            {
                selectedColor.Background = selectingColorRGB.Background;
            }
            
            if (PART_ToggleButton != null)
            {
                PART_ToggleButton.IsChecked = false;
            }
            ConfirmColor?.Invoke(this, EventArgs.Empty);
        }
    }

}
