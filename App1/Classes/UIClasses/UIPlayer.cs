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
    public static class UIPlayer
    {
        public static void ShowItemsInListView(ListView ListViewItems, List<DBPlayer> ListAllItems, string sWhere = "", string sOrder = "")
        {
            new DBPlayer().FillList(ListAllItems, sWhere, sOrder);
            ListViewHandling.ResetListView(ListViewItems);

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
                ListViewHandling.NoItemsToShow(ListViewItems);
            }
        }

        public static DBPlayer GetSelectedPlayer(SelectionChangedEventArgs e, List<DBPlayer> ListAllItems)
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

        public static void ShowSpecificPlayerInComboBox(ComboBox comboBox, int nIDPlayer, string toRemove)
        {
            ComboBoxHandling.ResetComboBox(comboBox);
            TextBlock block = new TextBlock
            {
                Name = toRemove + nIDPlayer.ToString(),
                Text = App1.DataAccess.GetPlayer(nIDPlayer)
            };
            comboBox.Items.Add(block);
            comboBox.SelectedIndex = 0;
        }

        public static void ShowUnusedPLayersInComboBox(List<DBPlayer> ListAllItems, ComboBox comboBox, string toRemove, int nIDMatch)
        {
            new DBPlayer().FillList(ListAllItems, "", "", true, nIDMatch);
            ComboBoxHandling.ResetComboBox(comboBox);

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


    }
}
