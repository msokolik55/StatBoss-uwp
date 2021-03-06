﻿using App1;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StatBoss.Classes
{
    public static class MainVariables
    {
        private static int nIDActualSeason;
        public static int NIDActualSeason
        {
            get { return nIDActualSeason; }
            set
            {
                nIDActualSeason = value;
                ShowActualSeason();
            }
        }

        private static int nIDActualTeam;
        public static int NIDActualTeam
        {
            get { return nIDActualTeam; }
            set
            {
                nIDActualTeam = value;
                ShowActualTeam();
            }
        }

        public static void InitializeMainVariables()
        {
            try
            {
                NIDActualSeason = DataAccess.GetMaxID("tbl_seasons", false);
                NIDActualTeam = DataAccess.GetMaxID("tbl_teams");
            }
            catch (Exception)
            {
            }
        }

        public static void ShowActualSeason(NavigationViewItem nvItem = null)
        {
            string sCommand = "SELECT * FROM tbl_seasons WHERE nID='" + nIDActualSeason + "'";
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            string sName = "";
            while (query.Read())
            {
                sName = query.GetString(query.GetOrdinal("sName"));
            }

            try { nvItem.Content = "Season: " + sName; }
            catch(Exception)
            {
                try
                {
                    Frame contentFrame = Window.Current.Content as Frame;
                    MainPage mp = contentFrame.Content as MainPage;
                    Grid grid = mp.Content as Grid;
                    nvItem = grid.FindName("ActualSeason") as NavigationViewItem;

                    string content = (sName != "") ? sName : "Add season";
                    nvItem.Content = "Season: " + content;
                }
                catch (Exception)
                {
                }
            }
        }

        public static void ShowActualTeam(NavigationViewItem nvItem = null)
        {
            string sCommand = "SELECT * FROM tbl_teams WHERE nID='" + nIDActualTeam + "' AND nIDSeason='" + nIDActualSeason + "'";
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            string sName = "";
            while (query.Read())
            {
                sName = query.GetString(query.GetOrdinal("sCategoryName"));
            }

            try { nvItem.Content = "Team: " + sName; }
            catch (Exception)
            {
                try
                {
                    Frame contentFrame = Window.Current.Content as Frame;
                    MainPage mp = contentFrame.Content as MainPage;
                    Grid grid = mp.Content as Grid;
                    nvItem = grid.FindName("ActualTeam") as NavigationViewItem;

                    string content = (sName != "") ? sName : "Add team";
                    nvItem.Content = "Team: " + content;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}
