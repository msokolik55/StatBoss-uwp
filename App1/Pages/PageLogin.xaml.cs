using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StatBoss.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageLogin : Page
    {
        public PageLogin()
        {
            this.InitializeComponent();
        }

        private void ButtSign_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUser(TextBoxLogin.Text, TextBoxPassword.Text))
            {
                App1.DataAccess.sDBName = GetDBName(TextBoxLogin.Text, TextBoxPassword.Text);

                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(App1.MainPage));
            }
        }

        private SqliteDataReader QueryDB(string sCommand)
        {
            string sDBName = "users.db";

            using (SqliteConnection db = new SqliteConnection("Filename=" + sDBName))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand(sCommand, db);
                SqliteDataReader query = selectCommand.ExecuteReader();

                db.Close();

                return query;
            }
        }

        private bool CheckUser(string sLogin, string sPassword)
        {
            string sCommand = "SELECT * FROM tbl_users WHERE sLogin='" + sLogin + "' AND sPassword='" + sPassword + "'";

            SqliteDataReader users = QueryDB(sCommand);
            return users.HasRows;
        }

        private string GetDBName(string sLogin, string sPassword)
        {
            string sCommand = "SELECT * FROM tbl_users WHERE sLogin='" + sLogin + "' AND sPassword='" + sPassword + "'";

            SqliteDataReader user = QueryDB(sCommand);
            while (user.Read()) { return user.GetString(user.GetOrdinal("sDBName")); }

            return "";
        }
    }

    class User
    {
        public string sLogin;
        public string sPassword;

        public User()
        {
        }
    }
}
