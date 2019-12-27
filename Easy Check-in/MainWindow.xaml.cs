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
            Additional.HideScriptErrors(WebViewer, true);
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
                while (!Additional.Connected)
                {
                    await Task.Run(() =>
                    {
                        Additional.CheckConnect();
                    });
                }
            }
            Captive_Portal_Auth();
        }

        public async void Captive_Portal_Auth()
        {
            Additional.TracUtil("hotspot_reg");
            Captive_Tab.IsSelected = true;
            await Task.Run(() =>
            {
                Additional.CheckInternetConnection();
            });
            if (Additional.ConnectedToTheIntetnet)  {  }
            else
            {
                WebViewer.Source = new Uri("http://msftconnecttest.com/redirect");
                await Task.Delay(2000);
                WebViewer.Visibility = Visibility.Visible;
                while (!Additional.ConnectedToTheIntetnet)
                    await Task.Run(() =>
                    {
                        Additional.CheckInternetConnection();
                    });
                WebViewer.Visibility = Visibility.Hidden;
            }
            VPN_CP_ConnectionCheck();
        }

        public async void VPN_CP_ConnectionCheck()
        {
            await Task.Run(() =>
            {
                Additional.CheckVPNConnection();
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
                        Additional.CheckVPNConnection();
                    });
            }
            ConnectedToVPN();
        }

        public void ConnectedToVPN()
        {
            Connected_Tab.IsSelected = true;
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
            WiFi_Connect();
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
