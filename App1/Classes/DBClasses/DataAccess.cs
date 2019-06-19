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
        static readonly string sDBName = "sqliteSample.db";

        public static void InitializeDatabase()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=" + sDBName))
            {
                db.Open();

                List<string> sCreateTables = new List<string>
                {
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
                                    "nPlusMinus	TINYINT(4), " +
                                    "dInserted	DATETIME, " +
                                    "nIDInserted	SMALLINT(6), " +
                                    "dUpdated	DATETIME, " +
                                    "nIDUpdated	SMALLINT(6))",

                    "CREATE TABLE IF NOT EXISTS tbl_teams ( " +
                                    "nID	INTEGER, " +
                                    "nIDSeason	SMALLINT(6) NOT NULL, " +
                                    "sCategoryName	VARCHAR(10) NOT NULL, " +
                                    "sName	VARCHAR(100) NOT NULL, " +
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
            using (SqliteConnection db = new SqliteConnection("Filename=" + sDBName))
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
            using (SqliteConnection db = new SqliteConnection("Filename=" + sDBName))
            {
                db.Open();

                try
                {
                    SqliteCommand insertCommand = new SqliteCommand(sCommand, db);
                    insertCommand.ExecuteReader();

                }
                catch (SqliteException exception)
                {
                    DisplayException(sCommand + "\n" + exception.ToString());
                }
                db.Close();
            }
        }

        public static void RemoveDB(string sTableName, int nID, Action action, bool seasonInTable = true)
        {
            string sCommand = "DELETE FROM " + sTableName + " WHERE nID = '" + nID + "'";
            if (seasonInTable) { sCommand += " AND nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "'"; }

            DataAccess.ExecDB(sCommand);

            if (sTableName == "tbl_teams") { StatBoss.Classes.MainVariables.NIDActualTeam = DataAccess.GetMaxID("tbl_teams"); }
            else if (sTableName == "tbl_seasons")
            {
                StatBoss.Classes.MainVariables.NIDActualSeason = DataAccess.GetMaxID("tbl_seasons", false);
                StatBoss.Classes.MainVariables.NIDActualTeam = DataAccess.GetMaxID("tbl_teams");
            }
        }

        public static int GetMaxID(string sTableName, bool bSeason = true)
        {
            int nMaxID = 0;

            string sCommand = "SELECT MAX(nID) FROM " + sTableName;
            if(bSeason) { sCommand += " WHERE nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "'"; }

            SqliteDataReader query = QueryDB(sCommand);

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
            string sCommand = "SELECT * FROM tbl_opponents WHERE nID='" + nID + "' AND nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "'";
            SqliteDataReader query = QueryDB(sCommand);

            string sOpponent = "";
            while (query.Read())
            {
                sOpponent = query.GetString(query.GetOrdinal("sName"));
            }

            return sOpponent;
        }

        public static string GetPlayer(int nID)
        {
            string sCommand = "SELECT * FROM tbl_players WHERE nID='" + nID + "' AND nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "'";
            SqliteDataReader query = QueryDB(sCommand);

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

        public static void RemoveItem(string[] tables, string columnNameInDB, int id, string tableName, Action resetPage, bool seasonInTable = true)
        {
            if (IsAppearance(tables, columnNameInDB, id, seasonInTable))
            {
                Classes.PageHandling.DialogsHandling.DisplayAreAppearances();
            }
            else
            {
                Classes.PageHandling.DialogsHandling.DisplayDeleteItemDialog(tableName, id, resetPage, seasonInTable);
            }
        }

        private static bool IsAppearance(string[] tables, string columnNameInDB, int id, bool seasonInTable = true)
        {
            bool appearance = false;
            foreach (string table in tables)
            {
                string sCommand = "SELECT * FROM " + table + " WHERE " + columnNameInDB + "='" + id + "'";
                if (seasonInTable) { sCommand += " AND nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "'"; }

                SqliteDataReader query = QueryDB(sCommand);
                if (query.HasRows) { appearance = true; };
            }

            return appearance;
        }
    }
}
