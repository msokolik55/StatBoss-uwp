using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class Team
    {
        public int nID;
        public int nIDSeason;
        public string sShortName;
        public string sName;

        public Team()
        {
        }

        public Team(int id, int nidseason, string sshortname, string sname)
        {
            this.nID = id;
            this.nIDSeason = nidseason;
            this.sShortName = sname;
            this.sName = sname;
        }

        private void FillList(List<Team> ListAllItems, string sWhere, string sOrder)
        {
            string sCommand = "SELECT * FROM tbl_teams WHERE nIDSeason='" + DataAccess.NIDActualSeason + "'" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var item = new Team
                {
                    nID = query.GetInt32(query.GetOrdinal("nID")),
                    sShortName = query.GetString(query.GetOrdinal("sCategoryName")),
                    sName = query.GetString(query.GetOrdinal("sName"))
                };

                ListAllItems.Add(item);
            }
        }

        public void ShowItemsInListView(ListView ListViewItems, List<Team> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new Team().FillList(ListAllItems, sWhere, sOrder);
            PageHandling.ListViewHandling.ResetListView(ListViewItems);

            if (ListAllItems.Count > 0)
            {
                foreach (var item in ListAllItems)
                {
                    TextBlock block = new TextBlock
                    {
                        Name = item.nID.ToString(),
                        Text = item.sShortName + " " + item.sName
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

        public Team GetSelectedTeam(SelectionChangedEventArgs e, List<Team> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new Team();
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
                    sCommand = "INSERT INTO tbl_teams (nID, nIDSeason, sCategoryName, sName, nYearFrom, nYearTo)" +
                              " VALUES('" + nID + "', '" + nIDSeason + "', '" + sShortName + "', '" + sName + "')";
                    break;

                case "edit":
                    sCommand = "UPDATE tbl_teams SET sCategoryName='" + sShortName + "', " +
                                                    "sName='" + sName + "', " +
                                "WHERE nID = '" + nID + "' AND nIDSeason = '" + nIDSeason + "'";

                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
