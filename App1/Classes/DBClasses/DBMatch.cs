using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class DBMatch
    {
        public int nID;
        public int nIDSeason;
        public bool bPlayed;
        public int nIDUserTeam;
        public int nIDOpponent;
        public DateTime dDateTime;
        public string sMatchPlace;
        public bool bHome;
        public int nGive;
        public int nReceived;
        public int nGive1;
        public int nReceived1;
        public int nGive2;
        public int nReceived2;
        public int nGive3;
        public int nReceived3;
        public string sMatchDescription;
        public DateTime dInserted;
        public DateTime dUpdated;

        public DBMatch()
        {
        }

        public DBMatch(int id, int nidseason, bool bplayed, int niduserteam, int nidopponent, DateTime ddatetime, string smatchplace, bool bhome,
            int ngive, int nreceived, int ngive1, int nreceived1, int ngive2, int nreceived2, int ngive3, int nreceived3, string smatchdescription, DateTime dinserted, DateTime dupdated)
        {
            this.nID = id;
            this.nIDSeason = nidseason;
            this.bPlayed = bplayed;
            this.nIDUserTeam = niduserteam;
            this.nIDOpponent = nidopponent;
            this.dDateTime = ddatetime;
            this.sMatchPlace = smatchplace;
            this.bHome = bhome;
            this.nGive = ngive;
            this.nReceived = nreceived;
            this.nGive1 = ngive1;
            this.nReceived1 = nreceived1;
            this.nGive2 = ngive2;
            this.nReceived2 = nreceived2;
            this.nGive3 = ngive3;
            this.nReceived3 = nreceived3;
            this.sMatchDescription = smatchdescription;
            this.dInserted = dinserted;
            this.dUpdated = dupdated;
        }

        public void FillList(List<DBMatch> ListAllItems, string sWhere, string sOrder)
        {
            string sCommand = "SELECT * FROM tbl_matches WHERE nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "' AND nIDUserTeam='" + StatBoss.Classes.MainVariables.NIDActualTeam + "'" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var imatch = new DBMatch
                {
                    nID = query.GetInt32(query.GetOrdinal("nID")),
                    nIDSeason = query.GetInt32(query.GetOrdinal("nIDSeason")),
                    bPlayed = query.GetBoolean(query.GetOrdinal("bPlayed")),
                    nIDUserTeam = query.GetInt32(query.GetOrdinal("nIDUserTeam")),
                    nIDOpponent = query.GetInt32(query.GetOrdinal("nIDOpponent")),

                    dDateTime = query.GetDateTime(query.GetOrdinal("dDatetime")),
                    sMatchPlace = query.GetString(query.GetOrdinal("sMatchPlace")),

                    bHome = query.GetBoolean(query.GetOrdinal("bHome")),
                    nGive = query.GetInt32(query.GetOrdinal("nGive")),
                    nReceived = query.GetInt32(query.GetOrdinal("nReceived")),
                    nGive1 = query.GetInt32(query.GetOrdinal("nGive1")),
                    nReceived1 = query.GetInt32(query.GetOrdinal("nReceived1")),
                    nGive2 = query.GetInt32(query.GetOrdinal("nGive2")),
                    nReceived2 = query.GetInt32(query.GetOrdinal("nReceived2")),
                    nGive3 = query.GetInt32(query.GetOrdinal("nGive3")),
                    nReceived3 = query.GetInt32(query.GetOrdinal("nReceived3")),
                    sMatchDescription = query.GetString(query.GetOrdinal("sMatchDescription")),
                    dInserted = query.GetDateTime(query.GetOrdinal("dInserted")),

                    dUpdated = query.GetDateTime(query.GetOrdinal("dUpdated"))
                };

                ListAllItems.Add(imatch);
            }
        }

        public void ChangeDB(string action)
        {
            string sCommand = "";

            switch (action)
            {
                case "add":
                    int played = bPlayed == true ? 1 : 0;
                    int home = bHome == true ? 1 : 0;
                    sCommand = "INSERT INTO tbl_matches (nID, nIDSeason, bPlayed, nIDUserTeam, nIDOpponent, dDatetime, sMatchPlace, bHome, nGive, nReceived, nGive1, nReceived1, nGive2, nReceived2, nGive3, nReceived3, sMatchDescription, dInserted, dUpdated)" +
                                                " VALUES('" + nID + "', " +
                                                "'" + nIDSeason + "', " +
                                                "'" + played + "', " +
                                                "'" + nIDUserTeam + "', " +
                                                "'" + nIDOpponent + "', " +
                                                "'" + dDateTime.ToString("yyyy-MM-dd HH:mm") + "', " +
                                                "'" + sMatchPlace + "', " +
                                                "'" + home + "', " +
                                                "'" + nGive + "', " +
                                                "'" + nReceived + "', " +
                                                "'" + nGive1 + "', " +
                                                "'" + nReceived1 + "', " +
                                                "'" + nGive2 + "', " +
                                                "'" + nReceived2 + "', " +
                                                "'" + nGive3 + "', " +
                                                "'" + nReceived3 + "', " +
                                                "'" + sMatchDescription + "', " +
                                                "datetime('now'), " +
                                                "datetime('now'))";
                    break;

                case "edit":
                    played = bPlayed == true ? 1 : 0;
                    home = bHome == true ? 1 : 0;
                    sCommand = "UPDATE tbl_matches SET bPlayed='" + played + "', " +
                                                             "nIDOpponent='" + nIDOpponent + "', " +
                                                             "dDatetime='" + dDateTime.ToString("yyyy-MM-dd HH:mm") + "', " +
                                                             "sMatchPlace='" + sMatchPlace + "', " +
                                                             "bHome='" + home + "', " +
                                                             "nGive='" + nGive + "', " +
                                                             "nReceived='" + nReceived + "', " +
                                                             "nGive1='" + nGive1 + "', " +
                                                             "nReceived1='" + nReceived1 + "', " +
                                                             "nGive2='" + nGive2 + "', " +
                                                             "nReceived2='" + nReceived2 + "', " +
                                                             "nGive3='" + nGive3 + "', " +
                                                             "nReceived3='" + nReceived3 + "', " +
                                                             "sMatchDescription='" + sMatchDescription + "', " +
                                                             "dUpdated=datetime('now')" +
                                      "WHERE nID = " + nID;
                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
