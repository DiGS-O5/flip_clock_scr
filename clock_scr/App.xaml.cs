using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace clock_scr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length > 0)
            {
                string mode = e.Args[0].ToLower(CultureInfo.InvariantCulture);

                if (mode.StartsWith("/c"))
                {
                    Settings settingsWindow = new Settings();

                    settingsWindow.Show();
                    return;
                }
                else if (mode.StartsWith("/p"))
                {
                    Application.Current.Shutdown();
                    return;
                }
            }

            // 通常のスタートアップロジック
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
            int screenNumber = 0;
            foreach (System.Windows.Forms.Screen scr in System.Windows.Forms.Screen.AllScreens)
            {
                MainWindow screensaver = new MainWindow(screenNumber);
                screensaver.Topmost = true;
                screensaver.Left = scr.Bounds.Left;
                screensaver.Top = scr.Bounds.Top;
                screensaver.Show();
                screensaver.WindowStyle = WindowStyle.None;
                screensaver.WindowState = System.Windows.WindowState.Maximized;
                screensaver.ResizeMode = ResizeMode.NoResize;
                screenNumber++;
            }


        }
    }
}
