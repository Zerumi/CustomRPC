using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomRPC
{
    static class Update
    {
        public static void AppID(MainWindow window)
        {
            window.client_id = window.tbArgument.Text;
            MessageBox.Show("Для применения изменений перезагрузите программу (сохраните конфигурацию)", "Custom RPC", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
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
            
        }
        public static void Spectate(MainWindow window)
        {
            
        }
        public static void AskToJoin(MainWindow window)
        {

        }
    }
}
