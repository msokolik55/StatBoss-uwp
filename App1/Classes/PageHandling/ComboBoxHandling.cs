using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.PageHandling
{
    public static class ComboBoxHandling
    {
        public static void ResetComboBox(ComboBox box)
        {
            while (box.Items.Count > 0) { box.Items.RemoveAt(0); }
            box.SelectedIndex = -1;
        }

        public static int GetIDFromComboBox(object item, string toRemove)
        {
            TextBlock selectedBlock = item as TextBlock;
            return int.Parse(selectedBlock.Name.Substring(toRemove.Length));
        }

        public static int GetIDIntoComboBox(ComboBox comboBox, int nIDToFind, string toRemove)
        {
            int id = 0;
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                TextBlock iblock = comboBox.Items[i] as TextBlock;
                if (nIDToFind == int.Parse(iblock.Name.Substring(toRemove.Length)))
                {
                    id = i;
                }
            }

            return id;
        }
    }
}
