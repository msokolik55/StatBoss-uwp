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
    public sealed partial class PageOpponents : Page
    {
        private string sTableName = "tbl_opponents";
        List<Classes.DBClasses.Opponent> ListAllItems;

        string[] tables = { "tbl_matches" };

        bool bSortNameASC;

        public PageOpponents()
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
            ButtRemoveDB.IsEnabled = false;

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
            ListAllItems = new List<Classes.DBClasses.Opponent>();
            new Classes.DBClasses.Opponent().ShowItemsInListView(ListViewItems, ListAllItems, sWhere, sOrder);
        }

        private void ListViewItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtEditSelected.IsEnabled = true;
            ButtRemoveDB.IsEnabled = true;

            EnableEditableElements(false);

            try
            {
                Classes.DBClasses.Opponent selectedItem = new Classes.DBClasses.Opponent().GetSelectedOpponent(e, ListAllItems);

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
            ButtRemoveDB.IsEnabled = false;

            TextBoxID.Text = (DataAccess.GetMaxID(sTableName) + 1).ToString();
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

        private void ButtRemoveDB_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.RemoveItem(tables, "nIDOpponent", int.Parse(TextBoxID.Text), sTableName, ResetPage);
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
                var team = new Classes.DBClasses.Team
                {
                    nID = int.Parse(TextBoxID.Text),
                    nIDSeason = DataAccess.NIDActualSeason,
                    sName = TextBoxName.Text,
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
        private void ButtSortName_Click(object sender, RoutedEventArgs e)
        {
            string sOrder = " ORDER BY sName";
            sOrder += bSortNameASC == true ? " ASC" : " DESC";
            ShowItemsInListView("", sOrder);

            bSortNameASC = !bSortNameASC;
        }

        private void TextBoxFindName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string sFindName = TextBoxFindName.Text;
            string sWhere = " AND sName LIKE '%" + sFindName + "%'";
            ShowItemsInListView(sWhere);
        }
    }
}
