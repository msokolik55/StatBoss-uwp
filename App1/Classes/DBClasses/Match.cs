using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class Match
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

        public Match()
        {
        }

        public Match(int id, int nidseason, bool bplayed, int niduserteam, int nidopponent, DateTime ddatetime, string smatchplace, bool bhome,
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

        private void FillList(List<Match> ListAllItems, string sWhere, string sOrder)
        {
            string sCommand = "SELECT * FROM tbl_matches WHERE nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "' AND nIDUserTeam='" + StatBoss.Classes.MainVariables.NIDActualTeam + "'" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var imatch = new Match
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

        public void ShowItemsInListView(ListView ListViewItems, List<Match> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new Match().FillList(ListAllItems, sWhere, sOrder);
            PageHandling.ListViewHandling.ResetListView(ListViewItems);

            if (ListAllItems.Count > 0)
            {
                foreach (var item in ListAllItems)
                {
                    TextBlock block = new TextBlock
                    {
                        Name = item.nID.ToString(),
                        Text = item.dDateTime.ToString("dd.MM.yyyy HH:mm") + " " + DataAccess.GetOpponent(item.nIDOpponent)
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

        public Match GetSelectedMatch(SelectionChangedEventArgs e, List<Match> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new Match();
            foreach (var listItem in ListAllItems)
            {
                if (listItem.nID == id)
                {
                    selectedItem = listItem;
                }
            }

            return selectedItem;
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

        public void ShowInComboBox(List<Match> ListAllItems, ComboBox comboBox, string toRemove)
        {
            FillList(ListAllItems, " AND bPlayed = '1'", "");
            PageHandling.ComboBoxHandling.ResetComboBox(comboBox);

            foreach (var imatch in ListAllItems)
            {
                TextBlock block = new TextBlock
                {
                    Name = toRemove + imatch.nID.ToString(),
                    Text = imatch.dDateTime.ToString("dd.MM.yyyy HH:mm") + " " + DataAccess.GetOpponent(imatch.nIDOpponent)
                };

                comboBox.Items.Add(block);
            }
        }

        public Match GetSelectedMatchFromComboBox(SelectionChangedEventArgs e, List<Match> ListAllItems, ComboBox comboBox, string toRemove)
        {
            Match actualMatch = new Match();
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name.Substring(toRemove.Length));

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                TextBlock iblock = comboBox.Items[i] as TextBlock;
                if (id == int.Parse(iblock.Name.Substring(toRemove.Length)))
                {
                    actualMatch = ListAllItems[i];
                }
            }

            return actualMatch;
        }
    }
}
