using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Copilot2ChatGPT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int WINDOW_WIDTH = 506;

        public int AnimateSwitchLeft;

        public MainWindow()
        {
            InitializeComponent();

            AnimateSwitchLeft = (int)((SystemParameters.WorkArea.Width * 2 - WINDOW_WIDTH) / 2);

            // Hook Copilot Key.
            string message;
            var hookId = GlobalKeyboardHook.Instance.Hook(
                new List<Key> { Key.LWin, Key.LeftShift, Key.F23 }, () =>
                {
                    if (Left < AnimateSwitchLeft)
                        AnimateOut();
                    else
                        AnimateIn();
                }, out message);

            // Set window size
            Height = SystemParameters.WorkArea.Height;
            Width = WINDOW_WIDTH;
            Top = 0;
            Left = SystemParameters.WorkArea.Width;

            // Hide Window
            Visibility = Visibility.Hidden;
        }

        private void AnimateIn()
        {
            Visibility = Visibility.Visible;
            Topmost = true;
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = TimeSpan.FromMilliseconds(350);
            da.From = SystemParameters.WorkArea.Width;
            da.To = SystemParameters.WorkArea.Width - Width;
            da.EasingFunction = new CircleEase();
            BeginAnimation(LeftProperty, da);
        }

        private void AnimateOut()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = TimeSpan.FromMilliseconds(350);
            da.From = SystemParameters.WorkArea.Width - Width;
            da.To = SystemParameters.WorkArea.Width;
            da.EasingFunction = new CircleEase();
            da.Completed += (s, e) =>
            {
                Visibility = Visibility.Hidden;
                Topmost = false;
            };
            BeginAnimation(LeftProperty, da);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "button_OpenInNewWindow":
                    Process.Start("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe", "https://chat.openai.com/");
                    AnimateOut();
                    break;
                case "button_Settings":
                    break;
                case "button_Reload":
                    MainWebView.CoreWebView2.Reload();
                    break;
                case "button_Close":
                    AnimateOut();
                    break;
                default:
                    break;
            }
        }
    }
}