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
using System.Configuration;

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
            StartdateTime = DateTime.UtcNow;
            presence.Timestamps = new Timestamps(StartdateTime);
            LoadStatus();
        }

        static System.Collections.Specialized.NameValueCollection settings = ConfigurationManager.AppSettings;

        private string client_id = settings.Get("ApplicationID");

        DispatcherTimer timer = new DispatcherTimer();

        DiscordRpcClient client;

        DateTime StartdateTime = default;

        DateTime EnddateTime = default;

        RichPresence presence = new RichPresence();

        private void UpdatePresence(DateTime StartTime, string details = "", string state = "", string LargeImg = "", string LargeImgText = "", string SmallImg = "", string SmallImgText = "")
        {
            presence = new RichPresence()
            {
                Assets = new Assets()
                {
                    LargeImageKey = LargeImg,
                    LargeImageText = LargeImgText,
                    SmallImageKey = SmallImg,
                    SmallImageText = SmallImgText
                },
                Details = details,
                State = state,
                Timestamps = new Timestamps()
                {
                    Start = StartTime
                }
            };
        }

        public void load()
        {
            client = new DiscordRpcClient(client_id)
            {
                SkipIdenticalPresence = true
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

        private void bUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            updateStatus();
        }

        private void bApply_Click(object sender, RoutedEventArgs e)
        {
            UpdatePresence(StartdateTime, tbDetails.Text, tbState.Text, "", tbLargeImgText.Text, "", tbSmallImgText.Text);
        }

        private void cbSpectate_Checked(object sender, RoutedEventArgs e)
        {
            //settings.Set("boolSpectate", "True");
            bSetSpectate.IsEnabled = true;
        }

        private void cbAskToJoin_Checked(object sender, RoutedEventArgs e)
        {
            //settings.Set("boolAskToJoin", "True");
            bSetAskToJoin.IsEnabled = true;
        }

        private void cbSpectate_Unchecked(object sender, RoutedEventArgs e)
        {
            //settings.Set("boolSpectate", "False");
            bSetSpectate.IsEnabled = false;
        }

        private void cbAskToJoin_Unchecked(object sender, RoutedEventArgs e)
        {
            //settings.Set("boolAskToJoin", "False");
            bSetAskToJoin.IsEnabled = false;
        }

        private void bLoadStatus_Click(object sender, RoutedEventArgs e)
        {
            LoadStatus();
        }

        private void LoadStatus()
        {
            tbDetails.Text = settings.Get("Details");
            tbState.Text = settings.Get("State");
            bSetSpectate.IsEnabled = Convert.ToBoolean(settings.Get("boolSpectate"));
            bSetAskToJoin.IsEnabled = Convert.ToBoolean(settings.Get("boolAskToJoin"));
            cbSpectate.IsChecked = Convert.ToBoolean(settings.Get("boolSpectate"));
            cbAskToJoin.IsChecked = Convert.ToBoolean(settings.Get("boolAskToJoin"));
            tbSmallImgText.Text = settings.Get("SmallImageText");
            tbLargeImgText.Text = settings.Get("LargeImageText");
        }

        private void SaveStatus()
        {
            settings.Set("ApplicationID", client.ApplicationID);
            settings.Set("Details", tbDetails.Text);
            settings.Set("State", tbState.Text);
            settings.Set("boolSpectate", Convert.ToString(cbSpectate.IsChecked.GetValueOrDefault()));
            settings.Set("boolAskToJoin", Convert.ToString(cbAskToJoin.IsChecked.GetValueOrDefault()));
            settings.Set("LargeImage", "");
            settings.Set("LargeImageText", tbLargeImgText.Text);
            settings.Set("SmallImage", "");
            settings.Set("SmallImageText", tbSmallImgText.Text);
        }

        private void bUpdateConfig_Click(object sender, RoutedEventArgs e)
        {
            SaveStatus();
        }
    }
}
