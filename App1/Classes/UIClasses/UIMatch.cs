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
    public static class UIMatch
    {
        public static void ShowItemsInListView(ListView ListViewItems, List<DBMatch> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new DBMatch().FillList(ListAllItems, sWhere, sOrder);
            ListViewHandling.ResetListView(ListViewItems);

            if (ListAllItems.Count > 0)
            {
                foreach (var item in ListAllItems)
                {
                    TextBlock block = new TextBlock
                    {
                        Name = item.nID.ToString(),
                        Text = item.dDateTime.ToString("dd.MM.yyyy HH:mm") + " " + App1.DataAccess.GetOpponent(item.nIDOpponent)
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

        public static DBMatch GetSelectedMatch(SelectionChangedEventArgs e, List<DBMatch> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new DBMatch();
            foreach (var listItem in ListAllItems)
            {
                if (listItem.nID == id)
                {
                    selectedItem = listItem;
                }
            }

            return selectedItem;
        }

        public static void ShowInComboBox(List<DBMatch> ListAllItems, ComboBox comboBox, string toRemove)
        {
            new DBMatch().FillList(ListAllItems, " AND bPlayed = '1'", "", false);
            ComboBoxHandling.ResetComboBox(comboBox);

            foreach (var imatch in ListAllItems)
            {
                TextBlock block = new TextBlock
                {
                    Name = toRemove + imatch.nID.ToString(),
                    Text = imatch.dDateTime.ToString("dd.MM.yyyy HH:mm") + " " + App1.DataAccess.GetOpponent(imatch.nIDOpponent)
                };

                comboBox.Items.Add(block);
            }
        }

        public static DBMatch GetSelectedMatchFromComboBox(SelectionChangedEventArgs e, List<DBMatch> ListAllItems, ComboBox comboBox, string toRemove)
        {
            DBMatch actualMatch = new DBMatch();
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
