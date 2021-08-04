namespace CustomRPC
{
    public class RuUserInfo : IUserInfo
    {
        public int LangIndex { get { return 1; } }

        public string ClientError()
        {
            return "Что-то пошло не так...";
        }

        public string ClientDisconnected()
        {
            return "Клиент разорвал соединение:";
        }

        public string Exit()
        {
            return "Выход";
        }

        public string LatestStatusUpdate()
        {
            return "Последнее обновление статуса на стороне Discord:";
        }

        public void MBmiGameNameHelp()
        {
            _ = System.Windows.Forms.MessageBox.Show("Из-за ограничений Discord, чтобы изменить название игры, вам надо создать свое приложение на их портале разработчиков (https://discord.com/developers/applications/).\n" +
                "1) Создайте приложение, нажав сверху \"New Application\", введите название (это и будет название вашей игры) или вы можете взять существующее приложение, если у вас такое есть.\n" +
                "2) Далее вам нужно будет скопировать поле \"Client ID\". В программе, выберите \"Статус -> Обновить ID приложения\" и вставьте недавно скопированный ID.\n" +
                "3) Перезагрузите программу.");
        }

        public void MBmiImgChange()
        {
            _ = System.Windows.Forms.MessageBox.Show("Из-за ограничений Discord, чтобы изменить большое и маленькое изображение в статусе, вам надо изменить свое приложение на их портале разработчиков (https://discord.com/developers/applications/).\n" +
                "1) Откройте свое приложение на портале(как его создать, см.раздел \"Как изменить название игры?\").\n" +
                "2) Слева в списке выберете пункт \"Rich Presence\".\n" +
                "3) Промотайте страницу вниз, найдите раздел \"Rich Presence Assets\". Ниже нажмите кнопку \"Add Image(s)\"\n" +
                "4) Выберете изображение на вашем компьютере (ВАЖНО! Изображение должно быть по размеру больше чем 512x512)\n" +
                "5) Выберете название изображения (Вы не сможете поменять название после сохранения)\n" +
                "6) В программе выберите \"Статус -> Обновить (малую) картинку\" и введите туда название изображения, которое вы загрузили. Вы также можете ввести название того изображения которое уже загрузили до этого.\n" +
                "/ ! \\ Обработка изображения может занять несколько часов");
        }

        public void MBmiPSize()
        {
            _ = System.Windows.Forms.MessageBox.Show("Для изменения размера группы, вам нужно дать знать Discord'у, что такая группа существует.\n" +
                "1) В программе выберите \"RPC -> Изменить ID группы\" и в поле введите любое значение. Подтвердите, нажав Enter/Ок.\n" +
                "2) Теперь вы можете изменять размер группы в основном окне.");
        }

        public void MBmiTimestFormat()
        {
            _ = System.Windows.Forms.MessageBox.Show("При синтаксическом анализе преобразуется строковое представление даты и времени в DateTime значение. Как правило, строки даты и времени имеют два разных варианта использования в приложениях:\n" +
                "Дата и время принимают различные формы и отражают соглашения о текущей культуре или определенной культуре. Например, приложение позволяет пользователю, имеющему текущий язык и региональные параметры en - US, ввести значение даты \"12/15/2013\" или \"15 декабря 2013\". Он позволяет пользователю, чей текущий язык и региональные параметры — en - GB, ввести значение даты \"15/12/2013\" или \"15 декабря 2013\".\n" +
                "Дата и время представлены в заранее определенном формате. Например, приложение сериализует дату как \"20130103\" независимо от языка и региональных параметров, на которых выполняется приложение. Для приложения может требоваться ввод дат в кратком формате даты текущего языка и региональных параметров.");
        }

        public string UImRPC()
        {
            return "Статус";
        }

        public string UImRPCappID()
        {
            return "Обновить ID приложения";
        }

        public string UImRPClargeIMG()
        {
            return "Обновить картинку";
        }

        public string UImRPCsmallIMG()
        {
            return "Обновить малую картинку";
        }

        public string UImRPCtimestamp()
        {
            return "Обновить отметку времени";
        }

        public string UImRPCpartyID()
        {
            return "Обновить ID группы";
        }

        public string UImRPCspecCD()
        {
            return "Обновить код наблюдения";
        }

        public string UImRPCask2joinCD()
        {
            return "Обновить код вступления";
        }

        public string UImConfig()
        {
            return "Конфигурация";
        }

        public string UImcSave()
        {
            return "Сохранить конфигурацию";
        }

        public string UImcLoad()
        {
            return "Загрузить конфигурацию";
        }

        public string UImHelp()
        {
            return "Помощь";
        }

        public string UImGameNameHelp()
        {
            return "Как изменить название игры?";
        }

        public string UImPSize()
        {
            return "Почему я не могу изменить размер группы?";
        }

        public string UImImgChange()
        {
            return "Как изменить картинку?";
        }

        public string UImTimestFormat()
        {
            return "Какой формат даты и времени поддерживается?";
        }

        public string UIbApply()
        {
            return "Применить";
        }

        public string TimeNull()
        {
            return "Не обновлялось";
        }

        public string UIhlDDP()
        {
            return "*Портал разработчиков*";
        }

        public string UIhlLinktoapp()
        {
            return "*Если это Ваше приложение, ссылка*";
        }

        public string UImLang()
        {
            return "Язык";
        }
    }
}