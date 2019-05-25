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
    public static class UIStat
    {
        public static void ShowItemsInListView(ListView ListViewItems, List<DBStat> ListAllItems, int nIDMatch, string sWhere = "", string sOrder = "", bool bASC = true)
        {
            new DBStat().FillList(ListAllItems, nIDMatch, sWhere, sOrder, bASC);

            ListViewHandling.ResetListView(ListViewItems);

            if (ListAllItems.Count > 0)
            {
                foreach (var item in ListAllItems)
                {
                    TextBlock block = new TextBlock
                    {
                        Name = item.nID.ToString(),
                        Text = item.nNumber.ToString() + "\t" + App1.DataAccess.GetPlayer(item.nIDPlayer) + " (" + item.nGoals + " + " + item.nAssists + ")"
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

        public static DBStat GetSelectedStat(SelectionChangedEventArgs e, List<DBStat> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new DBStat();
            foreach (var listItem in ListAllItems)
            {
                if (listItem.nID == id)
                {
                    selectedItem = listItem;
                }
            }

            return selectedItem;
        }
    }
}
