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
    public static class UIPosition
    {
        public static void ShowItemsInListView(ListView ListViewItems, List<DBPosition> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new DBPosition().FillList(ListAllItems, sWhere, sOrder);
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

        public static void ShowInComboBox(List<DBPosition> ListAllItems, ComboBox comboBox, string toRemove, string sWhere = "", string sOrder = "")
        {
            new DBPosition().FillList(ListAllItems, sWhere, sOrder);
            ComboBoxHandling.ResetComboBox(comboBox);

            foreach (var iposition in ListAllItems)
            {
                TextBlock block = new TextBlock
                {
                    Name = toRemove + iposition.nID.ToString(),
                    Text = iposition.sName
                };

                comboBox.Items.Add(block);
            }
        }

        public static DBPosition GetSelectedPosition(SelectionChangedEventArgs e, List<DBPosition> ListAllItems)
        {
            var listViewItem = e.AddedItems;

            TextBlock block = (TextBlock)listViewItem[listViewItem.Count - 1];
            int id = int.Parse(block.Name);

            var selectedItem = new DBPosition();
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
