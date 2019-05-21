using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class DBPlayer
    {
        public int nID;
        public string sFirstName;
        public string sSurname;
        public DateTime dBirthday;
        public int nIDUserTeam;
        public int nIDPosition;

        public DBPlayer()
        {
        }

        public DBPlayer(int id, string sfirstname, string ssurname, DateTime dbirthday, int niduserteam, int position)
        {
            this.nID = id;
            this.sFirstName = sfirstname;
            this.sSurname = ssurname;
            this.dBirthday = dbirthday;
            this.nIDUserTeam = niduserteam;
            this.nIDPosition = position;
        }

        private void FillList(List<DBPlayer> ListAllItems, string sWhere="", string sOrder="")
        {
            string sCommand = "SELECT * FROM tbl_players WHERE nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "' AND nIDUserTeam='" + StatBoss.Classes.MainVariables.NIDActualTeam + "'" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var iplayer = new DBPlayer
                {
                    nID = query.GetInt32(query.GetOrdinal("nID")),
                    sFirstName = query.GetString(query.GetOrdinal("sFirstName")),
                    sSurname = query.GetString(query.GetOrdinal("sSurname")),
                    dBirthday = query.GetDateTime(query.GetOrdinal("dBirthday")),
                    nIDPosition = query.GetInt32(query.GetOrdinal("nIDPosition"))
                };

                ListAllItems.Add(iplayer);
            }
        }

        public void ShowItemsInListView(ListView ListViewItems, List<DBPlayer> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new DBPlayer().FillList(ListAllItems, sWhere, sOrder);
            PageHandling.ListViewHandling.ResetListView(ListViewItems);

            if (ListAllItems.Count > 0)
            {
                foreach (var item in ListAllItems)
                {
                    TextBlock block = new TextBlock
                    {
                        Name = item.nID.ToString(),
                        Text = item.sSurname + " " + item.sFirstName + " " + item.dBirthday.ToString("dd.MM.yyyy")
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

        public void ShowUnusedPLayersInComboBox(List<DBPlayer> ListAllItems, ComboBox comboBox, string toRemove, int nIDMatch)
        {
            string sWhere = " AND nID not in (SELECT nIDPlayer FROM tbl_stats " +
                                             "WHERE nIDSeason = '" + StatBoss.Classes.MainVariables.NIDActualSeason + "' AND " +
                                                   "nIDUserTeam = '" + StatBoss.Classes.MainVariables.NIDActualTeam + "' AND " +
                                                   "nIDMatch = '" + nIDMatch + "')";

            FillList(ListAllItems, sWhere);
            PageHandling.ComboBoxHandling.ResetComboBox(comboBox);

            foreach (var iplayer in ListAllItems)
            {
                TextBlock block = new TextBlock
                {
                    Name = toRemove + iplayer.nID.ToString(),
                    Text = iplayer.sSurname + " " + iplayer.sFirstName
                };

                comboBox.Items.Add(block);
            }
        }

        public void ShowSpecificPlayerInComboBox(ComboBox comboBox, int nIDPlayer, string toRemove)
        {
            PageHandling.ComboBoxHandling.ResetComboBox(comboBox);
            TextBlock block = new TextBlock
            {
                Name = toRemove + nIDPlayer.ToString(),
                Text = DataAccess.GetPlayer(nIDPlayer)
            };
            comboBox.Items.Add(block);
            comboBox.SelectedIndex = 0;
        }

        public DBPlayer GetSelectedPlayer(SelectionChangedEventArgs e, List<DBPlayer> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new DBPlayer();
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
                    sCommand = "INSERT INTO tbl_players (nID, nIDSeason, sFirstName, sSurname, dBirthday, nIDUserTeam, nIDPosition) " +
                               "VALUES('" + nID + "', '" + StatBoss.Classes.MainVariables.NIDActualSeason + "', '" + sFirstName + "', '" + sSurname + "', '" + dBirthday.ToString("yyyy-MM-dd") + "', '" + StatBoss.Classes.MainVariables.NIDActualTeam + "', '" + nIDPosition + "')";
                    break;

                case "edit":
                    sCommand = "UPDATE tbl_players SET sFirstName='" + sFirstName + "', " +
                                                     "sSurname='" + sSurname + "', " +
                                                     "dBirthday='" + dBirthday + "', " +
                                                     "nIDPosition='" + nIDPosition + "' " +
                              "WHERE nID = '" + nID + "' AND nIDSeason = '" + StatBoss.Classes.MainVariables.NIDActualSeason + "'";
                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
