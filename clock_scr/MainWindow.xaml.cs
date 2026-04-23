using clock_scr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace clock_scr
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        private Point? initialMousePosition;

        public MainWindow(int screenNumber)
        {
            InitializeComponent();
            AppSettingsStore.Load();

            if (Properties.Settings.Default.hideCursor == 1)
            {
                Cursor = Cursors.None;
            }

            int windowBit = Properties.Settings.Default.checkBit;
            if ((windowBit & (1 << screenNumber)) != 0)
            {
                MainContentFrame.Content = new MainPage();
            }
            else
            {
                Page blackPage = new Page();
                blackPage.Background = new SolidColorBrush(Colors.Black);
                MainContentFrame.Content = blackPage;
            }
            int exitBit = Properties.Settings.Default.exitBit;
            if ((exitBit & 1) != 0)
            {
                PreviewKeyDown += MainWindow_PreviewKeyDown;
            }
            if ((exitBit & 2) != 0)
            {
                MouseMove += Window_MouseMove;
            }
            if ((exitBit & 4) != 0)
            {
                MouseDoubleClick += Window_MouseDoubleClick;
            }

        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Application.Current.Shutdown();
        }

        void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePosition = e.MouseDevice.GetPosition(null);

            if (initialMousePosition.HasValue)
            {
                if (Math.Abs(initialMousePosition.Value.X - currentMousePosition.X) > 0 ||
                    Math.Abs(initialMousePosition.Value.Y - currentMousePosition.Y) > 0)
                {
                    Application.Current.Shutdown();
                }
            }
            else
            {
                initialMousePosition = currentMousePosition;
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}