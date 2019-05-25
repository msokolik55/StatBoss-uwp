using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class DBOverallStat
    {
        public int nID;
        public int nIDSeason;
        public int nIDUserTeam;
        public string sFirstName;
        public string sSurname;
        public int nMatches;
        public int nMinutes;
        public int nGoals;
        public int nAssists;
        public int nPenalties;
        public int nRedCards;

        public DBOverallStat()
        {
        }

        public DBOverallStat(int nid, int nidseason, int nnumber, int niduserteam, string sfirstname, string ssurname,
            int nmatches, int nminutes, int ngoals, int nassists, int npenalties, int nredcards)
        {
            this.nID = nid;
            this.nIDSeason = nidseason;
            this.nIDUserTeam = niduserteam;
            this.sFirstName = sfirstname;
            this.sSurname = ssurname;
            this.nMatches = nmatches;
            this.nMinutes = nminutes;
            this.nGoals = ngoals;
            this.nAssists = nassists;
            this.nPenalties = npenalties;
            this.nRedCards = nredcards;
        }

        public void FillList(List<DBOverallStat> ListAllItems, string sWhere, string sOrder)
        {
            try
            {
                if (sWhere != "") { sWhere = " AND (p.sSurname || ' ' || p.sFirstName LIKE '%" + sWhere + "%')"; }

                string sCommand = "SELECT p.sFirstName AS sFirstName, p.sSurname AS sSurname, COUNT(s.nIDPlayer) AS nMatches, SUM(s.nMinutes) AS nMinutes, SUM(s.nGoals) AS nGoals, SUM(s.nAssistance) AS nAssists, SUM(s.nPenalties) AS nPenalties, SUM(s.nRedCards) AS nRedCards " +
                                  "FROM tbl_stats AS s " +
                                  "JOIN tbl_players AS p " +
                                  "ON p.nID = s.nIDPlayer " +
                                  "WHERE s.nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "' AND s.nIDUserTeam='" + StatBoss.Classes.MainVariables.NIDActualTeam + "' " + sWhere +
                                  "GROUP BY s.nIDPlayer" + sOrder;
                SqliteDataReader query = DataAccess.QueryDB(sCommand);

                Debug.WriteLine(sCommand);

                int id = 0;
                while (query.Read())
                {
                    var item = new DBOverallStat
                    {
                        nID = id,
                        sFirstName = query.GetString(query.GetOrdinal("sFirstName")),
                        sSurname = query.GetString(query.GetOrdinal("sSurname")),
                        nMatches = query.GetInt32(query.GetOrdinal("nMatches")),
                        nMinutes = query.GetInt32(query.GetOrdinal("nMinutes")),
                        nGoals = query.GetInt32(query.GetOrdinal("nGoals")),
                        nAssists = query.GetInt32(query.GetOrdinal("nAssists")),
                        nPenalties = query.GetInt32(query.GetOrdinal("nPenalties")),
                        nRedCards = query.GetInt32(query.GetOrdinal("nRedCards"))
                    };

                    ListAllItems.Add(item);
                    id++;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
