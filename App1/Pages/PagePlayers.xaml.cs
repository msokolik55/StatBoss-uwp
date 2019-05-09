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

using Microsoft.Data.Sqlite;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PagePlayers : Page
    {
        private string sTableName = "tbl_players";
        List<Classes.DBClasses.Player> ListAllItems;
        List<Classes.DBClasses.Position> ListAllPositions;

        private string toRemove = "position_";

        string[] tables = { "tbl_stats" };

        bool bSortBirthdayASC;

        public PagePlayers()
        {
            this.InitializeComponent();

            ResetPage();
            ShowPositions();

            TextBoxID.IsEnabled = false;
        }

        // ---------------------------
        // Page Functions
        // ---------------------------
        private void ResetPage()
        {
            ButtAdd.IsEnabled = (DataAccess.NIDActualSeason > 0 && DataAccess.NIDActualTeam > 0) ? true : false;
            ButtEditSelected.IsEnabled = false;
            ButtDeleteDB.IsEnabled = false;

            Classes.PageHandling.ListViewHandling.ResetListView(ListViewItems);
            ShowItemsInListView();

            EnableEditableElements(false);
            TextBoxID.Text = "";
            TextBoxFirstName.Text = "";
            TextBoxSurname.Text = "";
            DatePickerBirthday.Date = DateTime.Now;
            ComboBoxPosition.SelectedIndex = -1;

            ButtAddToDB.Visibility = Visibility.Collapsed;
            ButtEditDB.Visibility = Visibility.Collapsed;
        }

        private void EnableEditableElements(bool enabled)
        {
            TextBoxFirstName.IsEnabled = enabled;
            TextBoxSurname.IsEnabled = enabled;
            DatePickerBirthday.IsEnabled = enabled;
            ComboBoxPosition.IsEnabled = enabled;
        }

        // ---------------------------
        // ListView Section
        // ---------------------------
        private void ShowItemsInListView(string sWhere = "", string sOrder = "")
        {
            ListAllItems = new List<Classes.DBClasses.Player>();

            try
            {
                new Classes.DBClasses.Player().ShowItemsInListView(ListViewItems, ListAllItems, sWhere, sOrder);
            }
            catch (Exception)
            {
            }
        }

        private void ListViewItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtEditSelected.IsEnabled = true;
            ButtDeleteDB.IsEnabled = true;

            ButtAddToDB.Visibility = Visibility.Collapsed;
            ButtEditDB.Visibility = Visibility.Collapsed;

            EnableEditableElements(false);

            try
            {
                Classes.DBClasses.Player selectedItem = new Classes.DBClasses.Player().GetSelectedPlayer(e, ListAllItems);

                TextBoxID.Text = selectedItem.nID.ToString();
                TextBoxFirstName.Text = selectedItem.sFirstName;
                TextBoxSurname.Text = selectedItem.sSurname;
                DatePickerBirthday.Date = selectedItem.dBirthday;
                ComboBoxPosition.SelectedIndex = Classes.PageHandling.ComboBoxHandling.GetIDIntoComboBox(ComboBoxPosition, selectedItem.nIDPosition, toRemove);
            }
            catch (Exception)
            {
            }
        }

        // ---------------------------
        // ComboBoxes Section
        // ---------------------------
        private void ShowPositions()
        {
            ListAllPositions = new List<Classes.DBClasses.Position>();
            new Classes.DBClasses.Position().ShowInComboBox(ListAllPositions, ComboBoxPosition, toRemove);
        }

        // ---------------------------
        // Buttons Section
        // ---------------------------
        private void ButtAdd_Click(object sender, RoutedEventArgs e)
        {
            ButtEditSelected.IsEnabled = false;
            ButtDeleteDB.IsEnabled = false;

            TextBoxID.Text = (DataAccess.GetMaxID("tbl_players") + 1).ToString();            
            TextBoxFirstName.Text = "";
            TextBoxSurname.Text = "";
            ComboBoxPosition.SelectedIndex = -1;

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
            DataAccess.RemoveItem(tables, "nIDPlayer", int.Parse(TextBoxID.Text), sTableName, ResetPage);
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
                DateTimeOffset date = (DateTimeOffset)DatePickerBirthday.Date;

                var player = new Classes.DBClasses.Player
                {
                    nID = int.Parse(TextBoxID.Text),
                    sFirstName = TextBoxFirstName.Text,
                    sSurname = TextBoxSurname.Text,
                    dBirthday = new DateTime(date.Year, date.Month, date.Day),
                    nIDUserTeam = DataAccess.NIDActualTeam,
                    nIDPosition = Classes.PageHandling.ComboBoxHandling.GetIDFromComboBox(ComboBoxPosition.SelectedItem, toRemove)
                };

                player.ChangeDB(action);
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
        private void ButtSortBirthday_Click(object sender, RoutedEventArgs e)
        {
            string sOrder = " ORDER BY dBirthday";
            sOrder += bSortBirthdayASC == true ? " ASC" : " DESC";
            ShowItemsInListView("", sOrder);

            bSortBirthdayASC = !bSortBirthdayASC;
        }

        private void TextBoxFindName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string sFindName = TextBoxFindName.Text;
            string sWhere = " AND (sSurname || ' ' || sFirstName LIKE '%" + sFindName + "%')";
            ShowItemsInListView(sWhere);
        }
    }
}
