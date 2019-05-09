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
            ContentFrame.Navigate(typeof(StatBoss.Pages.PageInstructions));
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

            if (DataAccess.NIDActualSeason != -1)
            {
                SqliteDataReader season = DataAccess.QueryDB("SELECT * FROM tbl_seasons WHERE nID='" + DataAccess.NIDActualSeason + "'");
                string sSeason = "";
                while (season.Read()) { sSeason = season.GetString(season.GetOrdinal("sName")); }
                ActualSeason.Content = "Season: " + sSeason;
            }

            if (DataAccess.NIDActualTeam != -1)
            {
                SqliteDataReader team = DataAccess.QueryDB("SELECT * FROM tbl_teams WHERE nID='" + DataAccess.NIDActualTeam + "'");
                string sTeam = "";
                while (team.Read()) { sTeam = team.GetString(team.GetOrdinal("sCategoryName")); }
                ActualTeam.Content = "Team: " + sTeam;
            }
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            switch (args.InvokedItem)
            {
                case "Instructions":
                    ContentFrame.Navigate(typeof(StatBoss.Pages.PageInstructions));
                    break;

                case "Players Stats":
                    ContentFrame.Navigate(typeof(StatBoss.Pages.PageOverallStats));
                    break;

                case "Players":
                    ContentFrame.Navigate(typeof(PagePlayers));
                    break;

                case "Matches":
                    ContentFrame.Navigate(typeof(Pages.PageMatches));
                    break;

                case "Stats in Matches":
                    ContentFrame.Navigate(typeof(Pages.PageMatchesEdit));
                    break;

                case "Seasons":
                    ContentFrame.Navigate(typeof(Pages.PageSeasons), ContentFrame);
                    break;

                case "Teams":
                    ContentFrame.Navigate(typeof(Pages.PageTeams));
                    break;

                case "Positions":
                    ContentFrame.Navigate(typeof(Pages.PagePositions));
                    break;

                case "Opponents":
                    ContentFrame.Navigate(typeof(Pages.PageOpponents));
                    break;

                case "About":
                    Classes.PageHandling.DialogsHandling.DisplayAbout();
                    break;
            }            
        }
    }
}
