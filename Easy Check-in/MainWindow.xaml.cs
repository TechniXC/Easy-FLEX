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
            MessageBox.Show("There are troubles with Network connection.\nWe have to start again.\nSorry...");
            Greeting_Tab.IsSelected = true;
        }

        public async void WiFi_Connect()
        {
            if (NetworkInterface.GetIsNetworkAvailable()) 
            { 
                Additional.Connected = true;
            }
            else
            {
                Wifi_Tab.IsSelected = true;
                wifi_videoguide.Play();
                WifiSettings();
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
                Step_Back();
            });
        }

        public async void Captive_Portal_Auth()
        {
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
                    WebViewer.Source = new Uri("http://msftconnecttest.com/redirect");
                    WebViewer.Visibility = Visibility.Visible;
                    while (!Additional.ConnectedToTheIntetnet)
                        await Task.Run(() =>
                        {
                            _ = Additional.CheckInternetConnection();
                        });
                    WebViewer.Visibility = Visibility.Hidden;
                }
            }
            VPN_CP_ConnectionCheck();
        }

        public async void VPN_CP_ConnectionCheck()
        {
            await Task.Run(() =>
            {
                _ = Additional.CheckVPNConnection();
            });
            if (Additional.ConnectedVPN) {  }
            else
            {
                Additional.TracUtil("connectgui");
                VPN_Tab.IsSelected = true;
                vpn_videoguide.Play();
                while (!Additional.ConnectedVPN)
                    await Task.Run(() =>
                    {
                        _ = Additional.CheckVPNConnection();
                    });
            }
            ConnectedToVPN();

        }

        public async void ConnectedToVPN()
        {
            Connected_Tab.IsSelected = true;
            await Task.Delay(20000);
            Easy_FLEX_MainWindow.Close();
        }

        public void WifiSettings()
        {
            Process.Start("ms-availablenetworks:");
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Easy FLEX™\nVersion: Alpha\nAuthor: Nikita Letov (GTS\\SEC)\nCopiright © 2020 All rights reserved");
        }

        private async void Greeting_Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            First_check_bar.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                _ = Additional.CheckVPNConnection();
            });
            if (Additional.ConnectedVPN) { ConnectedToVPN(); }
            else { WiFi_Connect(); }
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            Easy_FLEX_MainWindow.Close();
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

        private void restart_vpn_button_Click(object sender, RoutedEventArgs e)
        {
            Additional.TracUtil("hotspot_reg");
            Additional.TracUtil("connectgui");
        }
    }
}
