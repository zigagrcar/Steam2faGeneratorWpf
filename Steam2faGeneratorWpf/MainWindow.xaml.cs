using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SteamAuth;
using System.Threading;

namespace Steam2faGeneratorWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static System.Timers.Timer myTimer { get; private set; }

        private void getCode()
        {
            string sharedSecret = "";

            try
            {
                if (!File.Exists("shared_secret.txt"))
                {
                    throw new FileNotFoundException();
                }
                sharedSecret = File.ReadAllText(@"shared_secret.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                if (sharedSecret.Length == 0)
                {
                    throw new FileLoadException();
                }
                SteamGuardAccount account = new SteamGuardAccount();
                account.SharedSecret = sharedSecret;
                string code = account.GenerateSteamGuardCode();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    textBox.Text = code;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            // Create a timer
            myTimer = new System.Timers.Timer();
            // Tell the timer what to do when it elapses
            myTimer.Elapsed += new ElapsedEventHandler(myEvent);
            // Set it to go off every five seconds
            myTimer.Interval = 5000;
            // And start it        
            myTimer.Enabled = true;

            // Get code on start
            getCode();
        }

        private void bCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textBox.Text);
        }

        private void bExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Implement a call with the right signature for events going off
        private void myEvent(object source, ElapsedEventArgs e)
        {
            getCode();
        }
    }
}
