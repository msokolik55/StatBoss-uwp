using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.PageHandling
{
    public static class DialogsHandling
    {
        public static async void DisplayDeleteItemDialog(string sTableName, int nID, Action action, bool seasonInTable = true)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete this item?",
                Content = "This item will be deleted permanently.",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Close"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                DataAccess.RemoveDB(sTableName, nID, action, seasonInTable);
                action.Invoke();
            }
        }

        public static async void DisplayNoCorrectFields()
        {
            ContentDialog noAllFieldsDialog = new ContentDialog
            {
                Title = "No filled fields",
                Content = "All fields have to be filled correctly.",
                CloseButtonText = "OK"
            };

            ContentDialogResult result = await noAllFieldsDialog.ShowAsync();
        }

        public static async void DisplayAreAppearances()
        {
            ContentDialog areAppearancesDialog = new ContentDialog
            {
                Title = "Can't delete this item",
                Content = "Firstly delete all appearances of this item in other tables.",
                CloseButtonText = "OK"
            };

            ContentDialogResult result = await areAppearancesDialog.ShowAsync();
        }

        public static async void DisplayAbout()
        {
            ContentDialog showAbout = new ContentDialog
            {
                Title = "About",
                Content = "Copyright © 2019 Michal Sokolik\n" +
                          "All rights reserved\n\n" +
                          "E-mail: michal.sokolik@studentstc.sk",
                CloseButtonText = "OK"
            };

            ContentDialogResult result = await showAbout.ShowAsync();
        }
    }
}
