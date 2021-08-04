using System;
using System.Windows;

namespace CustomRPC
{
    public static class Update
    {
        public static void AppID(MainWindow window)
        {
            window.client_id = window.tbArgument.Text;
            MessageBox.Show("To apply the changes, restart the program (save the configuration)", "Custom RPC", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }
        public static void LargeImg(MainWindow window)
        {
            window.LargeImg = window.tbArgument.Text;
        }
        public static void SmallImg(MainWindow window)
        {
            window.SmallImg = window.tbArgument.Text;
        }
        public static void Timestamp(MainWindow window)
        {
            var arg = window.tbArgument.Text;
            try
            {
                window.TimeStamp = DateTime.Parse(arg).ToUniversalTime();
                window.sTimeStamp = arg;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void PartyID(MainWindow window)
        {
            window.PartyID = window.tbArgument.Text;
        }
        public static void Spectate(MainWindow window)
        {
            window.SpectateCode = window.tbArgument.Text;
        }
        public static void AskToJoin(MainWindow window)
        {
            window.AskToJoinCode = window.tbArgument.Text;
        }
    }
}
