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
using System.Reflection;

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
            LoadStatus();
            UpdatePresence(TimeStamp, tbDetails.Text, tbState.Text, LargeImg, tbLargeImgText.Text, SmallImg, tbSmallImgText.Text);
        }

        static System.Collections.Specialized.NameValueCollection settings = ConfigurationManager.AppSettings;

        public string client_id = settings.Get("ApplicationID");

        DispatcherTimer timer = new DispatcherTimer();

        public DiscordRpcClient client;

        public DateTime TimeStamp = default;

        public string sTimeStamp = null;

        public string LargeImg = settings.Get("LargeImage");

        public string SmallImg = settings.Get("SmallImage");

        public bool isStartTime = true;

        string sendname = null;

        RichPresence presence = new RichPresence();

        private void UpdatePresence(DateTime Timestamp, string details = "", string state = "", string LargeImg = "", string LargeImgText = "", string SmallImg = "", string SmallImgText = "")
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
                State = state
            };
            if (DateTime.UtcNow.Subtract(TimeStamp).TotalSeconds > 0)
            {
                presence.Timestamps = new Timestamps()
                {
                    Start = Timestamp
                };
            }
            else
            {
                presence.Timestamps = new Timestamps()
                {
                    End = Timestamp
                };
            }
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
            timer.Start();
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
            if (DateTime.UtcNow.Subtract(TimeStamp).TotalSeconds > 0)
            {
                presence.Timestamps = new Timestamps()
                {
                    Start = TimeStamp
                };
            }
            else
            {
                presence.Timestamps = new Timestamps()
                {
                    End = TimeStamp
                };
            }
            updateStatus();
            if (DateTime.UtcNow.Subtract(TimeStamp).TotalSeconds > 0)
            {
                lTimestamp.Content = $"{(Convert.ToInt32(DateTime.UtcNow.Subtract(TimeStamp).Hours) == 0? Convert.ToString(Convert.ToInt32(DateTime.UtcNow.Subtract(TimeStamp).Minutes)) : Convert.ToString(Convert.ToString(Convert.ToInt32(DateTime.UtcNow.Subtract(TimeStamp).Hours))) + ":" + Convert.ToString(Convert.ToInt32(DateTime.UtcNow.Subtract(TimeStamp).Minutes)))}:{Convert.ToInt32(DateTime.UtcNow.Subtract(TimeStamp).Seconds)} elapsed";
            }
            else
            {
                lTimestamp.Content = $"{(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Hours) == 0 ? Convert.ToString(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Minutes)) : Convert.ToString(Convert.ToString(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Hours))) + ":" + Convert.ToString(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Minutes)))}:{Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Seconds)} left";
            }
        }

        private void bUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            updateStatus();
        }

        private void bApply_Click(object sender, RoutedEventArgs e)
        {
            UpdatePresence(TimeStamp, tbDetails.Text, tbState.Text, LargeImg, tbLargeImgText.Text, SmallImg, tbSmallImgText.Text);
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
            lGName.Content = "ApplicationID: " + settings.Get("ApplicationID");
            tbDetails.Text = settings.Get("Details");
            tbState.Text = settings.Get("State");
            bSetSpectate.IsEnabled = Convert.ToBoolean(settings.Get("boolSpectate"));
            bSetAskToJoin.IsEnabled = Convert.ToBoolean(settings.Get("boolAskToJoin"));
            cbSpectate.IsChecked = Convert.ToBoolean(settings.Get("boolSpectate"));
            cbAskToJoin.IsChecked = Convert.ToBoolean(settings.Get("boolAskToJoin"));
            tbSmallImgText.Text = settings.Get("SmallImageText");
            tbLargeImgText.Text = settings.Get("LargeImageText");
            LargeImg = settings.Get("LargeImage");
            SmallImg = settings.Get("SmallImage");
            sTimeStamp = settings.Get("Timestamp");
            TimeStamp = DateTime.Parse(sTimeStamp).ToUniversalTime();
        }

        private void SaveStatus()
        {
            settings.Set("ApplicationID", client_id);
            settings.Set("Details", tbDetails.Text);
            settings.Set("State", tbState.Text);
            settings.Set("boolSpectate", Convert.ToString(cbSpectate.IsChecked.GetValueOrDefault()));
            settings.Set("boolAskToJoin", Convert.ToString(cbAskToJoin.IsChecked.GetValueOrDefault()));
            settings.Set("LargeImage", LargeImg);
            settings.Set("LargeImageText", tbLargeImgText.Text);
            settings.Set("SmallImage", SmallImg);
            settings.Set("SmallImageText", tbSmallImgText.Text);
            settings.Set("Timestamp", sTimeStamp);
            var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            appSettings.AppSettings.Settings.Clear();
            for (int i = 0; i < settings.Count; i++)
            {
                appSettings.AppSettings.Settings.Add(settings.AllKeys[i], settings[i]);
            }
            appSettings.Save();
        }

        private void UpdateArg(object sender, string sendname)
        {
            lTitle.Content = (sender as Button).Content;
            tbArgument.Visibility = Visibility.Visible;
            bOK.Visibility = Visibility.Visible;
            this.sendname = sendname;
        }

        private void bUpdateConfig_Click(object sender, RoutedEventArgs e)
        {
            SaveStatus();
        }

        private void bUpdateRPC_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "AppID");
        }

        private void bUpdateLargeImg_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "LargeImg");
        }

        private void bUpdateSmallImg_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "SmallImg");
        }

        private void bUpdateTimestamp_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "Timestamp");
        }

        private void bSetSpectate_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "Spectate");
        }

        private void bSetAskToJoin_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "AskToJoin");
        }

        private void bOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lTitle.Content = "";
                tbArgument.Visibility = Visibility.Hidden;
                bOK.Visibility = Visibility.Hidden;
                var method = AppDomain.CurrentDomain.GetAssemblies()
    .Select(x => x.GetTypes())
    .SelectMany(x => x)
    .Where(x => x.Namespace == "CustomRPC")
    .Where(c => c.GetMethod(sendname) != null)
    .Select(c => c.GetMethod(sendname)).First();
                method.Invoke(null, new object[] { this });
                tbArgument.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
