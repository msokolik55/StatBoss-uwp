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
    public static class UIOpponent
    {
        public static void ShowItemsInListView(ListView ListViewItems, List<DBOpponent> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new DBOpponent().FillList(ListAllItems, sWhere, sOrder);
            ListViewHandling.ResetListView(ListViewItems);

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
                ListViewHandling.NoItemsToShow(ListViewItems);
            }
        }

        public static DBOpponent GetSelectedOpponent(SelectionChangedEventArgs e, List<DBOpponent> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new DBOpponent();
            foreach (var listItem in ListAllItems)
            {
                if (listItem.nID == id)
                {
                    selectedItem = listItem;
                }
            }

            return selectedItem;
        }

        public static void ShowInComboBox(List<DBOpponent> ListAllItems, ComboBox comboBox, string toRemove, string sWhere = "", string sOrder = "")
        {
            new DBOpponent().FillList(ListAllItems, sWhere, sOrder);
            ComboBoxHandling.ResetComboBox(comboBox);

            foreach (var iopponent in ListAllItems)
            {
                TextBlock block = new TextBlock
                {
                    Name = toRemove + iopponent.nID.ToString(),
                    Text = App1.DataAccess.GetOpponent(iopponent.nID)
                };

                comboBox.Items.Add(block);
            }
        }
    }
}
