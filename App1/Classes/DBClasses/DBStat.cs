using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class DBStat
    {
        public int nID;
        public int nIDSeason;
        public int nIDMatch;
        public int nIDPosition;
        public int nNumber;
        public int nIDPlayer;
        public int nIDUserTeam;
        public int nMinutes;
        public int nGoals;
        public int nAssists;
        public int nPenalties;
        public int nRedCards;
        public int nPlusMinus;
        public DateTime dInserted;
        public DateTime dUpdated;

        public DBStat()
        {
        }

        public DBStat(int id, int nidseason, int nidmatch, int nidposition, int nnumber, int nidplayer, int niduserteam,
            int nminutes, int ngoals, int nassists, int npenalties, int nredcards, int nplusminus, DateTime dinserted, DateTime dupdated)
        {
            this.nID = id;
            this.nIDSeason = nidseason;
            this.nIDMatch = nidmatch;
            this.nIDPosition = nidposition;
            this.nNumber = nnumber;
            this.nIDPlayer = nidplayer;
            this.nIDUserTeam = niduserteam;
            this.nMinutes = nminutes;
            this.nGoals = ngoals;
            this.nAssists = nassists;
            this.nPenalties = npenalties;
            this.nRedCards = nredcards;
            this.nPlusMinus = nplusminus;
            this.dInserted = dinserted;
            this.dUpdated = dupdated;
        }

        public void FillList(List<DBStat> ListAllItems, int nIDMatch, string sWhere, string sOrder, bool bASC = true)
        {
            try
            {
                if(sWhere != "") { sWhere = " AND (p.sSurname || ' ' || p.sFirstName LIKE '%" + sWhere + "%')"; }
                if (sOrder != "")
                {
                    sOrder = " ORDER BY " + sOrder;
                    sOrder += bASC == true ? " ASC" : " DESC";
                }

                string sCommand = "SELECT * FROM tbl_stats AS s " +
                                  "JOIN tbl_players AS p ON p.nID = s.nIDPlayer " +
                                  "WHERE s.nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "' AND s.nIDMatch='" + nIDMatch + "'" + sWhere + sOrder;
                SqliteDataReader query = DataAccess.QueryDB(sCommand);

                while (query.Read())
                {
                    var item = new DBStat
                    {
                        nID = query.GetInt32(query.GetOrdinal("nID")),
                        nIDSeason = query.GetInt32(query.GetOrdinal("nIDSeason")),
                        nIDMatch = query.GetInt32(query.GetOrdinal("nIDMatch")),
                        nIDPosition = query.GetInt32(query.GetOrdinal("nIDPosition")),
                        nNumber = query.GetInt32(query.GetOrdinal("nNumber")),
                        nIDPlayer = query.GetInt32(query.GetOrdinal("nIDPlayer")),
                        nIDUserTeam = query.GetInt32(query.GetOrdinal("nIDUserTeam")),
                        nMinutes = query.GetInt32(query.GetOrdinal("nMinutes")),
                        nGoals = query.GetInt32(query.GetOrdinal("nGoals")),
                        nAssists = query.GetInt32(query.GetOrdinal("nAssistance")),
                        nPenalties = query.GetInt32(query.GetOrdinal("nPenalties")),
                        nRedCards = query.GetInt32(query.GetOrdinal("nRedCards")),
                        nPlusMinus = query.GetInt32(query.GetOrdinal("nPlusMinus")),
                        dInserted = query.GetDateTime(query.GetOrdinal("dInserted")),

                        dUpdated = query.GetDateTime(query.GetOrdinal("dUpdated"))
                    };

                    ListAllItems.Add(item);
                }
            }
            catch (Exception)
            {
            }
        }

        public void ChangeDB(string action)
        {
            string sCommand = "";

            switch (action)
            {
                case "add":
                    sCommand = "INSERT INTO tbl_stats (nID, nIDSeason, nIDMatch, nIDPosition, nNumber, nIDPlayer, nIDUserTeam, nMinutes, nGoals, nAssistance, nPenalties, nRedCards, nPlusMinus, dInserted, dUpdated)" +
                                        " VALUES('" + nID + "', " +
                                        "'" + nIDSeason + "', " +
                                        "'" + nIDMatch + "', " +
                                        "'" + nIDPosition + "', " +
                                        "'" + nNumber + "', " +
                                        "'" + nIDPlayer + "', " +
                                        "'" + nIDUserTeam + "', " +
                                        "'" + nMinutes + "', " +
                                        "'" + nGoals + "', " +
                                        "'" + nAssists + "', " +
                                        "'" + nPenalties + "', " +
                                        "'" + nRedCards + "', " +
                                        "'" + nPlusMinus + "', " +
                                        "datetime('now'), " +
                                        "datetime('now'))";
                    break;

                case "edit":
                    sCommand = "UPDATE tbl_stats SET nIDPosition='" + nIDPosition + "', " +
                                                    "nNumber='" + nNumber + "', " +
                                                    "nMinutes='" + nMinutes + "', " +
                                                    "nGoals='" + nGoals + "', " +
                                                    "nAssistance='" + nAssists + "', " +
                                                    "nPenalties='" + nPenalties + "', " +
                                                    "nRedCards='" + nRedCards + "', " +
                                                    "nPlusMinus='" + nPlusMinus + "', " +
                                                    "dUpdated=datetime('now')" +
                                "WHERE nID = '" + nID + "' AND nIDSeason = '" + StatBoss.Classes.MainVariables.NIDActualSeason + "'";
                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
