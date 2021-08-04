using System;

namespace CustomRPC
{
    public class EnUserInfo : IUserInfo
    {
        public int LangIndex { get { return 0; } }

        public string ClientError()
        {
            return "Something went wrong...";
        }

        public string ClientDisconnected()
        {
            return "A client has disconnected:";
        }

        public string LatestStatusUpdate()
        {
            return "Latest status update on the Discord side:";
        }

        public string Exit()
        {
            return "Exit";
        }

        public void MBmiGameNameHelp()
        {
            _ = System.Windows.Forms.MessageBox.Show("Due to Discord restrictions, in order to change the name of the game, you need to create your application on their developer portal (https://discord.com/developers/applications/).\n" +
                "1) Create an application. Go to the discord developer portal hyperlink, click \"New Application\" at the top, enter a name(this will be the name of your game) or you can take an existing application if you have one.\n" +
                "2) Next, you will need to copy the \"Client ID\" field. In the program, select \"RPC -> Update Application ID\" and paste the recently copied ID.\n" +
                "3) Restart the program.");
        }

        public void MBmiPSize()
        {
            _ = System.Windows.Forms.MessageBox.Show("For change party size, you need to have this party.\n" +
                "1) In the program select \"RPC -> Update PartyID\" and write something to field. Press Enter/Ok.\n" +
                "2) Now you can change party size");
        }

        public void MBmiImgChange()
        {
            _ = System.Windows.Forms.MessageBox.Show("Due to Discord restrictions, in order to change the large and small status image, you need to change your app on their developer portal (https://discord.com/developers/applications/).\n" +
                "1) Open your application on the dev portal(how to create it, see the section \"How to change game name?\").\n" +
                "2) On the left in the list, select the item \"Rich Presence\".\n" +
                "3) Scroll down the page to find the \"Rich Presence Assets\" section.Below click the \"Add Image (s)\"\n" +
                "3) Select an image on your computer(IMPORTANT!The image must be larger than 512x512)\n" +
                "4) Select the name of the image(you cannot change the name after saving)\n" +
                "5) In the program select \"RPC -> Update Large / Small Image\" and enter the name of the image that you uploaded there. You can also enter the name of the image that has already been uploaded before." +
                "/ ! \\ Image processing may take up several hours");
        }

        public void MBmiTimestFormat()
        {
            _ = System.Windows.Forms.MessageBox.Show("Parsing converts the string representation of a date and time to a DateTime value. Typically, date and time strings have two different usages in applications:\n\n" +
                "A date and time takes a variety of forms and reflects the conventions of either the current culture or a specific culture. For example, an application allows a user whose current culture is en - US to input a date value as \"12/15/2013\" or \"December 15, 2013\". It allows a user whose current culture is en - gb to input a date value as \"15/12/2013\" or \"15 December 2013.\"\n\n" +
                "A date and time is represented in a predefined format.For example, an application serializes a date as \"20130103\" independently of the culture on which the app is running. An application may require dates be input in the current culture's short date format.");
        }

        public string UImRPC()
        {
            return "RPC";
        }

        public string UImRPCappID()
        {
            return "Update ApplicationID";
        }

        public string UImRPClargeIMG()
        {
            return "Update Large Image";
        }

        public string UImRPCsmallIMG()
        {
            return "Update Small Image";
        }

        public string UImRPCtimestamp()
        {
            return "Update Timestamp";
        }

        public string UImRPCpartyID()
        {
            return "Update PartyID";
        }

        public string UImRPCspecCD()
        {
            return "Update Spectate code";
        }

        public string UImRPCask2joinCD()
        {
            return "Update Ask To Join code";
        }

        public string UImConfig()
        {
            return "Config";
        }

        public string UImcSave()
        {
            return "Save Status";
        }

        public string UImcLoad()
        {
            return "Load Status";
        }

        public string UImHelp()
        {
            return "Help";
        }

        public string UImGameNameHelp()
        {
            return "How to change game name?";
        }

        public string UImPSize()
        {
            return "Why i can't change party size?";
        }

        public string UImImgChange()
        {
            return "How to change large and small images?";
        }

        public string UImTimestFormat()
        {
            return "What date-time format is accepted to write?";
        }

        public string UIbApply()
        {
            return "Apply changes";
        }

        public string TimeNull()
        {
            return "Not updated";
        }

        public string UIhlDDP()
        {
            return "*Discord developer portal*";
        }

        public string UIhlLinktoapp()
        {
            return "*If you own application, you can click here*";
        }

        public string UImLang()
        {
            return "Language";
        }
    }
}
