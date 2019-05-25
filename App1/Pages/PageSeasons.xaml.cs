using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageSeasons : Page
    {
        private string sTableName = "tbl_seasons";
        List<Classes.DBClasses.DBSeason> ListAllItems;

        string[] tables = { "tbl_matches", "tbl_opponents", "tbl_players", "tbl_stats", "tbl_teams" };

        bool bSortNameASC;

        public PageSeasons()
        {
            this.InitializeComponent();

            ResetPage();
            TextBoxID.IsEnabled = false;
        }

        // ---------------------------
        // Page Functions
        // ---------------------------
        private void ResetPage()
        {
            ButtAdd.IsEnabled = true;
            ButtEditSelected.IsEnabled = false;
            ButtDeleteDB.IsEnabled = false;
            ButtSelect.IsEnabled = false;

            Classes.PageHandling.ListViewHandling.ResetListView(ListViewItems);
            ShowItemsInListView();

            EnableEditableElements(false);
            TextBoxID.Text = "";
            TextBoxName.Text = "";

            ButtAddToDB.Visibility = Visibility.Collapsed;
            ButtEditDB.Visibility = Visibility.Collapsed;
        }

        private void EnableEditableElements(bool enabled)
        {
            TextBoxName.IsEnabled = enabled;
        }

        // ---------------------------
        // ListView Section
        // ---------------------------
        private void ShowItemsInListView(string sWhere = "", string sOrder = "")
        {
            ListAllItems = new List<Classes.DBClasses.DBSeason>();
            StatBoss.Classes.UIClasses.UISeason.ShowItemsInListView(ListViewItems, ListAllItems, sWhere, sOrder);
        }

        private void ListViewItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtEditSelected.IsEnabled = true;
            ButtDeleteDB.IsEnabled = true;
            ButtSelect.IsEnabled = true;

            ButtAddToDB.Visibility = Visibility.Collapsed;
            ButtEditDB.Visibility = Visibility.Collapsed;

            EnableEditableElements(false);

            try
            {
                Classes.DBClasses.DBSeason selectedItem = StatBoss.Classes.UIClasses.UISeason.GetSelectedSeason(e, ListAllItems);

                TextBoxID.Text = selectedItem.nID.ToString();
                TextBoxName.Text = selectedItem.sName;
            }
            catch (Exception)
            {
            }
        }

        // ---------------------------
        // Buttons Section
        // ---------------------------
        private void ButtAdd_Click(object sender, RoutedEventArgs e)
        {
            ButtEditSelected.IsEnabled = false;
            ButtDeleteDB.IsEnabled = false;
            ButtSelect.IsEnabled = false;

            TextBoxID.Text = (DataAccess.GetMaxID(sTableName, false) + 1).ToString();
            TextBoxName.Text = "";

            ListViewItems.SelectedIndex = -1;

            ButtAddToDB.Visibility = Visibility.Visible;
            ButtEditDB.Visibility = Visibility.Collapsed;

            EnableEditableElements(true);
        }

        private void ButtEditSelected_Click(object sender, RoutedEventArgs e)
        {
            ButtAddToDB.Visibility = Visibility.Collapsed;
            ButtEditDB.Visibility = Visibility.Visible;
            ButtEditDB.IsEnabled = true;

            EnableEditableElements(true);
        }

        private void ButtDeleteDB_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.RemoveItem(tables, "nIDSeason", int.Parse(TextBoxID.Text), sTableName, ResetPage, false);
        }

        private void ButtSelect_Click(object sender, RoutedEventArgs e)
        {
            StatBoss.Classes.MainVariables.NIDActualSeason = int.Parse(TextBoxID.Text);
            StatBoss.Classes.MainVariables.NIDActualTeam = DataAccess.GetMaxID("tbl_teams");
        }

        private void ButtAddToDB_Click(object sender, RoutedEventArgs e)
        {
            ChangeDB("add");
        }

        private void ButtEditDB_Click(object sender, RoutedEventArgs e)
        {
            ChangeDB("edit");
        }

        private void ChangeDB(string action)
        {
            if (Classes.PageHandling.FieldsChecking.AreElementsCorrect(GridEditableElements.Children))
            {
                var season = new Classes.DBClasses.DBSeason
                {
                    nID = int.Parse(TextBoxID.Text),
                    sName = TextBoxName.Text
                };

                season.ChangeDB(action);
                ResetPage();
            }
            else
            {
                Classes.PageHandling.DialogsHandling.DisplayNoCorrectFields();
            }
        }

        // ---------------------------
        // Filters and Sorts Section
        // ---------------------------
        private void ButtSortName_Click(object sender, RoutedEventArgs e)
        {
            string sOrder = " ORDER BY sName";
            sOrder += bSortNameASC == true ? " ASC" : " DESC";
            ShowItemsInListView("", sOrder);

            bSortNameASC = !bSortNameASC;
        }

        private void TextBoxFindName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowItemsInListView(TextBoxFindName.Text);
        }

    }
}
