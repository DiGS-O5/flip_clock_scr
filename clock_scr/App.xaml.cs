using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace clock_scr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private HwndSource? previewSource;
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
                    if (e.Args.Length >= 2 && long.TryParse(e.Args[1], out long handleValue))
                    {
                        ShowPreview(new IntPtr(handleValue));
                        return;
                    }

                    Shutdown();
                    return;
                }
                else if (mode.StartsWith("/s"))
                {
                    ShowScreensaver();
                    return;
                }
            }

            ShowScreensaver();
        }

        private void ShowScreensaver()
        {
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

        private void ShowPreview(IntPtr parentHandle)
        {
            AppSettingsStore.Load();

            if (!NativeMethods.GetClientRect(parentHandle, out NativeMethods.RECT rect))
            {
                Shutdown();
                return;
            }

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            if (width <= 0 || height <= 0)
            {
                Shutdown();
                return;
            }

            HwndSourceParameters parameters = new HwndSourceParameters("ScreenSaverPreview");
            parameters.ParentWindow = parentHandle;
            parameters.WindowStyle =
                NativeMethods.WS_VISIBLE |
                NativeMethods.WS_CHILD |
                NativeMethods.WS_CLIPCHILDREN |
                NativeMethods.WS_CLIPSIBLINGS;
            parameters.SetPosition(0, 0);
            parameters.Width = width;
            parameters.Height = height;

            previewSource = new HwndSource(parameters);
            previewSource.RootVisual = new MainPage();

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }
    }
}
