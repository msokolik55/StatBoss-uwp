using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.PageHandling
{
    public static class FieldsChecking
    {
        public static bool AreElementsCorrect(UIElementCollection elements)
        {
            if (CheckTextBoxes(elements) && CheckComboBoxes(elements)) { return true; }
            else { return false; }
        }

        private static bool CheckTextBoxes(UIElementCollection elements)
        {
            List<TextBox> boxes = new List<TextBox>();
            foreach (var child in elements)
            {
                if (child is TextBox) { boxes.Add(child as TextBox); }
            }

            foreach (var box in boxes)
            {
                if (box.Text == "")
                {
                    return false;
                }

                try
                {
                    if (box.Tag.ToString() == "number")
                    {
                        bool success = int.TryParse(box.Text, out int code);
                        if (!success) { return false; }
                    }
                }
                catch (Exception)
                {
                }
            }
            return true;
        }

        private static bool CheckComboBoxes(UIElementCollection elements)
        {
            List<ComboBox> boxes = new List<ComboBox>();
            foreach (var child in elements)
            {
                if (child is ComboBox) { boxes.Add(child as ComboBox); }
            }

            foreach (var box in boxes)
            {
                if (box.SelectedIndex == -1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
