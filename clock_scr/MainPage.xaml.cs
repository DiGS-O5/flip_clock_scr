using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;
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
using static System.Net.Mime.MediaTypeNames;

namespace clock_scr
{
    /// <summary>
    /// MainPage.xaml の相互作用ロジック
    /// </summary>
    public partial class MainPage : Page
    {
        private DispatcherTimer? timer;
        private Timer? clockTimer;
        private int rotationCount;
        private int lastMinute;
        private int lastHour;
        private int currentSecond;
        private int currentMinute;
        private int currentHour;

        private bool changeMinute1;
        private bool changeMinute10;
        private bool changeHour1;
        private bool changeHour10;
        private bool changeTF;
        private bool toggleTF;

        private string faceHL = "";
        private string faceHLN = "";
        private string faceHR = "";
        private string faceHRN = "";
        private string faceML = "";
        private string faceMLN = "";
        private string faceMR = "";
        private string faceMRN = "";
        private string faceTF = "";
        private double offsetHM = 0;
        private double watchSize;
        private double gradientBorder;
        private int timeFormat;
        private double offsetTF;
        private int dateIndication;
        private string? displayColor;
        private string? backFrameColor;

        private TextBlock[] textBlockTop;
        private TextBlock[] textBlockBottom;
        private TextBlock[] textBlockTF;
        private readonly LinearGradientBrush TopGradientBrush = new() { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1), GradientStops = new GradientStopCollection { new GradientStop(), new GradientStop() } };
        private readonly LinearGradientBrush BottomGradientBrush = new() { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1), GradientStops = new GradientStopCollection { new GradientStop(), new GradientStop() } };
        public Model3D HLT
        {
            get { return (Model3D)GetValue(HLTProperty); }
            set { SetValue(HLTProperty, value); }
        }

        public Model3D HLB
        {
            get { return (Model3D)GetValue(HLBProperty); }
            set { SetValue(HLBProperty, value); }
        }

        public Model3D HLTN
        {
            get { return (Model3D)GetValue(HLTNProperty); }
            set { SetValue(HLTNProperty, value); }
        }

        public Model3D HLBN
        {
            get { return (Model3D)GetValue(HLBNProperty); }
            set { SetValue(HLBNProperty, value); }
        }

        public Model3D HRT
        {
            get { return (Model3D)GetValue(HRTProperty); }
            set { SetValue(HRTProperty, value); }
        }

        public Model3D HRB
        {
            get { return (Model3D)GetValue(HRBProperty); }
            set { SetValue(HRBProperty, value); }
        }

        public Model3D HRTN
        {
            get { return (Model3D)GetValue(HRTNProperty); }
            set { SetValue(HRTNProperty, value); }
        }

        public Model3D HRBN
        {
            get { return (Model3D)GetValue(HRBNProperty); }
            set { SetValue(HRBNProperty, value); }
        }

        public Model3D MLT
        {
            get { return (Model3D)GetValue(MLTProperty); }
            set { SetValue(MLTProperty, value); }
        }

        public Model3D MLB
        {
            get { return (Model3D)GetValue(MLBProperty); }
            set { SetValue(MLBProperty, value); }
        }

        public Model3D MLTN
        {
            get { return (Model3D)GetValue(MLTNProperty); }
            set { SetValue(MLTNProperty, value); }
        }

        public Model3D MLBN
        {
            get { return (Model3D)GetValue(MLBNProperty); }
            set { SetValue(MLBNProperty, value); }
        }

        public Model3D MRT
        {
            get { return (Model3D)GetValue(MRTProperty); }
            set { SetValue(MRTProperty, value); }
        }

        public Model3D MRB
        {
            get { return (Model3D)GetValue(MRBProperty); }
            set { SetValue(MRBProperty, value); }
        }

        public Model3D MRTN
        {
            get { return (Model3D)GetValue(MRTNProperty); }
            set { SetValue(MRTNProperty, value); }
        }

        public Model3D MRBN
        {
            get { return (Model3D)GetValue(MRBNProperty); }
            set { SetValue(MRBNProperty, value); }
        }

        public Model3D TF
        {
            get { return (Model3D)GetValue(TFProperty); }
            set { SetValue(TFProperty, value); }
        }

        public Model3D DI
        {
            get { return (Model3D)GetValue(DIProperty); }
            set { SetValue(DIProperty, value); }
        }

        public static readonly DependencyProperty HLTProperty =
             DependencyProperty.Register(nameof(HLT), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty HLBProperty =
            DependencyProperty.Register(nameof(HLB), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty HLTNProperty =
            DependencyProperty.Register(nameof(HLTN), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty HLBNProperty =
            DependencyProperty.Register(nameof(HLBN), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty HRTProperty =
            DependencyProperty.Register(nameof(HRT), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty HRBProperty =
            DependencyProperty.Register(nameof(HRB), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty HRTNProperty =
            DependencyProperty.Register(nameof(HRTN), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty HRBNProperty =
            DependencyProperty.Register(nameof(HRBN), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty MLTProperty =
            DependencyProperty.Register(nameof(MLT), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty MLBProperty =
            DependencyProperty.Register(nameof(MLB), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty MLTNProperty =
            DependencyProperty.Register(nameof(MLTN), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty MLBNProperty =
            DependencyProperty.Register(nameof(MLBN), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty MRTProperty =
            DependencyProperty.Register(nameof(MRT), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty MRBProperty =
            DependencyProperty.Register(nameof(MRB), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty MRTNProperty =
            DependencyProperty.Register(nameof(MRTN), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty MRBNProperty =
            DependencyProperty.Register(nameof(MRBN), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty TFProperty =
            DependencyProperty.Register(nameof(TF), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        public static readonly DependencyProperty DIProperty =
            DependencyProperty.Register(nameof(DI), typeof(Model3D), typeof(MainPage), new PropertyMetadata(null));

        MainViewModel ViewModel;

        public MainPage()
        {
            InitializeComponent();

            ViewModel = (MainViewModel)DataContext;

            GetCurrentTime();
            SetLastTime();
            InitializeCustomSettings();

            InitializeTextBlocks();
            InitializeWatch();
            InitializeTimer();

            var events = new EventsExtension<Grid>(DicePanel);
            events.MouseDragDelta.Subscribe(ViewModel.RotateDelta);
        }
        private void GetCurrentTime()
        {
            currentSecond = DateTime.Now.Second;
            currentMinute = DateTime.Now.Minute;
            currentHour = DateTime.Now.Hour;
            if (timeFormat != 0)
            {
                toggleTF = currentHour >= 12;
                if (toggleTF)
                {
                    currentHour %= 12;
                }
            }
        }

        private void SetLastTime()
        {
            //lastSecond = currentSecond;
            lastMinute = currentMinute;
            lastHour = currentHour;
        }
        private void InitializeCustomSettings()
        {

            watchSize = Properties.Settings.Default.watchSize;
            offsetHM = Properties.Settings.Default.offsetHM;
            gradientBorder = Properties.Settings.Default.gradientBorder;
            timeFormat = Properties.Settings.Default.timeFormat;
            offsetTF = Properties.Settings.Default.offsetTF;
            dateIndication = Properties.Settings.Default.dateIndication;
            displayColor = Properties.Settings.Default.displayColor;
            backFrameColor = Properties.Settings.Default.backFrameColor;

            SetMatrix(Properties.Settings.Default.rotateAngel);
        }

        private void InitializeTextBlocks()
        {
            textBlockTop = new TextBlock[]
            {
                HrLeftTop, HrLeftTopNew, HrRightTop, HrRightTopNew,
                MinLeftTop, MinLeftTopNew, MinRightTop, MinRightTopNew
            };
            textBlockBottom = new TextBlock[]
            {
                HrLeftBottom, HrLeftBottomNew, HrRightBottom, HrRightBottomNew,
                MinLeftBottom, MinLeftBottomNew, MinRightBottom, MinRightBottomNew
            };
            textBlockTF = new TextBlock[]
            {
                TimeFormatAM,TimeFormatPM
            };
        }

        private void ChangeStyle(TextBlock[] textBlocks, Style baseStyle, Setter[] additionalSetters)
        {
            Style newStyle = new(typeof(TextBlock))
            {
                BasedOn = baseStyle
            };
            foreach (Setter setter in additionalSetters)
            {
                newStyle.Setters.Add(setter);
            }

            foreach (var textBlock in textBlocks)
            {
                textBlock.Style = newStyle;
            }
        }

        public void InitializeWatch()
        {

            PerspectiveCamera camera = new()
            {
                Position = new Point3D(0, 0, watchSize)
            };
            viewport.Camera = camera;
            LoadOffset();

            Setter foregroundSetter = new(TextBlock.ForegroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString(displayColor)));
            Setter backgroundSetter = new(TextBlock.BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString(backFrameColor)));
            Setter opacityMaskTopSetter = new(TextBlock.OpacityMaskProperty, new LinearGradientBrush(new GradientStopCollection()
                {new GradientStop(Colors.Black, gradientBorder), new GradientStop(Colors.Transparent, gradientBorder)}, new Point(0, 0), new Point(0, 1)));
            Setter opacityMaskBottomSetter = new(TextBlock.OpacityMaskProperty, new LinearGradientBrush(new GradientStopCollection()
                {new GradientStop(Colors.Transparent, 1 - gradientBorder), new GradientStop(Colors.Black, 1 - gradientBorder)}, new Point(0, 0), new Point(0, 1))); ChangeStyle(textBlockTop, (Style)Resources["FaceStyleTop"], new Setter[] { foregroundSetter, backgroundSetter });
            
            //ChangeStyle(textBlockTop, (Style)Resources["FaceStyleTop"], new Setter[] { foregroundSetter, backgroundSetter, opacityMaskTopSetter }); 
            ChangeStyle(textBlockBottom, (Style)Resources["FaceStyleBottom"], new Setter[] { foregroundSetter, backgroundSetter, opacityMaskBottomSetter });
            ChangeStyle(textBlockTF, (Style)Resources["FaceStyleTF"], new Setter[] { foregroundSetter, backgroundSetter });

            GetCurrentTime();
            SetLastTime();

            if (timeFormat != 0)
            {
                if (timeFormat == 1)
                {
                    TimeFormatPM.RenderTransform = new ScaleTransform(1, -1);
                }
                else if (timeFormat == 2)
                {
                    TimeFormatPM.RenderTransform = new ScaleTransform(-1, 1);
                }
                faceTF = FaceTFCoord(-2.0 - offsetHM - offsetTF, toggleTF, timeFormat);
                TF = CubeUtility.CreateFaceModel(faceTF, nameof(TimeFormatAM), nameof(TimeFormatPM));
            }
            else
            {
                TF = new Model3DGroup();
            }

            if (dateIndication != 0)
            {
                DI = CubeUtility.CreateFaceModel("-2,-1.2,0 -2,-2.2,0 1,-2.2,0 1,-1.2,0", nameof(DateIndication));
            }
            else
            {
                DI = new Model3DGroup();
            }

            CultureInfo englishCulture = new CultureInfo("en-US");
            DateTime currentDate = DateTime.Now;

            DateIndicationLeft.Text = currentDate.ToString("dd", englishCulture);
            DateIndicationTop.Text = currentDate.ToString("dddd", englishCulture);
            DateIndicationDown.Text = currentDate.ToString("MMMM yyyy", englishCulture);
            HrRightTop.Text = (currentHour % 10).ToString();
            HrRightBottom.Text = (currentHour % 10).ToString();
            HrLeftTop.Text = (currentHour >= 10 ? currentHour / 10 : 0).ToString();
            HrLeftBottom.Text = (currentHour >= 10 ? currentHour / 10 : 0).ToString();
            MinRightTop.Text = (currentMinute % 10).ToString();
            MinRightBottom.Text = (currentMinute % 10).ToString();
            MinLeftTop.Text = (currentMinute >= 10 ? currentMinute / 10 : 0).ToString();
            MinLeftBottom.Text = (currentMinute >= 10 ? currentMinute / 10 : 0).ToString();
            MRT = CubeUtility.CreateFaceModel(faceMR, nameof(MinRightTop));
            MRB = CubeUtility.CreateFaceModel(faceMR, nameof(MinRightBottom));
            MLT = CubeUtility.CreateFaceModel(faceML, nameof(MinLeftTop));
            MLB = CubeUtility.CreateFaceModel(faceML, nameof(MinLeftBottom));
            HRT = CubeUtility.CreateFaceModel(faceHR, nameof(HrRightTop));
            HRB = CubeUtility.CreateFaceModel(faceHR, nameof(HrRightBottom));
            HLT = CubeUtility.CreateFaceModel(faceHL, nameof(HrLeftTop));
            HLB = CubeUtility.CreateFaceModel(faceHL, nameof(HrLeftBottom));
            
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            timer.Tick += Timer_Tick;

            clockTimer = new Timer(1000);
            clockTimer.Elapsed += ClockTimer_Elapsed;
            clockTimer.Start();
        }

        public void SetSettings(double offset1, double size, double border, int tf, double offset2, int di, string color1, string color2){
            offsetHM = offset1;
            watchSize = size;
            gradientBorder = border;
            timeFormat = tf;
            offsetTF = offset2;
            dateIndication = di;
            displayColor = color1;
            backFrameColor = color2;
        }

        private static string FaceCoord(double p, bool f)
        {
            return Math.Round(p , 2) + "," + (f ? -1 : 1) + ",0 " + Math.Round(p , 2) + "," + (f ? 1 : -1) + ",0 " + Math.Round(p + 1.0, 2) + "," + (f ? 1 : -1) + ",0 " + Math.Round(p + 1.0, 2) + "," + (f ? -1 : 1) + ",0";
        }

        private static string FaceTFCoord(double p, bool toggle, int tf)
        {
            if (tf == 1)
            {
                return Math.Round(p - 1.0, 2) + "," + (toggle ? -1 : 1) + ",0 " + Math.Round(p - 1.0, 2) + "," + (toggle ? -0.5 : 0.5) + ",0 " + Math.Round(p, 2) + "," + (toggle ? -0.5 : 0.5) + ",0 " + Math.Round(p, 2) + "," + (toggle ? -1 : 1) + ",0";
            }
            else
            {
                return Math.Round(p - 0.5, 2) + ",0 ,0 " + Math.Round(p, 2) + ",0 ,0 " + Math.Round(p, 2) + "," + (toggle ? -1 : 1) + ",0 " + Math.Round(p - 0.5, 2) + "," + (toggle ? -1 : 1) + ",0";
            }
            
        }

        private void ClockTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            GetCurrentTime();

            if (currentMinute != lastMinute || currentHour != lastHour)
            {
                changeMinute1 = currentMinute % 10 != lastMinute % 10;
                changeMinute10 = (currentMinute == 0 ? 0 : currentMinute / 10) != (lastMinute == 0 ? 0 : lastMinute / 10);
                changeHour1 = currentHour % 10 != lastHour % 10;
                changeHour10 = (currentHour == 0 ? 0 : currentHour / 10) != (lastHour == 0 ? 0 : lastHour / 10);

                changeTF = (lastHour - currentHour == 11);
                timer?.Start();
                SetLastTime();
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (rotationCount == 0)
            {
                if (changeMinute1)
                {
                    MinRightTopNew.Text = (currentMinute % 10).ToString();
                    MinRightBottomNew.Text = (currentMinute % 10).ToString();
                    MRBN = CubeUtility.CreateFaceModel(faceMRN, nameof(MinRightBottomNew));
                }
                if (changeMinute10)
                {
                    MinLeftTopNew.Text = (currentMinute >= 10 ? currentMinute / 10 : 0).ToString();
                    MinLeftBottomNew.Text = (currentMinute >= 10 ? currentMinute / 10 : 0).ToString();
                    MLBN = CubeUtility.CreateFaceModel(faceMLN, nameof(MinLeftBottomNew));
                }
                if (changeHour1)
                {
                    HrRightTopNew.Text = (currentHour % 10).ToString();
                    HrRightBottomNew.Text = (currentHour % 10).ToString();
                    HRBN = CubeUtility.CreateFaceModel(faceHRN, nameof(HrRightBottomNew));
                }
                if (changeHour10)
                {
                    HrLeftTopNew.Text = (currentHour >= 10 ? currentHour / 10 : 0).ToString();
                    HrLeftBottomNew.Text = (currentHour >= 10 ? currentHour / 10 : 0).ToString();
                    HLBN = CubeUtility.CreateFaceModel(faceHLN, nameof(HrLeftBottomNew));
                }

            }
            if (rotationCount == 1)
            {
                if (changeMinute1)
                {
                    MRTN = CubeUtility.CreateFaceModel(faceMR, nameof(MinRightTopNew));
                }
                if (changeMinute10)
                {
                    MLTN = CubeUtility.CreateFaceModel(faceML, nameof(MinLeftTopNew));
                }
                if (changeHour1)
                {
                    HRTN = CubeUtility.CreateFaceModel(faceHR, nameof(HrRightTopNew));
                }
                if (changeHour10)
                {
                    HLTN = CubeUtility.CreateFaceModel(faceHL, nameof(HrLeftTopNew));
                }
            }
            if (rotationCount < 36)
            {
                if (changeMinute1)
                {
                    ModelRotate.Rotate(MRT, Axes["+x"]);
                    ModelRotate.Rotate(MRBN, Axes["+x"]);
                }
                if (changeMinute10)
                {
                    ModelRotate.Rotate(MLT, Axes["+x"]);
                    ModelRotate.Rotate(MLBN, Axes["+x"]);
                }
                if (changeHour1)
                {
                    ModelRotate.Rotate(HRT, Axes["+x"]);
                    ModelRotate.Rotate(HRBN, Axes["+x"]);
                }
                if (changeHour10)
                {
                    ModelRotate.Rotate(HLT, Axes["+x"]);
                    ModelRotate.Rotate(HLBN, Axes["+x"]);
                }
                if (changeTF)
                {
                    ModelRotate.Rotate(TF, Axes["+x"]);
                }
                rotationCount++;
            }
            else
            {
                if (changeMinute1)
                {
                    MinRightTop.Text = MinRightTopNew.Text;
                    MinRightBottom.Text = MinRightBottomNew.Text;

                    MRT = CubeUtility.CreateFaceModel(faceMR, MinRightTop);
                    MRB = CubeUtility.CreateFaceModel(faceMR, MinRightBottom);

                    MRTN = new Model3DGroup();
                    MRBN = new Model3DGroup();
                }
                if (changeMinute10)
                {
                    MinLeftTop.Text = MinLeftTopNew.Text;
                    MinLeftBottom.Text = MinLeftBottomNew.Text;

                    MLT = CubeUtility.CreateFaceModel(faceML, nameof(MinLeftTop));
                    MLB = CubeUtility.CreateFaceModel(faceML, nameof(MinLeftBottom));

                    MLTN = new Model3DGroup();
                    MLBN = new Model3DGroup();
                }
                if (changeHour1)
                {
                    HrRightTop.Text = HrRightTopNew.Text;
                    HrRightBottom.Text = HrRightBottomNew.Text;

                    HRT = CubeUtility.CreateFaceModel(faceHR, nameof(HrRightTop));
                    HRB = CubeUtility.CreateFaceModel(faceHR, nameof(HrRightBottom));

                    HRTN = new Model3DGroup();
                    HRBN = new Model3DGroup();
                }
                if (changeHour10)
                {
                    HrLeftTop.Text = HrLeftTopNew.Text;
                    HrLeftBottom.Text = HrLeftBottomNew.Text;

                    HLT = CubeUtility.CreateFaceModel(faceHL, nameof(HrLeftTop));
                    HLB = CubeUtility.CreateFaceModel(faceHL, nameof(HrLeftBottom));

                    HLTN = new Model3DGroup();
                    HLBN = new Model3DGroup();
                }
                timer?.Stop();
                rotationCount = 0;
                changeMinute1 = false;
                changeMinute10 = false;
                changeHour1 = false;
                changeHour10 = false;
                changeTF = false;
            }

        }

        private void LoadOffset()
        {
            faceHL = FaceCoord(-2.0 - offsetHM, false);
            faceHLN = FaceCoord(-2.0 - offsetHM, true);
            faceHR = FaceCoord(-1.0 - offsetHM, false);
            faceHRN = FaceCoord(-1.0 - offsetHM, true);
            faceML = FaceCoord(offsetHM, false);
            faceMLN = FaceCoord(offsetHM, true);
            faceMR = FaceCoord(1.0 + offsetHM, false);
            faceMRN = FaceCoord(1.0 + offsetHM, true);
        }

        static readonly Dictionary<string, Vector3D> Axes = new()
            {
                { "-x", Vector3D.Parse("-1,0,0") },
                { "+x", Vector3D.Parse("1,0,0") },
                { "-y", Vector3D.Parse("0,-1,0") },
                { "+y", Vector3D.Parse("0,1,0") },
                { "-z", Vector3D.Parse("0,0,-1") },
                { "+z", Vector3D.Parse("0,0,1") },
            };


        public string GetMatrix()
        {
            string matrix = ViewModel.Angle;
            return matrix;
        }
        public void SetMatrix(string matrix)
        {
            ViewModel.Angle = matrix;
        }
    }
}
