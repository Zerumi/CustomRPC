using System;
using System.Windows;

namespace CustomRPC
{
    public static class Load
    {
        public static void lAppID(MainWindow window)
        {
            window.tbArgument.Text = window.client_id;
        }
        public static void lLargeImg(MainWindow window)
        {
            window.tbArgument.Text = window.LargeImg;
        }
        public static void lSmallImg(MainWindow window)
        {
            window.tbArgument.Text = window.SmallImg;
        }
        public static void lTimestamp(MainWindow window)
        {
            window.tbArgument.Text = window.TimeStamp.ToLocalTime().ToString();
        }
        public static void lPartyID(MainWindow window)
        {
            window.tbArgument.Text = window.PartyID;
        }
        public static void lSpectate(MainWindow window)
        {
            window.tbArgument.Text = window.SpectateCode;
        }
        public static void lAskToJoin(MainWindow window)
        {
            window.tbArgument.Text = window.AskToJoinCode;
        }
    }
}
