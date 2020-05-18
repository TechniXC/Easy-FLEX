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
    public class Additional : MetroWindow
    {
        public static void HideScriptErrors(WebBrowser wb, bool hide)
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

        public static void TracUtil(string Args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Program Files (x86)\\CheckPoint\\Endpoint Security\\Endpoint Connect\\trac.exe");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.Arguments = Args;
            try
            { Process.Start(startInfo); }
            catch
            {
                MessageBox.Show(Properties.Resources.NoCPSoft);
                Environment.Exit(1);
            }
        }

        public static bool ConnectedToTheIntetnet = false;

        public async static Task CheckInternetConnection()
        {
            if (IsInternetAvailableTcp())
            { ConnectedToTheIntetnet = true; }
            else
            { ConnectedToTheIntetnet = false; }
        }
        /*public static bool IsInternetAvailable()
        {
            System.Net.WebRequest ExtReq = System.Net.WebRequest.Create("https://rosbank.ru/");
            ExtReq.Timeout = 10000;
            try
            {
                System.Net.WebResponse ExtResp = ExtReq.GetResponse();
                ExtResp.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }*/

        public static bool ConnectedVPN = false;
        public static bool NeededVPN()
        {
            System.Net.WebRequest IntReq = System.Net.WebRequest.Create("https://sm/");
            IntReq.Timeout = 3000;
            System.Net.WebResponse IntResp;
            try
            {
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

        public async static Task CheckVPNConnection()
        {
            if (NeededVPN())
            { ConnectedVPN = true; }
            else
            { ConnectedVPN = false; }
        }

        public static bool Connected = false;
        public async static Task CheckConnect()
        {
            while (true)
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                { Connected = true; }
                else
                { Connected = false; }
                await Task.Delay(2000);
            }
        }

        public static bool IsInternetAvailableTcp()
        {
                try
                {
                    using (var client = new TcpClient())
                    {
                        var result = client.BeginConnect("fo.rosbank.ru", 443, null, null);
                        var success = result.AsyncWaitHandle.WaitOne(10);
                        client.EndConnect(result);
                        return true;
                    }
                }
                catch
                {
                    return false;
            }
        }
    }
}
