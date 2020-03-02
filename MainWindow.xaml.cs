using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DiscordRPC;
using DiscordRPC.Message;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CustomRPC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            load();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        private const string client_id = "684091407836774400";

        DispatcherTimer timer = new DispatcherTimer();

        DiscordRpcClient client;

        RichPresence presence = new RichPresence()
        {
            Timestamps = new Timestamps()
            {
                Start = DateTime.Now
            },
            Details = "Developing great application",
            State = "Playing mode: Always Alone"
        };

        public void load()
        {
            client = new DiscordRpcClient(client_id)
            {
                SkipIdenticalPresence = false
            };

            client.OnReady += onReady;

            client.OnError += Client_OnError;

            client.OnConnectionFailed += (_, __) =>
            {
                client.Deinitialize();
                MessageBox.Show("Клиент был отключен: " + __.FailedPipe);
            };

            client.Initialize();
        }

        private void Client_OnError(object sender, ErrorMessage args)
        {
            MessageBox.Show("Что-то пошло не так...\n" + args.Message);
        }

        private void onReady(object _, ReadyMessage __)
        {
            //timer.Start();
            updateStatus();
        }

        private void updateStatus()
        {
            if (!client.IsInitialized)
                return;

            client.SetPresence(presence);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            updateStatus();
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            updateStatus();
        }

        private void bApply_Click(object sender, RoutedEventArgs e)
        {
            presence.Details = tbDetails.Text;
            presence.State = tbState.Text;
        }
    }
}
