using Microsoft.Data.Sqlite;
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
    public sealed partial class PageTeams : Page
    {
        private string sTableName = "tbl_teams";
        List<Classes.DBClasses.DBTeam> ListAllItems;

        string[] tables = { "tbl_matches", "tbl_players", "tbl_stats" };

        bool bSortShortNameASC;

        public PageTeams()
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
            ButtAdd.IsEnabled = (StatBoss.Classes.MainVariables.NIDActualSeason > 0) ? true : false;
            ButtEditSelected.IsEnabled = false;
            ButtDeleteDB.IsEnabled = false;
            ButtSelect.IsEnabled = false;

            Classes.PageHandling.ListViewHandling.ResetListView(ListViewItems);
            ShowItemsInListView();

            EnableEditableElements(false);
            TextBoxID.Text = "";
            TextBoxShortName.Text = "";
            TextBoxName.Text = "";

            ButtAddToDB.Visibility = Visibility.Collapsed;
            ButtEditDB.Visibility = Visibility.Collapsed;
        }

        private void EnableEditableElements(bool enabled)
        {
            TextBoxShortName.IsEnabled = enabled;
            TextBoxName.IsEnabled = enabled;
        }

        // ---------------------------
        // ListView Section
        // ---------------------------
        private void ShowItemsInListView(string sWhere = "", string sOrder = "")
        {
            ListAllItems = new List<Classes.DBClasses.DBTeam>();
            StatBoss.Classes.UIClasses.UITeam.ShowItemsInListView(ListViewItems, ListAllItems, sWhere, sOrder);
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
                Classes.DBClasses.DBTeam selectedItem = StatBoss.Classes.UIClasses.UITeam.GetSelectedTeam(e, ListAllItems);

                TextBoxID.Text = selectedItem.nID.ToString();
                TextBoxShortName.Text = selectedItem.sShortName;
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

            TextBoxID.Text = (DataAccess.GetMaxID(sTableName) + 1).ToString();
            TextBoxShortName.Text = "";
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
            DataAccess.RemoveItem(tables, "nIDUserTeam", int.Parse(TextBoxID.Text), sTableName, ResetPage);
        }

        private void ButtSelect_Click(object sender, RoutedEventArgs e)
        {
            StatBoss.Classes.MainVariables.NIDActualTeam = int.Parse(TextBoxID.Text);
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
                var team = new Classes.DBClasses.DBTeam
                {
                    nID = int.Parse(TextBoxID.Text),
                    nIDSeason = StatBoss.Classes.MainVariables.NIDActualSeason,
                    sShortName = TextBoxShortName.Text,
                    sName = TextBoxName.Text
                };

                team.ChangeDB(action);
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
        private void ButtSortShortName_Click(object sender, RoutedEventArgs e)
        {
            string sOrder = " ORDER BY sCategoryName";
            sOrder += bSortShortNameASC == true ? " ASC" : " DESC";
            ShowItemsInListView("", sOrder);

            bSortShortNameASC = !bSortShortNameASC;
        }

        private void TextBoxFindName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowItemsInListView(TextBoxFindName.Text);
        }
    }
}
