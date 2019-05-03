using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App1
{
    public static class DataAccess
    {
        private static int nIDActualSeason = -1;
        public static int NIDActualSeason
        {
            get { return nIDActualSeason; }
            set
            {
                nIDActualSeason = value;

                string sCommand = "SELECT * FROM tbl_seasons WHERE nID='" + DataAccess.nIDActualSeason + "'";
                SqliteDataReader query = DataAccess.QueryDB(sCommand);

                string sName = "";
                while (query.Read())
                {
                    sName = query.GetString(query.GetOrdinal("sName"));
                }

                try
                {
                    Frame contentFrame = Window.Current.Content as Frame;
                    MainPage mp = contentFrame.Content as MainPage;
                    Grid grid = mp.Content as Grid;
                    NavigationViewItem nvItem = grid.FindName("ActualSeason") as NavigationViewItem;

                    nvItem.Content = "Season: " + sName;
                }
                catch (Exception)
                {
                }
            }
        }

        private static int nIDActualTeam = -1;
        public static int NIDActualTeam
        {
            get { return nIDActualTeam; }
            set
            {
                nIDActualTeam = value;

                string sCommand = "SELECT * FROM tbl_teams WHERE nID='" + DataAccess.nIDActualTeam + "'";
                SqliteDataReader query = DataAccess.QueryDB(sCommand);

                string sName = "";
                while (query.Read())
                {
                    sName = query.GetString(query.GetOrdinal("sCategoryName"));
                }

                try
                {
                    Frame contentFrame = Window.Current.Content as Frame;
                    MainPage mp = contentFrame.Content as MainPage;
                    Grid grid = mp.Content as Grid;
                    NavigationViewItem nvItem = grid.FindName("ActualTeam") as NavigationViewItem;

                    nvItem.Content = "Team: " + sName;
                }
                catch (Exception)
                {
                }
            }
        }

        public static void InitializeDatabase()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                List<string> sCreateTables = new List<string>
                {
                    "CREATE TABLE IF NOT EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                                                                      "Text_Entry NVARCHAR(2048) NULL)",

                    "CREATE TABLE IF NOT EXISTS tbl_users ( " +
                                    "nID	INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                    "sLogin	VARCHAR(20) NOT NULL, " +
                                    "sPassword	VARCHAR(20) NOT NULL, " +
                                    "sFirstName	VARCHAR(20), " +
                                    "sSurname	VARCHAR(50), " +
                                    "dInserted	DATETIME, " +
                                    "nIDInserted	SMALLINT(6), " +
                                    "dUpdated	DATETIME, " +
                                    "nIDUpdated	SMALLINT(6))",

                    "CREATE TABLE IF NOT EXISTS tbl_seasons ( " +
                                    "nID	INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                    "sName	VARCHAR(255) NOT NULL, " +
                                    "dInserted	DATETIME, " +
                                    "nIDInserted	SMALLINT(6), " +
                                    "dUpdated	DATETIME, " +
                                    "nIDUpdated	SMALLINT(6))",

                    "CREATE TABLE IF NOT EXISTS tbl_players ( " +
                                    "nID	INTEGER, " +
                                    "nIDSeason	SMALLINT(6) NOT NULL, " +
                                    "sFirstName	VARCHAR(100) NOT NULL, " +
                                    "sSurname	VARCHAR(100) NOT NULL, " +
                                    "dBirthday	DATE NOT NULL, " +
                                    "nIDUserTeam	SMALLINT(6) NOT NULL, " +
                                    "nMatches	SMALLINT(6) DEFAULT 0, " +
                                    "nMinutes	SMALLINT(6) DEFAULT 0, " +
                                    "nGoals	SMALLINT(6) DEFAULT 0, " +
                                    "nAssistance	SMALLINT(6) DEFAULT 0, " +
                                    "nPenalties	SMALLINT(6) DEFAULT 0, " +
                                    "nRedCards	TINYINT(4) DEFAULT 0, " +
                                    "nIDPosition	TINYINT(1) NOT NULL, " +
                                    "dInserted	DATETIME, " +
                                    "nIDInserted	SMALLINT(6), " +
                                    "dUpdated	DATETIME, " +
                                    "nIDUpdated	SMALLINT(6))",

                    "CREATE TABLE IF NOT EXISTS tbl_matches ( " +
                                    "nID	INTEGER, " +
                                    "nIDSeason	SMALLINT(6) NOT NULL, " +
                                    "bPlayed	TINYINT(4), " +
                                    "nIDUserTeam	SMALLINT(6) NOT NULL, " +
                                    "nIDOpponent	SMALLINT(6) NOT NULL, " +
                                    "dDatetime	DATETIME, " +
                                    "sMatchPlace	VARCHAR(255), " +
                                    "bHome	TINYINT(1), " +
                                    "nGive	TINYINT(4), " +
                                    "nReceived	TINYINT(4), " +
                                    "nGive1	TINYINT(4), " +
                                    "nReceived1	TINYINT(4), " +
                                    "nGive2	TINYINT(4), " +
                                    "nReceived2	TINYINT(4), " +
                                    "nGive3	TINYINT(4), " +
                                    "nReceived3	TINYINT(4), " +
                                    "sMatchDescription	TEXT, " +
                                    "dInserted	DATETIME, " +
                                    "nIDInserted	SMALLINT(6), " +
                                    "dUpdated	DATETIME, " +
                                    "nIDUpdated	SMALLINT(6))",

                    "CREATE TABLE IF NOT EXISTS tbl_opponents ( " +
                                    "nID	INTEGER, " +
                                    "nIDSeason	SMALLINT(6) NOT NULL, " +
                                    "sName	VARCHAR(255) NOT NULL, " +
                                    "dInserted	DATETIME, " +
                                    "nIDInserted	SMALLINT(6), " +
                                    "dUpdated	DATETIME, " +
                                    "nIDUpdated	SMALLINT(6))",

                    "CREATE TABLE IF NOT EXISTS tbl_positions ( " +
                                    "nID	INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                    "sName	VARCHAR(255) NOT NULL, " +
                                    "dInserted	DATETIME, " +
                                    "nIDInserted	SMALLINT(6), " +
                                    "dUpdated	DATETIME, " +
                                    "nIDUpdated	SMALLINT(6))",

                    "CREATE TABLE IF NOT EXISTS tbl_stats ( " +
                                    "nID	INTEGER, " +
                                    "nIDSeason	SMALLINT(4) NOT NULL, " +
                                    "nIDMatch	SMALLINT(4) NOT NULL, " +
                                    "nIDPosition	SMALLINT(6), " +
                                    "nNumber	TINYINT(4), " +
                                    "nIDPlayer	SMALLINT(4) NOT NULL, " +
                                    "nIDUserTeam	SMALLINT(6) NOT NULL, " +
                                    "nMinutes	TINYINT(4), " +
                                    "nGoals	TINYINT(4), " +
                                    "nAssistance	TINYINT(4), " +
                                    "nPenalties	TINYINT(4), " +
                                    "nRedCards	TINYINT(4), " +
                                    "dInserted	DATETIME, " +
                                    "nIDInserted	SMALLINT(6), " +
                                    "dUpdated	DATETIME, " +
                                    "nIDUpdated	SMALLINT(6))",

                    "CREATE TABLE IF NOT EXISTS tbl_teams ( " +
                                    "nID	INTEGER, " +
                                    "nIDSeason	SMALLINT(6) NOT NULL, " +
                                    "sCategoryName	VARCHAR(10) NOT NULL, " +
                                    "sName	VARCHAR(100) NOT NULL, " +
                                    "nYearFrom	SMALLINT(6) DEFAULT 0, " +
                                    "nYearTo	SMALLINT(6) DEFAULT 0, " +
                                    "dInserted	DATETIME, " +
                                    "nIDInserted	SMALLINT(6), " +
                                    "dUpdated	DATETIME, " +
                                    "nIDUpdated	SMALLINT(6))"
                };

                foreach (string sTableCommand in sCreateTables)
                {
                    SqliteCommand createTable = new SqliteCommand(sTableCommand, db);
                    createTable.ExecuteReader();
                }

                db.Close();
            }
        }

        public static SqliteDataReader QueryDB(string sCommand)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand(sCommand, db);
                SqliteDataReader query = selectCommand.ExecuteReader();

                db.Close();

                return query;
            }
        }

        public static void ExecDB(string sCommand)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                try
                {
                    SqliteCommand insertCommand = new SqliteCommand(sCommand, db);
                    insertCommand.ExecuteReader();

                }
                catch (SqliteException exception)
                {
                    DisplayException(exception.ToString());
                }
                db.Close();
            }
        }

        public static int GetMaxID(string sTableName, bool bSeason = true)
        {
            int nMaxID = 0;

            string sCommand = "SELECT MAX(nID) FROM " + sTableName;
            if(bSeason) { sCommand += " WHERE nIDSeason='" + DataAccess.nIDActualSeason + "'"; }

            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                try
                {
                    nMaxID = query.GetInt32(0);
                }
                catch (Exception)
                {
                    nMaxID = 0;
                }
            }

            return nMaxID;
        }

        public static string GetOpponent(int nID)
        {
            string sCommand = "SELECT * FROM tbl_opponents WHERE nID='" + nID + "' AND nIDSeason='" + DataAccess.NIDActualSeason + "'";
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            string sOpponent = "";
            while (query.Read())
            {
                sOpponent = query.GetString(query.GetOrdinal("sName"));
            }

            return sOpponent;
        }

        public static string GetPlayer(int nID)
        {
            string sCommand = "SELECT * FROM tbl_players WHERE nID='" + nID + "' AND nIDSeason='" + DataAccess.NIDActualSeason + "'";
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            string sPlayer = "";
            while (query.Read())
            {
                sPlayer = query.GetString(query.GetOrdinal("sSurname")) + " " + query.GetString(query.GetOrdinal("sFirstName"));
            }

            return sPlayer;
        }

        private static async void DisplayException(string exceptionText)
        {
            ContentDialog dialogException = new ContentDialog
            {
                Title = "SQLite Exception",
                Content = exceptionText,
                CloseButtonText = "OK"
            };

            ContentDialogResult result = await dialogException.ShowAsync();
        }

        public static void RemoveItem(string[] tables, string columnNameInDB, int id, string tableName, Action resetPage, bool isItemSeason=false)
        {
            if (IsAppearance(tables, columnNameInDB, id, isItemSeason))
            {
                Classes.PageHandling.DialogsHandling.DisplayAreAppearances();
            }
            else
            {
                Classes.PageHandling.DialogsHandling.DisplayDeleteItemDialog(tableName, id, resetPage, NIDActualSeason);
            }
        }

        private static bool IsAppearance(string[] tables, string columnNameInDB, int id, bool isItemSeason = false)
        {
            bool appearance = false;
            foreach (string table in tables)
            {
                string sCommand = "SELECT * FROM " + table + " WHERE " + columnNameInDB + "='" + id + "'";
                if (!isItemSeason) { sCommand += " AND nIDSeason='" + NIDActualSeason + "'"; }

                SqliteDataReader query = QueryDB(sCommand);
                if (query.HasRows) { appearance = true; };
            }

            return appearance;
        }
    }
}
