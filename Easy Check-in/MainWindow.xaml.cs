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
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Here will be additional options!");
        }

        private void Greeting_Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            Wifi_Tab.IsSelected = true;
            wifi_videoguide.Play();
        }
    }
}
