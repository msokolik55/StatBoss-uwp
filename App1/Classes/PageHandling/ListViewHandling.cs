using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.PageHandling
{
    public static class ListViewHandling
    {
        public static void ResetListView(ListView listView)
        {
            while (listView.Items.Count > 0) { listView.Items.RemoveAt(listView.Items.Count - 1); }
            listView.SelectedIndex = -1;
        }

        public static void NoItemsToShow(ListView ListViewItems)
        {
            TextBlock block = new TextBlock
            {
                Text = "There is no entry in the database for this selection."
            };

            ListViewItems.Items.Add(block);
            ListViewItems.IsEnabled = false;
        }
    }
}
