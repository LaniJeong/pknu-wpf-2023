using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Threading.Tasks;
using System.Windows;


namespace project_CulturalProperties.Logics
{
    internal class Commons
    {
        //MySQL용
        public static readonly string myConnString = "Server=localhost;" +
                                                    "Port=3306;" +
                                                    "Database=miniproject;" +
                                                    "Uid=root;" +
                                                    "Pwd=12345;";

        public static async Task<MessageDialogResult> ShowMessageAsync(string title, string message,
            MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(title, message, style, null);
        }
    }
}
