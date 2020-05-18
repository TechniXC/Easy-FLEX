using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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


namespace Easy_FLEX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            Additional.HideScriptErrors(WebViewer, true);
        }

        public async void Step_Back()
        {
            while (Additional.Connected)
            {
                await Task.Delay(2000);
            }
            MessageBox.Show(Properties.Resources.NtwTrouble);
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        public async void WiFi_Connect()
        {
            Easy_FLEX_MainWindow.Title = "Easy FLEX - " + Properties.Resources.WiFiTab;
            for (double i = progressBar.Value; i <= 25; i++)
            {
                progressBar.Value = i;
                await Task.Delay(1);
            }
            SecondCircle.Fill = new SolidColorBrush(Colors.Red);
            Two.Foreground = new SolidColorBrush(Colors.Black);
            TwoText.Foreground = new SolidColorBrush(Colors.Black);
            if (NetworkInterface.GetIsNetworkAvailable()) 
            { 
                Additional.Connected = true;
            }
            else
            {
                Wifi_Tab.IsSelected = true;
                WifiSettings();
                wifi_videoguide.Play();
                await Task.Run(() =>
                {
                    Additional.CheckConnect();
                });
                while (!Additional.Connected)
                {
                    await Task.Delay(2000);
                }
            }
            Captive_Portal_Auth();
            await Task.Run(() =>
            {
                Additional.CheckConnect();
                Step_Back();
            });
        }

        public async void Captive_Portal_Auth()
        {
            Easy_FLEX_MainWindow.Title = "Easy FLEX - " + Properties.Resources.CaptiveTab;
            for (double i = progressBar.Value; i <= 50; i++)
            {
                progressBar.Value = i;
                await Task.Delay(1);
                if (progressBar.Value >= 25)
                {
                    SecondCircle.Fill = new SolidColorBrush(Colors.Red);
                    Two.Foreground = new SolidColorBrush(Colors.Black);
                    TwoText.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            ThirdCircle.Fill = new SolidColorBrush(Colors.Red);
            Three.Foreground = new SolidColorBrush(Colors.Black);
            ThreeText.Foreground = new SolidColorBrush(Colors.Black);
            Captive_Tab.IsSelected = true;
            await Task.Run(() =>
            {
                _ = Additional.CheckInternetConnection();
            });
            if (Additional.ConnectedToTheIntetnet)  {  }
            else
            {
                Additional.TracUtil("hotspot_reg");
                await Task.Delay(5000);
                await Task.Run(() =>
                {
                    _ = Additional.CheckInternetConnection();
                });
                if (Additional.ConnectedToTheIntetnet) { }
                else
                {
                    WebViewer.Source = new Uri("http://neverssl.com");
                    WebViewer.Visibility = Visibility.Visible;
                    while (!Additional.ConnectedToTheIntetnet)
                        await Task.Run(() =>
                        {
                            _ = Additional.CheckInternetConnection();
                            Task.Delay(2000);
                        });
                    WebViewer.Visibility = Visibility.Hidden;
                }
            }
            VPN_CP_ConnectionCheck();
        }

        public async void VPN_CP_ConnectionCheck()
        {
            Easy_FLEX_MainWindow.Title = "Easy FLEX - " + Properties.Resources.VPNTab;
            for (double i = progressBar.Value; i <= 75; i++)
            {
                progressBar.Value = i;
                await Task.Delay(1);
                if (progressBar.Value >= 25)
                {
                    SecondCircle.Fill = new SolidColorBrush(Colors.Red);
                    Two.Foreground = new SolidColorBrush(Colors.Black);
                    TwoText.Foreground = new SolidColorBrush(Colors.Black);
                }
                if (progressBar.Value >= 50)
                {
                    ThirdCircle.Fill = new SolidColorBrush(Colors.Red);
                    Three.Foreground = new SolidColorBrush(Colors.Black);
                    ThreeText.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            FourthCircle.Fill = new SolidColorBrush(Colors.Red);
            Four.Foreground = new SolidColorBrush(Colors.Black);
            FourText.Foreground = new SolidColorBrush(Colors.Black);
            /*await Task.Run(() =>
            {
                _ = Additional.CheckVPNConnection();
            });
            if (Additional.ConnectedVPN) {  }
            else
            {*/
                Additional.TracUtil("connectgui");
                VPN_Tab.IsSelected = true;
                vpn_videoguide.Play();
                while (!Additional.ConnectedVPN)
                    await Task.Run(() =>
                    {
                        _ = Additional.CheckVPNConnection();
                    });
            //}
            ConnectedToVPN();

        }

        public async void ConnectedToVPN()
        {
            Easy_FLEX_MainWindow.Title = "Easy FLEX - " + Properties.Resources.ConnectedTab;
            for (double i = progressBar.Value; i <= 100; i++)
            {
                progressBar.Value = i;
                await Task.Delay(1);
                if (progressBar.Value >= 25)
                {
                    SecondCircle.Fill = new SolidColorBrush(Colors.Red);
                    Two.Foreground = new SolidColorBrush(Colors.Black);
                    TwoText.Foreground = new SolidColorBrush(Colors.Black);
                }
                if (progressBar.Value >= 50)
                {
                    ThirdCircle.Fill = new SolidColorBrush(Colors.Red);
                    Three.Foreground = new SolidColorBrush(Colors.Black);
                    ThreeText.Foreground = new SolidColorBrush(Colors.Black);
                }
                if (progressBar.Value >= 75)
                {
                    FourthCircle.Fill = new SolidColorBrush(Colors.Red);
                    Four.Foreground = new SolidColorBrush(Colors.Black);
                    FourText.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            FifthCircle.Fill = new SolidColorBrush(Colors.Red);
            Five.Foreground = new SolidColorBrush(Colors.Black);
            FiveText.Foreground = new SolidColorBrush(Colors.Black);
            Connected_Tab.IsSelected = true;
            await Task.Delay(20000);
            Easy_FLEX_MainWindow.Close();
        }

        public void WifiSettings()
        {
            try
            {
                wifi_videoguide.Source = new Uri(@"wifi_guide_ac.mkv",UriKind.RelativeOrAbsolute);
                Process.Start("C:\\Program Files (x86)\\Cisco\\Cisco AnyConnect Secure Mobility Client\\vpnui.exe");
            }
            catch
            {
                wifi_videoguide.Source = new Uri(@"wifi_guide.mp4", UriKind.RelativeOrAbsolute);
                Process.Start("ms-availablenetworks:");
            }

            
        }

        private void Options_Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Easy FLEX™\nVersion: 1.0.2.2\nAuthor: Nikita Letov (GTS\\SEC)\nCopiright © 2020 All rights reserved");
        }

        private void Options_Restart_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private async void Greeting_Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            //GreetingLabelHeader.Foreground = new SolidColorBrush(Colors.Gray);
            //Easy_FLEX_MainWindow.Title = "Easy FLEX " + Properties.Resources.WiFiTab;
            //await Task.Delay(2000);
            //progressBar.Value = 20;
            Easy_FLEX_MainWindow.Title = "Easy FLEX - " + Properties.Resources.WiFiTab;
            First_check_bar.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                _ = Additional.CheckVPNConnection();
            });
            if (Additional.ConnectedVPN) { ConnectedToVPN(); }
            else 
            {
                Additional.TracUtil("disconnect");
                await Task.Delay(1000);
                WiFi_Connect(); 
            }
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            Easy_FLEX_MainWindow.Close();
        }

        private void repeat_wifi_button_Click(object sender, RoutedEventArgs e)
        {
            wifi_videoguide.Stop();
            wifi_videoguide.Play();
        }

        private void repeat_vpn_button_Click(object sender, RoutedEventArgs e)
        {
            vpn_videoguide.Stop();
            vpn_videoguide.Play();
        }

        private async void restart_vpn_button_Click(object sender, RoutedEventArgs e)
        {
            Additional.TracUtil("disconnect");
            await Task.Delay(1000);
            Additional.TracUtil("hotspot_reg");
            Additional.TracUtil("connectgui");
        }
    }
}
