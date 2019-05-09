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
        public static async void DisplayDeleteItemDialog(string sTableName, int nID, Action action, bool isItemSeason = false)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Chcete odstrániť danú položku?",
                Content = "Tento krok nebudete môcť vrátiť späť.",
                PrimaryButtonText = "Odstrániť",
                CloseButtonText = "Zatvoriť"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string sCommand = "DELETE FROM " + sTableName + " WHERE nID = '" + nID + "'";
                if (!isItemSeason) { sCommand += " AND nIDSeason='" + DataAccess.NIDActualSeason + "'"; }

                DataAccess.ExecDB(sCommand);

                if (sTableName == "tbl_teams") { DataAccess.NIDActualTeam = DataAccess.GetMaxID("tbl_teams"); }
                else if (sTableName == "tbl_seasons")
                {
                    DataAccess.NIDActualSeason = DataAccess.GetMaxID("tbl_seasons", false);
                    DataAccess.NIDActualTeam = DataAccess.GetMaxID("tbl_teams");
                }

                action.Invoke();
            }
        }

        public static async void DisplayNoCorrectFields()
        {
            ContentDialog noAllFieldsDialog = new ContentDialog
            {
                Title = "No fiiled fields",
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
