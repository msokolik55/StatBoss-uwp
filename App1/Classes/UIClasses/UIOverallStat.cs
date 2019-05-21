using App1.Classes.DBClasses;
using App1.Classes.PageHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace StatBoss.Classes.UIClasses
{
    public static class UIOverallStat
    {
        public static void ShowItemsInListView(ListView ListViewItems, List<DBOverallStat> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new DBOverallStat().FillList(ListAllItems, sWhere, sOrder);

            ListViewHandling.ResetListView(ListViewItems);

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
                ListViewHandling.NoItemsToShow(ListViewItems);
            }

        }

        public static DBOverallStat GetSelectedOverallStat(SelectionChangedEventArgs e, List<DBOverallStat> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new DBOverallStat();
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
