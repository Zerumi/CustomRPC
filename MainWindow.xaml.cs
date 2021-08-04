using System;
using System.Linq;
using System.Windows;
using DiscordRPC;
using DiscordRPC.Message;
using System.Windows.Input;
using System.Windows.Threading;
using System.Configuration;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace CustomRPC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IUserInfo userInfo = new EnUserInfo();

        System.Windows.Controls.MenuItem[] menuItems { get; set; }

        public MainWindow()
        {
            notifyIcon1.Visible = true;
            notifyIcon1.ContextMenu = new ContextMenu();
            MenuItem ExitItem = new MenuItem() { Text = userInfo.Exit() };
            ExitItem.Click += ExitItem_Click;
            notifyIcon1.ContextMenu.MenuItems.Add(ExitItem);
            notifyIcon1.DoubleClick += NotifyIcon1_MouseDoubleClick;
            InitializeComponent();
            linktoapp.NavigateUri = new Uri($@"https://discord.com/developers/applications/{client_id}");
            menuItems = new System.Windows.Controls.MenuItem[] { mlEng, mlRus };
            menuItems[userInfo.LangIndex].IsChecked = true;
            Load();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 1);
            LoadStatus();
            UpdatePresence(TimeStamp, tbDetails.Text, tbState.Text, LargeImg, tbLargeImgText.Text, SmallImg, tbSmallImgText.Text);
        }

        private DateTime? latestDateTime = null;

        private void LoadTranslate()
        {
            mRPC.Header = userInfo.UImRPC();
            mRPCappID.Header = userInfo.UImRPCappID();
            mRPClargeIMG.Header = userInfo.UImRPClargeIMG();
            mRPCsmallIMG.Header = userInfo.UImRPCsmallIMG();
            mRPCtimestamp.Header = userInfo.UImRPCtimestamp();
            mRPCpartyID.Header = userInfo.UImRPCpartyID();
            mRPCspecCD.Header = userInfo.UImRPCspecCD();
            mRPCask2joinCD.Header = userInfo.UImRPCask2joinCD();
            mConfig.Header = userInfo.UImConfig();
            mcLoad.Header = userInfo.UImcLoad();
            mcSave.Header = userInfo.UImcSave();
            mHelp.Header = userInfo.UImHelp();
            miGameNameHelp.Header = userInfo.UImGameNameHelp();
            miImgChange.Header = userInfo.UImImgChange();
            miPSize.Header = userInfo.UImPSize();
            miTimestFormat.Header = userInfo.UImTimestFormat();
            mLang.Header = userInfo.UImLang();
            bApply.Content = userInfo.UIbApply();
            updatelabel.Content = userInfo.LatestStatusUpdate() + " " + (latestDateTime?.ToString() ?? userInfo.TimeNull());
            hlDDP.Inlines.Clear();
            linktoapp.Inlines.Clear();
            hlDDP.Inlines.Add(userInfo.UIhlDDP());
            linktoapp.Inlines.Add(userInfo.UIhlLinktoapp());
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        static readonly System.Collections.Specialized.NameValueCollection settings = ConfigurationManager.AppSettings;

        public string client_id = settings.Get("ApplicationID");

        readonly DispatcherTimer timer = new DispatcherTimer();

        public DiscordRpcClient client;

        public DateTime TimeStamp = default;

        public string sTimeStamp = null;

        public string LargeImg = settings.Get("LargeImage");

        public string SmallImg = settings.Get("SmallImage");

        public string PartyID = settings.Get("PartyID");

        public string SpectateCode = settings.Get("Spectate");

        public string AskToJoinCode = settings.Get("AskToJoin");

        public bool isStartTime = true;

        private string sendname = null;

        RichPresence presence = new RichPresence();

        private void UpdatePresence(DateTime Timestamp, string details = "", string state = "", string LargeImg = "", string LargeImgText = "", string SmallImg = "", string SmallImgText = "", string Party = "", string Spectate = "", string AskToJoin = "")
        {
            client.RegisterUriScheme(null, PartyID);

            client.RegisterUriScheme(null, SpectateCode);

            client.RegisterUriScheme(null, AskToJoinCode);

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
                Party = new Party()
                {
                    ID = Party,
                    Size = Int32.ParseOrDefault(partySize.Text),
                    Max = Int32.ParseOrDefault(partySizeMax.Text)
                },
                Secrets = new Secrets()
                {
                    JoinSecret = AskToJoin,
                    SpectateSecret = Spectate
                }
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

        public void Load()
        {
            client = new DiscordRpcClient(client_id)
            {
                SkipIdenticalPresence = true,
            };

            client.OnReady += OnReady;

            client.OnError += Client_OnError;

            client.OnConnectionFailed += (_, __) =>
            {
                client.Deinitialize();
                System.Windows.MessageBox.Show(userInfo.ClientDisconnected() + " " + __.FailedPipe);
            };

            client.Initialize();
        }

        private void Client_OnError(object sender, ErrorMessage args)
        {
            System.Windows.MessageBox.Show(userInfo.ClientError() + "\n" + args.Message);
        }

        private void OnReady(object _, ReadyMessage __)
        {
            timer.Start();
            UpdateStatus();
        }

        private async void UpdateStatus()
        {
            if (!client.IsInitialized)
                return;

            client.SetPresence(presence);

            latestDateTime = DateTime.Now;

            await Dispatcher.BeginInvoke(new Action(() => {
                updatelabel.Content = userInfo.LatestStatusUpdate() + " " + latestDateTime;
            }));
        }

        bool? iselapsed = null;
        private void TimerTick(object sender, EventArgs e)
        {
            timelabel.Content = DateTime.Now.ToString();
            if (DateTime.UtcNow.Subtract(TimeStamp).TotalSeconds > 0)
            {
                if (!iselapsed.HasValue)
                {
                    iselapsed = true;
                    presence.Timestamps = new Timestamps()
                    {
                        Start = TimeStamp
                    };
                    UpdateStatus();
                }
                else if (!iselapsed.Value) // remaining
                {
                    iselapsed = true;
                    presence.Timestamps = new Timestamps()
                    {
                        Start = TimeStamp
                    };
                    UpdateStatus();
                }
                lTimestamp.Content = $"{(Convert.ToInt32(DateTime.UtcNow.Subtract(TimeStamp).Hours) == 0 ? Convert.ToString(Convert.ToInt32(DateTime.UtcNow.Subtract(TimeStamp).Minutes)) : Convert.ToString(Convert.ToString(Convert.ToInt32(DateTime.UtcNow.Subtract(TimeStamp).Hours))) + ":" + Convert.ToString(Convert.ToInt32(DateTime.UtcNow.Subtract(TimeStamp).Minutes)))}:{Convert.ToInt32(DateTime.UtcNow.Subtract(TimeStamp).Seconds)} elapsed";
            }
            else
            {
                if (!iselapsed.HasValue)
                {
                    iselapsed = false;
                    presence.Timestamps = new Timestamps()
                    {
                        End = TimeStamp
                    };
                    UpdateStatus();
                }
                else if (iselapsed.Value) // elapsed
                {
                    iselapsed = false;
                    presence.Timestamps = new Timestamps()
                    {
                        End = TimeStamp
                    };
                    UpdateStatus();
                }
                lTimestamp.Content = $"{(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Hours) == 0 ? Convert.ToString(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Minutes)) : Convert.ToString(Convert.ToString(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Hours))) + ":" + Convert.ToString(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Minutes)))}:{Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Seconds)} left";
            }
        }

        private void UIbApply_Click(object sender, RoutedEventArgs e)
        {
            UpdatePresence(TimeStamp, tbDetails.Text, tbState.Text, LargeImg, tbLargeImgText.Text, SmallImg, tbSmallImgText.Text, PartyID, SpectateCode, AskToJoinCode);
            UpdateStatus();
        }

        private void UIbLoadStatus_Click(object sender, RoutedEventArgs e)
        {
            LoadStatus();
        }

        private void LoadStatus()
        {
            lGName.Content = "ApplicationID: " + settings.Get("ApplicationID");
            tbDetails.Text = settings.Get("Details");
            tbState.Text = settings.Get("State");
            tbSmallImgText.Text = settings.Get("SmallImageText");
            tbLargeImgText.Text = settings.Get("LargeImageText");
            LargeImg = settings.Get("LargeImage");
            SmallImg = settings.Get("SmallImage");
            sTimeStamp = settings.Get("Timestamp");
            TimeStamp = DateTime.Parse(sTimeStamp).ToUniversalTime();
            PartyID = settings.Get("PartyID");
            partySize.Text = settings.Get("PartySize");
            partySizeMax.Text = settings.Get("PartySizeMax");
            SpectateCode = settings.Get("Spectate");
            AskToJoinCode = settings.Get("AskToJoin");
        }

        private void SaveStatus()
        {
            settings.Set("ApplicationID", client_id);
            settings.Set("Details", tbDetails.Text);
            settings.Set("State", tbState.Text);
            settings.Set("LargeImage", LargeImg);
            settings.Set("LargeImageText", tbLargeImgText.Text);
            settings.Set("SmallImage", SmallImg);
            settings.Set("SmallImageText", tbSmallImgText.Text);
            settings.Set("Timestamp", sTimeStamp);
            settings.Set("PartyID", PartyID);
            settings.Set("PartySize", partySize.Text);
            settings.Set("PartySizeMax", partySizeMax.Text);
            settings.Set("Spectate", SpectateCode);
            settings.Set("AskToJoin", AskToJoinCode);
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
            lTitle.Content = (sender as System.Windows.Controls.MenuItem).Header;
            var method = AppDomain.CurrentDomain.GetAssemblies()
    .Select(x => x.GetTypes())
    .SelectMany(x => x)
    .Where(x => x.Namespace == "CustomRPC")
    .Where(c => c.GetMethod("l" + sendname) != null)
    .Select(c => c.GetMethod("l" + sendname)).First();
            method.Invoke(null, new object[] { this });
            tbArgument.Visibility = Visibility.Visible;
            bOK.Visibility = Visibility.Visible;
            this.sendname = sendname;
        }

        private void UIbUpdateConfig_Click(object sender, RoutedEventArgs e)
        {
            SaveStatus();
        }

        private void UIbUpdateRPC_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "AppID");
        }

        private void UIbUpdateLargeImg_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "LargeImg");
        }

        private void UIbUpdateSmallImg_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "SmallImg");
        }

        private void UIbUpdateTimestamp_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "Timestamp");
        }

        private void UIbUpdatePartyID_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "PartyID");
        }

        private void UIbSetSpectate_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "Spectate");
        }

        private void UIbSetAskToJoin_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "AskToJoin");
        }

        private void UIbOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lTitle.Content = string.Empty;
                tbArgument.Visibility = Visibility.Hidden;
                bOK.Visibility = Visibility.Hidden;
                var method = AppDomain.CurrentDomain.GetAssemblies()
    .Select(x => x.GetTypes())
    .SelectMany(x => x)
    .Where(x => x.Namespace == "CustomRPC")
    .Where(c => c.GetMethod(sendname) != null)
    .Select(c => c.GetMethod(sendname)).First();
                method.Invoke(null, new object[] { this });
                tbArgument.Text = string.Empty;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private readonly NotifyIcon notifyIcon1 = new NotifyIcon()
        {
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location)
        };

        private void NotifyIcon1_MouseDoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        struct Int32
        {
            public static int ParseOrDefault(string s)
            {
                if (int.TryParse(s, out int result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void UItbArgument_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    lTitle.Content = string.Empty;
                    tbArgument.Visibility = Visibility.Hidden;
                    bOK.Visibility = Visibility.Hidden;
                    var method = AppDomain.CurrentDomain.GetAssemblies()
        .Select(x => x.GetTypes())
        .SelectMany(x => x)
        .Where(x => x.Namespace == "CustomRPC")
        .Where(c => c.GetMethod(sendname) != null)
        .Select(c => c.GetMethod(sendname)).First();
                    method.Invoke(null, new object[] { this });
                    tbArgument.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.ToString());
                }
            }
        }

        private void UImiGameNameHelp_Click(object sender, RoutedEventArgs e)
        {
            userInfo.MBmiGameNameHelp();
        }

        private void UImiPSize_Click(object sender, RoutedEventArgs e)
        {
            userInfo.MBmiPSize();
        }

        private void UImiImgChange_Click(object sender, RoutedEventArgs e)
        {
            userInfo.MBmiImgChange();
        }

        private void UImiTimestFormat_Click(object sender, RoutedEventArgs e)
        {
            userInfo.MBmiTimestFormat();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void mlEng_Checked(object sender, RoutedEventArgs e)
        {
            menuItems[userInfo.LangIndex].IsChecked = false;
            userInfo = new EnUserInfo();
            LoadTranslate();
        }

        private void mlEng_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void mlRus_Checked(object sender, RoutedEventArgs e)
        {
            menuItems[userInfo.LangIndex].IsChecked = false;
            userInfo = new RuUserInfo();
            LoadTranslate();
        }

        private void mlRus_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}