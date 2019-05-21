using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class DBSeason
    {
        public int nID;
        public string sName;
        public DateTime dInserted;
        public DateTime dUpdated;

        public DBSeason()
        {
        }

        public DBSeason(int id, string sname, DateTime dinserted, DateTime dupdated)
        {
            this.nID = id;
            this.sName = sname;
            this.dInserted = dinserted;
            this.dUpdated = dupdated;
        }

        private void FillList(List<DBSeason> ListAllItems, string sWhere, string sOrder)
        {
            string sCommand = "SELECT * FROM tbl_seasons" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var item = new DBSeason
                {
                    nID = query.GetInt32(query.GetOrdinal("nID")),
                    sName = query.GetString(query.GetOrdinal("sName"))
                };

                ListAllItems.Add(item);
            }
        }

        public void ShowItemsInListView(ListView ListViewItems, List<DBSeason> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new DBSeason().FillList(ListAllItems, sWhere, sOrder);
            PageHandling.ListViewHandling.ResetListView(ListViewItems);

            if (ListAllItems.Count > 0)
            {
                foreach (var item in ListAllItems)
                {
                    TextBlock block = new TextBlock
                    {
                        Name = item.nID.ToString(),
                        Text = item.sName
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

        public DBSeason GetSelectedSeason(SelectionChangedEventArgs e, List<DBSeason> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new DBSeason();
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
                    sCommand = "INSERT INTO tbl_seasons (nID, sName, dInserted) VALUES('" + nID + "', '" + sName + "', datetime('now'))";
                    break;

                case "edit":
                    sCommand = "UPDATE tbl_seasons SET sName='" + sName + "', dUpdated=datetime('now') WHERE nID = " + nID; ;
                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
