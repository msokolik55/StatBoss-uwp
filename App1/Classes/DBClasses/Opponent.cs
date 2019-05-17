using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class Opponent
    {
        public int nID;
        public int nIDSeason;
        public string sName;
        public DateTime dInserted;
        public DateTime dUpdated;

        public Opponent()
        {
        }

        public Opponent(int id, int nidseason, string sname, DateTime dinserted, DateTime dupdated)
        {
            this.nID = id;
            this.nIDSeason = nidseason;
            this.sName = sname;
            this.dInserted = dinserted;
            this.dUpdated = dupdated;
        }

        private void FillList(List<Opponent> ListAllItems, string sWhere, string sOrder)
        {
            string sCommand = "SELECT * FROM tbl_opponents WHERE nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "'" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var item = new Opponent
                {
                    nID = query.GetInt32(query.GetOrdinal("nID")),
                    sName = query.GetString(query.GetOrdinal("sName"))
                };

                ListAllItems.Add(item);
            }
        }

        public void ShowItemsInListView(ListView ListViewItems, List<Opponent> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new Opponent().FillList(ListAllItems, sWhere, sOrder);
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

        public Opponent GetSelectedOpponent(SelectionChangedEventArgs e, List<Opponent> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new Opponent();
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
                    sCommand = "INSERT INTO tbl_opponents (nID, nIDSeason, sName, dInserted) VALUES('" + nID + "', '" + StatBoss.Classes.MainVariables.NIDActualSeason + "', '" + sName + "', datetime('now'))";
                    break;

                case "edit":
                    sCommand = "UPDATE tbl_opponents SET sName='" + sName + "', dUpdated=datetime('now') WHERE nID = " + nID + " AND nIDSeason = " + StatBoss.Classes.MainVariables.NIDActualSeason;
                    break;
            }

            DataAccess.ExecDB(sCommand);
        }

        public void ShowInComboBox(List<Opponent> ListAllItems, ComboBox comboBox, string toRemove, string sWhere = "", string sOrder = "")
        {
            FillList(ListAllItems, sWhere, sOrder);
            PageHandling.ComboBoxHandling.ResetComboBox(comboBox);

            foreach (var iopponent in ListAllItems)
            {
                TextBlock block = new TextBlock
                {
                    Name = toRemove + iopponent.nID.ToString(),
                    Text = DataAccess.GetOpponent(iopponent.nID)
                };

                comboBox.Items.Add(block);
            }
        }


    }
}
