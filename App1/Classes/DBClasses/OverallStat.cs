﻿using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class OverallStat
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

        public OverallStat()
        {
        }

        public OverallStat(int nid, int nidseason, int nnumber, int niduserteam, string sfirstname, string ssurname,
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

        private void FillList(List<OverallStat> ListAllItems, string sWhere, string sOrder)
        {
            try
            {
                string sCommand = "SELECT p.sFirstName AS sFirstName, p.sSurname AS sSurname, COUNT(s.nIDPlayer) AS nMatches, SUM(s.nMinutes) AS nMinutes, SUM(s.nGoals) AS nGoals, SUM(s.nAssistance) AS nAssists, SUM(s.nPenalties) AS nPenalties, SUM(s.nRedCards) AS nRedCards " +
                                  "FROM tbl_stats AS s " +
                                  "JOIN tbl_players AS p " +
                                  "ON p.nID = s.nIDPlayer " +
                                  "WHERE s.nIDSeason='" + DataAccess.NIDActualSeason + "' AND s.nIDUserTeam='" + DataAccess.NIDActualTeam + "' " + sWhere +
                                  "GROUP BY s.nIDPlayer" + sOrder;
                SqliteDataReader query = DataAccess.QueryDB(sCommand);

                Debug.WriteLine(sCommand);

                int id = 0;
                while (query.Read())
                {
                    var item = new OverallStat
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

        public void ShowItemsInListView(ListView ListViewItems, List<OverallStat> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new OverallStat().FillList(ListAllItems, sWhere, sOrder);

            PageHandling.ListViewHandling.ResetListView(ListViewItems);

            if (ListAllItems.Count > 0)
            {
                foreach (var item in ListAllItems)
                {
                    TextBlock block = new TextBlock
                    {
                        Name = item.nID.ToString(),
                        Text = item.sFirstName + " " + item.sSurname + " (" + item.nGoals + " + " + item.nAssists + ")"
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

        public OverallStat GetSelectedOverallStat(SelectionChangedEventArgs e, List<OverallStat> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new OverallStat();
            foreach (var listItem in ListAllItems)
            {
                if (listItem.nID == id)
                {
                    selectedItem = listItem;
                }
            }
            
            return ListAllItems[id];
        }
    }
}
