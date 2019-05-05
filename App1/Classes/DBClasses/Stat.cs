using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class Stat
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
        public DateTime dInserted;
        public DateTime dUpdated;

        public Stat()
        {
        }

        public Stat(int id, int nidseason, int nidmatch, int nidposition, int nnumber, int nidplayer, int niduserteam,
            int nminutes, int ngoals, int nassists, int npenalties, int nredcards, DateTime dinserted, DateTime dupdated)
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
            this.dInserted = dinserted;
            this.dUpdated = dupdated;
        }

        private void FillList(List<Stat> ListAllItems, int nIDMatch, string sWhere, string sOrder)
        {
            try
            {
                string sCommand = "SELECT * FROM tbl_stats AS s " +
                                  "JOIN tbl_players AS p ON p.nID = s.nIDPlayer " +
                                  "WHERE s.nIDSeason='" + DataAccess.NIDActualSeason + "' AND s.nIDMatch='" + nIDMatch + "'" + sWhere + sOrder;
                SqliteDataReader query = DataAccess.QueryDB(sCommand);

                while (query.Read())
                {
                    var item = new Stat
                    {
                        nID = query.GetInt32(query.GetOrdinal("nID")),
                        nIDSeason = query.GetInt32(query.GetOrdinal("nIDSeason")),
                        nIDMatch = query.GetInt32(query.GetOrdinal("nIDMatch")),
                        nIDPosition = query.GetInt32(query.GetOrdinal("nIDPosition")),
                        nNumber = query.GetInt32(query.GetOrdinal("nNumber")),
                        nIDPlayer = query.GetInt32(query.GetOrdinal("nIDPlayer")),
                        nIDUserTeam = query.GetInt32(query.GetOrdinal("nIDVSTeam")),
                        nMinutes = query.GetInt32(query.GetOrdinal("nMinutes")),
                        nGoals = query.GetInt32(query.GetOrdinal("nGoals")),
                        nAssists = query.GetInt32(query.GetOrdinal("nAssistance")),
                        nPenalties = query.GetInt32(query.GetOrdinal("nPenalties")),
                        nRedCards = query.GetInt32(query.GetOrdinal("nRedCards")),
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

        public void ShowItemsInListView(ListView ListViewItems, List<Stat> ListAllItems, int nIDMatch, string sWhere = "", string sOrder = "")
        {
            new Stat().FillList(ListAllItems, nIDMatch, sWhere, sOrder);

            PageHandling.ListViewHandling.ResetListView(ListViewItems);

            if (ListAllItems.Count > 0)
            {
                foreach (var item in ListAllItems)
                {
                    TextBlock block = new TextBlock
                    {
                        Name = item.nID.ToString(),
                        Text = item.nNumber.ToString() + "\t" + DataAccess.GetPlayer(item.nIDPlayer) + " (" + item.nGoals + " + " + item.nAssists + ")"
                    };

                    ListViewItems.Items.Add(block);
                }
                ListViewItems.IsEnabled = true;
            }
            else
            {
                PageHandling.ListViewHandling.NoItemsToShow(ListViewItems);
            }

        }

        public Stat GetSelectedStat(SelectionChangedEventArgs e, List<Stat> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new Stat();
            foreach (var listItem in ListAllItems)
            {
                if (listItem.nID == id)
                {
                    selectedItem = listItem;
                }
            }

            return selectedItem;
        }

        //public void AddToDB()
        //{
        //    string sCommand = "INSERT INTO tbl_stats (nID, nIDSeason, nIDMatch, nIDPosition, nNumber, nIDPlayer, nIDVSTeam, nMinutes, nGoals, nAssistance, nPenalties, nRedCards, dInserted, dUpdated)" +
        //                                " VALUES('" + nID + "', " +
        //                                "'" + nIDSeason + "', " +
        //                                "'" + nIDMatch + "', " +
        //                                "'" + nIDPosition + "', " +
        //                                "'" + nNumber + "', " +
        //                                "'" + nIDPlayer + "', " +
        //                                "'" + nIDUserTeam + "', " +
        //                                "'" + nMinutes + "', " +
        //                                "'" + nGoals + "', " +
        //                                "'" + nAssists + "', " +
        //                                "'" + nPenalties + "', " +
        //                                "'" + nRedCards + "', " +
        //                                "datetime('now'), " +
        //                                "datetime('now'))";
        //    DataAccess.ExecDB(sCommand);
        //}

        //public void EditDB()
        //{
        //    string sCommand = "UPDATE tbl_stats SET nIDPosition='" + nIDPosition + "', " +
        //                                                    "nNumber='" + nNumber + "', " +
        //                                                    "nMinutes='" + nMinutes + "', " +
        //                                                    "nGoals='" + nGoals + "', " +
        //                                                    "nAssistance='" + nAssists + "', " +
        //                                                    "nPenalties='" + nPenalties + "', " +
        //                                                    "nRedCards='" + nRedCards + "', " +
        //                                                    "dUpdated=datetime('now')" +
        //                        "WHERE nID = '" + nID + "' AND nIDSeason = '" + DataAccess.NIDActualSeason + "'";
        //    DataAccess.ExecDB(sCommand);
        //}

        public void ChangeDB(string action)
        {
            string sCommand = "";

            switch (action)
            {
                case "add":
                    sCommand = "INSERT INTO tbl_stats (nID, nIDSeason, nIDMatch, nIDPosition, nNumber, nIDPlayer, nIDVSTeam, nMinutes, nGoals, nAssistance, nPenalties, nRedCards, dInserted, dUpdated)" +
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
                                                    "dUpdated=datetime('now')" +
                                "WHERE nID = '" + nID + "' AND nIDSeason = '" + DataAccess.NIDActualSeason + "'";
                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
