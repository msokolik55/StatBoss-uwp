using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class Player
    {
        public int nID;
        public string sFirstName;
        public string sSurname;
        public DateTime dBirthday;
        public int nIDUserTeam;
        public int nIDPosition;

        public Player()
        {
        }

        public Player(int id, string sfirstname, string ssurname, DateTime dbirthday, int niduserteam, int position)
        {
            this.nID = id;
            this.sFirstName = sfirstname;
            this.sSurname = ssurname;
            this.dBirthday = dbirthday;
            this.nIDUserTeam = niduserteam;
            this.nIDPosition = position;
        }

        private void FillList(List<Player> ListAllItems, string sWhere, string sOrder)
        {
            string sCommand = "SELECT * FROM tbl_players WHERE nIDSeason='" + DataAccess.NIDActualSeason + "' AND nIDVSTeam='" + DataAccess.NIDActualTeam + "'" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var iplayer = new Player
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

        public void ShowItemsInListView(ListView ListViewItems, List<Player> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new Player().FillList(ListAllItems, sWhere, sOrder);
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

        public void ShowInComboBox(List<Player> ListAllItems, ComboBox comboBox, string toRemove, string sWhere = "", string sOrder = "")
        {
            FillList(ListAllItems, sWhere, sOrder);
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

        public Player GetSelectedPlayer(SelectionChangedEventArgs e, List<Player> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new Player();
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
                    sCommand = "INSERT INTO tbl_players (nID, nIDSeason, sFirstName, sSurname, dBirthday, nIDVSTeam, nIDPosition) " +
                               "VALUES('" + nID + "', '" + DataAccess.NIDActualSeason + "', '" + sFirstName + "', '" + sSurname + "', '" + dBirthday.ToString("yyyy-MM-dd") + "', '" + DataAccess.NIDActualTeam + "', '" + nIDPosition + "')";
                    break;

                case "edit":
                    sCommand = "UPDATE tbl_players SET sFirstName='" + sFirstName + "', " +
                                                     "sSurname='" + sSurname + "', " +
                                                     "dBirthday='" + dBirthday + "', " +
                                                     "nIDPosition='" + nIDPosition + "' " +
                              "WHERE nID = '" + nID + "' AND nIDSeason = '" + DataAccess.NIDActualSeason + "'";
                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
