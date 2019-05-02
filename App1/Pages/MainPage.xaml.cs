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
            ContentFrame.Navigate(typeof(Pages.Page1));
        }

        //private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        //{
        //    MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        //}

        //private void RadioButtonPaneItem_Click(object sender, RoutedEventArgs e)
        //{
        //    var radioButton = sender as RadioButton;

        //    if (radioButton != null)
        //    {
        //        switch (radioButton.Tag.ToString())
        //        {
        //            case "Map":
        //                MainFrame.Navigate(typeof(Page2));
        //                break;

        //            case "Mail":
        //                MainFrame.Navigate(typeof(Pages.Page1));
        //                break;

        //            case "AddMatch":
        //                MainFrame.Navigate(typeof(Pages.PageMatches));
        //                break;

        //            case "Stats":
        //                MainFrame.Navigate(typeof(Pages.PageMatchesEdit));
        //                break;

        //            case "AddPlayer":
        //                MainFrame.Navigate(typeof(Page3));
        //                break;

        //            case "PlayerDetails":
        //                MainFrame.Navigate(typeof(Pages.PagePlayersShow));
        //                break;

        //            case "Season":
        //                MainFrame.Navigate(typeof(Pages.Page4), MainFrame);
        //                break;

        //            case "Team":
        //                MainFrame.Navigate(typeof(Pages.Page5));
        //                break;

        //            case "Position":
        //                MainFrame.Navigate(typeof(Pages.PagePositions));
        //                break;

        //            case "Opponent":
        //                MainFrame.Navigate(typeof(Pages.PageOpponents));
        //                break;
        //        }
        //    }
        //}

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
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(Pages.Page1));
            }
            else
            {
                switch (args.InvokedItem)
                {
                    case "Players Stats":
                        ContentFrame.Navigate(typeof(Pages.PagePlayersStats));
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
                }
            }
        }
    }

}
