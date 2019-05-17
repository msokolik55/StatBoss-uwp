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

using Microsoft.Data.Sqlite;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            InitializeGlobalVariables();
            StatBoss.Classes.PageHandling.NavigationHandling.InitializateNavigation(ContentFrame);
        }

        public void InitializeGlobalVariables()
        {
            try
            {
                DataAccess.NIDActualSeason = DataAccess.GetMaxID("tbl_seasons", false);
                DataAccess.NIDActualTeam = DataAccess.GetMaxID("tbl_teams");
            }
            catch (Exception)
            {
            }

            if (DataAccess.NIDActualSeason != 0)
            {
                SqliteDataReader season = DataAccess.QueryDB("SELECT * FROM tbl_seasons WHERE nID='" + DataAccess.NIDActualSeason + "'");
                string sSeason = "";
                while (season.Read()) { sSeason = season.GetString(season.GetOrdinal("sName")); }
                ActualSeason.Content = "Season: " + sSeason;
            }
            else
            {
                ActualSeason.Content = "Season: Add season";
            }

            if (DataAccess.NIDActualTeam != 0)
            {
                SqliteDataReader team = DataAccess.QueryDB("SELECT * FROM tbl_teams WHERE nID='" + DataAccess.NIDActualTeam + "'");
                string sTeam = "";
                while (team.Read()) { sTeam = team.GetString(team.GetOrdinal("sCategoryName")); }
                ActualTeam.Content = "Team: " + sTeam;
            }
            else
            {
                ActualTeam.Content = "Team: Add team";
            }
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            StatBoss.Classes.PageHandling.NavigationHandling.NavigateTo(args, ContentFrame, NavMain);
        }
    }
}
