using System;
using System.Linq;
using System.Windows;
using DiscordRPC;
using DiscordRPC.Message;
using System.Windows.Input;
using System.Windows.Threading;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;

namespace CustomRPC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            notifyIcon1.Visible = true;
            notifyIcon1.ContextMenu = new System.Windows.Forms.ContextMenu();
            var ExitItem = new System.Windows.Forms.MenuItem() { Text = "Exit" };
            ExitItem.Click += ExitItem_Click;
            notifyIcon1.ContextMenu.MenuItems.Add(ExitItem);
            notifyIcon1.DoubleClick += notifyIcon1_MouseDoubleClick;
            InitializeComponent();
            linktoapp.NavigateUri = new Uri($@"https://discord.com/developers/applications/{client_id}");
            load();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 1);
            LoadStatus();
            UpdatePresence(TimeStamp, tbDetails.Text, tbState.Text, LargeImg, tbLargeImgText.Text, SmallImg, tbSmallImgText.Text);
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        static System.Collections.Specialized.NameValueCollection settings = ConfigurationManager.AppSettings;

        public string client_id = settings.Get("ApplicationID");

        DispatcherTimer timer = new DispatcherTimer();

        public DiscordRpcClient client;

        public DateTime TimeStamp = default;

        public string sTimeStamp = null;

        public string LargeImg = settings.Get("LargeImage");

        public string SmallImg = settings.Get("SmallImage");

        public string PartyID = settings.Get("PartyID");

        public string SpectateCode = settings.Get("Spectate");

        public string AskToJoinCode = settings.Get("AskToJoin");

        public bool isStartTime = true;

        string sendname = null;

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

        public void load()
        {
            client = new DiscordRpcClient(client_id)
            {
                SkipIdenticalPresence = true,
            };

            client.OnReady += onReady;

            client.OnError += Client_OnError;

            client.OnConnectionFailed += (_, __) =>
            {
                client.Deinitialize();
                System.Windows.MessageBox.Show("Клиент был отключен: " + __.FailedPipe);
            };

            client.Initialize();
        }

        private void Client_OnError(object sender, ErrorMessage args)
        {
            System.Windows.MessageBox.Show("Что-то пошло не так...\n" + args.Message);
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

            updatelabel.Content = $"Latest status update on the Discord side: {DateTime.Now}";
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
                    updateStatus();
                }
                else if (!iselapsed.Value) // remaining
                {
                    iselapsed = true;
                    presence.Timestamps = new Timestamps()
                    {
                        Start = TimeStamp
                    };
                    updateStatus();
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
                    updateStatus();
                }
                else if (iselapsed.Value) // elapsed
                {
                    iselapsed = false;
                    presence.Timestamps = new Timestamps()
                    {
                        End = TimeStamp
                    };
                    updateStatus();
                }
                lTimestamp.Content = $"{(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Hours) == 0 ? Convert.ToString(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Minutes)) : Convert.ToString(Convert.ToString(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Hours))) + ":" + Convert.ToString(Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Minutes)))}:{Convert.ToInt32(TimeStamp.Subtract(DateTime.UtcNow).Seconds)} left";
            }
        }

        private void bApply_Click(object sender, RoutedEventArgs e)
        {
            UpdatePresence(TimeStamp, tbDetails.Text, tbState.Text, LargeImg, tbLargeImgText.Text, SmallImg, tbSmallImgText.Text, PartyID, SpectateCode, AskToJoinCode);
            updateStatus();
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

        private void bUpdatePartyID_Click(object sender, RoutedEventArgs e)
        {
            UpdateArg(sender, "PartyID");
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

        private NotifyIcon notifyIcon1 = new NotifyIcon()
        {
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location)
        };

        private void notifyIcon1_MouseDoubleClick(object sender, EventArgs e)
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

        private void tbArgument_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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

        private void miGameNameHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Due to Discord restrictions, in order to change the name of the game, you need to create your application on their developer portal (https://discord.com/developers/applications/).\n" + 
                "1) Create an application. Go to the discord developer portal hyperlink, click \"New Application\" at the top, enter a name(this will be the name of your game) or you can take an existing application if you have one.\n" + 
                "2) Next, you will need to copy the \"Client ID\" field. In the program, select \"RPC -> Update Application ID\" and paste the recently copied ID.\n" + 
                "3) Restart the program.");
        }

        private void miPSize_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("For change party size, you need to have this party.\n" +
                "1) In the program select \"RPC -> Update PartyID\" and write something to field. Press Enter/Ok.\n" +
                "2) Now you can change party size");
        }

        private void miImgChange_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Due to Discord restrictions, in order to change the large and small status image, you need to change your app on their developer portal (https://discord.com/developers/applications/).\n" +
                "1) Open your application on the dev portal(how to create it, see the section \"How to change game name?\").\n" +
                "2) On the left in the list, select the item \"Rich Presence\".\n" +
                "3) Scroll down the page to find the \"Rich Presence Assets\" section.Below click the \"Add Image (s)\"\n" +
                "3) Select an image on your computer(IMPORTANT!The image must be larger than 512x512)\n" +
                "4) Select the name of the image(you cannot change the name after saving)\n" +
                "5) In the program select \"RPC -> Update Large / Small Image\" and enter the name of the image that you uploaded there. You can also enter the name of the image that has already been uploaded before.");
        }

        private void miTimestFormat_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Parsing converts the string representation of a date and time to a DateTime value. Typically, date and time strings have two different usages in applications:\n\n" +
                "A date and time takes a variety of forms and reflects the conventions of either the current culture or a specific culture. For example, an application allows a user whose current culture is en - US to input a date value as \"12/15/2013\" or \"December 15, 2013\". It allows a user whose current culture is en - gb to input a date value as \"15/12/2013\" or \"15 December 2013.\"\n\n" +
                "A date and time is represented in a predefined format.For example, an application serializes a date as \"20130103\" independently of the culture on which the app is running. An application may require dates be input in the current culture's short date format.");
        }

        #region RuHelp
        //        private void RuGameNameHelp()
        //        {
        //            System.Windows.Forms.MessageBox.Show("Из-за ограничений Discord, чтобы изменить название игры, вам надо создать свое приложение на их портале разработчиков (https://discord.com/developers/applications/).
        //1) Создайте приложение, нажав сверху "New Application", введите название(это и будет название вашей игры) или вы можете взять существующее приложение если у вас такое есть.
        //2) Далее вам нужно будет скопировать поле "Client ID".В программе, выберите "RPC -> Update Application ID" и вставьте недавно скопированный ID.
        //3) Перезагрузите программу.");
        //        }

        //        private void RuPSize1()
        //        {
        //            System.Windows.Forms.MessageBox.Show("Test");
        //        }

        //        private void RuImgChange()
        //        {
        //            System.Windows.Forms.MessageBox.Show("Из-за ограничений Discord, чтобы изменить большое и маленькое изображение в статусе, вам надо изменить свое приложение на их портале разработчиков (https://discord.com/developers/applications/).
        //1) Откройте свое приложение на портале(как его создать, см.раздел "How to change game name?").
        //2) Слева в списке выберете пункт "Rich Presence".
        //3) Промотайте страницу вниз, найдите раздел "Rich Presence Assets".Ниже нажмите кнопку "Add Image(s)"
        //3) Выберете изображение на вашем компьютере(ВАЖНО! Изображение должно быть по размеру больше чем 512x512)
        //4) Выберете название изображения(Вы не сможете поменять название после сохранения)
        //5) В программе выберите "RPC -> Update Large/Small Image" и введите туда название изображения, которое вы загрузили.Вы также можете ввести название того изображения которое уже загрузили до этого.");
        //        }

        //        private void RuTimestFormat()
        //        {
        //            System.Windows.Forms.MessageBox.Show("При синтаксическом анализе преобразуется строковое представление даты и времени в DateTime значение. Как правило, строки даты и времени имеют два разных варианта использования в приложениях:
        //Дата и время принимают различные формы и отражают соглашения о текущей культуре или определенной культуре.Например, приложение позволяет пользователю, имеющему текущий язык и региональные параметры en - US, ввести значение даты "12/15/2013" или "15 декабря 2013".Он позволяет пользователю, чей текущий язык и региональные параметры — en - GB, ввести значение даты "15/12/2013" или "15 декабря 2013".
        //Дата и время представлены в заранее определенном формате.Например, приложение сериализует дату как "20130103" независимо от языка и региональных параметров, на которых выполняется приложение.Для приложения может требоваться ввод дат в кратком формате даты текущего языка и региональных параметров.");
        //        }
        #endregion

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}
