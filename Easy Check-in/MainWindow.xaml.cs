using System;
using System.Collections.Generic;
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
using MahApps.Metro.Controls;
using System.Net.NetworkInformation;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Net.Sockets;


namespace Easy_Check_in
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            HideScriptErrors(WebViewer, true);
        }
        public async void CheckConnect()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                Captive_Tab.IsSelected = true;
                TracUtil("hotspot_reg");
                ReFreshPage();
            }
            else
            {
                Wifi_Tab.IsSelected = true;
                wifi_videoguide.Play();
                WifiSettings();
                while (!NetworkInterface.GetIsNetworkAvailable())
                {
                    await Task.Delay(3000);
                }
                Captive_Tab.IsSelected = true;
                TracUtil("hotspot_reg");
                ReFreshPage();
            }
        }


        public bool IsInternetAvailable()
        {
            System.Net.WebRequest ExtReq = System.Net.WebRequest.Create("https://rosbank.ru/");
            System.Net.WebResponse ExtResp;
            try
            {
                ExtReq.Timeout = 7000;
                ExtResp = ExtReq.GetResponse();
                ExtResp.Close();
                ExtReq = null;
                return true;
            }
            catch
            {
                ExtReq = null;
                return false;
            }
        }

        public bool NeededVPN()
        {
            System.Net.WebRequest IntReq = System.Net.WebRequest.Create("https://sm/");
            System.Net.WebResponse IntResp;
            try
            {
                IntReq.Timeout = 5000;
                IntResp = IntReq.GetResponse();
                IntResp.Close();
                IntReq = null;
                return true;
            }
            catch
            {
                IntReq = null;
                return false;
            }
        }

        public async void ReFreshPage()
        {
            if (IsInternetAvailable())
            {
                WebViewer.Visibility = Visibility.Hidden;
                await Task.Delay(5000);
                VPN_Tab.IsSelected = true;
                vpn_videoguide.Play();
                VPNStep();
            }
            else
            {
                WebViewer.Visibility = Visibility.Visible;
                WebViewer.Source = new Uri("http://msftconnecttest.com/redirect");
                while (!IsInternetAvailable())
                {
                    await Task.Delay(5000);
                }
                WebViewer.Visibility = Visibility.Hidden;
                await Task.Delay(3000);
                VPN_Tab.IsSelected = true;
                vpn_videoguide.Play();
                VPNStep();
            }
        }

        public async void VPNStep()
        {
            if (NeededVPN())
            {
                Connected_Tab.IsSelected = true;
            }
            else
            {
                TracUtil("connectgui");
                while (!NeededVPN())
                {
                    await Task.Delay(5000);
                }
                Connected_Tab.IsSelected = true;
            }
        }

        void TracUtil(string Args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Program Files (x86)\\CheckPoint\\Endpoint Security\\Endpoint Connect\\trac.exe");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.Arguments = Args;
            Process.Start(startInfo);
        }

        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }

        public void WifiSettings()
        {
            Process.Start("ms-availablenetworks:");
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Here will be additional options!");
        }

        private void Greeting_Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckConnect();
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void repeat_wifi_button_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan timeSpan = new TimeSpan();
            System.TimeSpan.TryParse("00:00:08", out timeSpan);
            wifi_videoguide.Stop();
            wifi_videoguide.Position = timeSpan;
            wifi_videoguide.Play();
        }

        private void repeat_vpn_button_Click(object sender, RoutedEventArgs e)
        {
            vpn_videoguide.Stop();
            vpn_videoguide.Play();
        }
    }
}
