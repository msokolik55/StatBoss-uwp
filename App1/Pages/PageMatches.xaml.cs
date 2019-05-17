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
    public sealed partial class PageMatches : Page
    {
        private string sTableName = "tbl_matches";
        List<Classes.DBClasses.Match> ListAllItems;

        private string toRemove = "opponent_";
        List<Classes.DBClasses.Opponent> ListAllOpponents;

        string[] tables = { "tbl_stats" };

        private bool bSortDateASC = true;

        public PageMatches()
        {
            this.InitializeComponent();

            ResetPage();
            ShowOpponents();

            TextBoxID.IsEnabled = false;
        }

        // ---------------------------
        // Page Functions
        // ---------------------------
        private void ResetPage()
        {
            ButtAdd.IsEnabled = (StatBoss.Classes.MainVariables.NIDActualSeason > 0 && StatBoss.Classes.MainVariables.NIDActualTeam > 0) ? true : false;
            ButtEditSelected.IsEnabled = false;
            ButtDeleteDB.IsEnabled = false;

            Classes.PageHandling.ListViewHandling.ResetListView(ListViewItems);
            ShowItemsInListView();

            EnableEditableElements(false);
            TextBoxID.Text = "";
            CheckBoxPlayed.IsChecked = false;
            ComboBoxOpponent.SelectedIndex = -1;
            TimePicker.Time = TimeSpan.Zero;
            DatePicker.Date = DateTime.Now;
            TextBoxPlace.Text = "";
            CheckBoxHome.IsChecked = false;
            TextBoxGive.Text = "";
            TextBoxGive1.Text = "";
            TextBoxGive2.Text = "";
            TextBoxGive3.Text = "";
            TextBoxReceived.Text = "";
            TextBoxReceived1.Text = "";
            TextBoxReceived2.Text = "";
            TextBoxReceived3.Text = "";
            TextBoxDescription.Text = "";

            ButtAddToDB.Visibility = Visibility.Collapsed;
            ButtEditDB.Visibility = Visibility.Collapsed;
        }

        private void EnableEditableElements(bool enabled)
        {
            CheckBoxPlayed.IsEnabled = enabled;
            ComboBoxOpponent.IsEnabled = enabled;
            TimePicker.IsEnabled = enabled;
            DatePicker.IsEnabled = enabled;
            TextBoxPlace.IsEnabled = enabled;
            CheckBoxHome.IsEnabled = enabled;
            TextBoxGive.IsEnabled = enabled;
            TextBoxGive1.IsEnabled = enabled;
            TextBoxGive2.IsEnabled = enabled;
            TextBoxGive3.IsEnabled = enabled;
            TextBoxReceived.IsEnabled = enabled;
            TextBoxReceived1.IsEnabled = enabled;
            TextBoxReceived2.IsEnabled = enabled;
            TextBoxReceived3.IsEnabled = enabled;
            TextBoxDescription.IsEnabled = enabled;
        }

        // ---------------------------
        // ListView Section
        // ---------------------------
        private void ShowItemsInListView(string sWhere = "", string sOrder = "")
        {
            ListAllItems = new List<Classes.DBClasses.Match>();

            try
            {
                new Classes.DBClasses.Match().ShowItemsInListView(ListViewItems, ListAllItems, sWhere, sOrder);
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
                Classes.DBClasses.Match selectedItem = new Classes.DBClasses.Match().GetSelectedMatch(e, ListAllItems);

                TextBoxID.Text = selectedItem.nID.ToString();
                CheckBoxPlayed.IsChecked = selectedItem.bPlayed;
                ComboBoxOpponent.SelectedIndex = Classes.PageHandling.ComboBoxHandling.GetIDIntoComboBox(ComboBoxOpponent, selectedItem.nIDOpponent, toRemove);
                TimePicker.Time = selectedItem.dDateTime.TimeOfDay;
                DatePicker.Date = selectedItem.dDateTime;
                TextBoxPlace.Text = selectedItem.sMatchPlace;

                CheckBoxHome.IsChecked = selectedItem.bHome;

                TextBoxGive.Text = selectedItem.nGive.ToString();
                TextBoxReceived.Text = selectedItem.nReceived.ToString();
                TextBoxGive1.Text = selectedItem.nGive1.ToString();
                TextBoxReceived1.Text = selectedItem.nReceived1.ToString();
                TextBoxGive2.Text = selectedItem.nGive2.ToString();
                TextBoxReceived2.Text = selectedItem.nReceived2.ToString();
                TextBoxGive3.Text = selectedItem.nGive3.ToString();
                TextBoxReceived3.Text = selectedItem.nReceived3.ToString();
                TextBoxDescription.Text = selectedItem.sMatchDescription;
            }
            catch (Exception)
            {
            }
        }

        // ---------------------------
        // ComboBoxes Section
        // ---------------------------
        private void ShowOpponents()
        {
            ListAllOpponents = new List<Classes.DBClasses.Opponent>();
            new Classes.DBClasses.Opponent().ShowInComboBox(ListAllOpponents, ComboBoxOpponent, toRemove);
        }

        // ---------------------------
        // Buttons Section
        // ---------------------------
        private void ButtAdd_Click(object sender, RoutedEventArgs e)
        {
            ButtEditSelected.IsEnabled = false;
            ButtDeleteDB.IsEnabled = false;

            TextBoxID.Text = (DataAccess.GetMaxID(sTableName) + 1).ToString();
            CheckBoxPlayed.IsChecked = false;
            ComboBoxOpponent.SelectedIndex = -1;
            TimePicker.Time = TimeSpan.Zero;
            DatePicker.Date = DateTime.Now;
            TextBoxPlace.Text = "";
            CheckBoxHome.IsChecked = false;
            TextBoxGive.Text = "0";
            TextBoxGive1.Text = "0";
            TextBoxGive2.Text = "0";
            TextBoxGive3.Text = "0";
            TextBoxReceived.Text = "0";
            TextBoxReceived1.Text = "0";
            TextBoxReceived2.Text = "0";
            TextBoxReceived3.Text = "0";
            TextBoxDescription.Text = " ";

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
            DataAccess.RemoveItem(tables, "nIDMatch", int.Parse(TextBoxID.Text), sTableName, ResetPage);
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
                TimeSpan time = TimePicker.Time;
                DateTimeOffset dDateTimeOff = (DateTimeOffset)DatePicker.Date;
                DateTime dDateTime = new DateTime(dDateTimeOff.Year, dDateTimeOff.Month, dDateTimeOff.Day, time.Hours, time.Minutes, time.Seconds);

                var match = new Classes.DBClasses.Match
                {
                    nID = int.Parse(TextBoxID.Text),
                    nIDSeason = StatBoss.Classes.MainVariables.NIDActualSeason,
                    bPlayed = (bool)CheckBoxPlayed.IsChecked,
                    nIDUserTeam = StatBoss.Classes.MainVariables.NIDActualTeam,
                    nIDOpponent = Classes.PageHandling.ComboBoxHandling.GetIDFromComboBox(ComboBoxOpponent.SelectedItem, toRemove),
                    dDateTime = dDateTime,

                    sMatchPlace = TextBoxPlace.Text,
                    bHome = (bool)CheckBoxHome.IsChecked,

                    nGive = int.Parse(TextBoxGive.Text),
                    nReceived = int.Parse(TextBoxReceived.Text),
                    nGive1 = int.Parse(TextBoxGive1.Text),
                    nReceived1 = int.Parse(TextBoxReceived1.Text),
                    nGive2 = int.Parse(TextBoxGive2.Text),
                    nReceived2 = int.Parse(TextBoxReceived2.Text),
                    nGive3 = int.Parse(TextBoxGive3.Text),
                    nReceived3 = int.Parse(TextBoxReceived3.Text),
                    sMatchDescription = TextBoxDescription.Text
            };

                match.ChangeDB(action);
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
        private void ButtSortDate_Click(object sender, RoutedEventArgs e)
        {
            string sOrder = " ORDER BY dDatetime";
            sOrder += bSortDateASC == true ? " ASC" : " DESC";
            ShowItemsInListView("", sOrder);

            bSortDateASC = !bSortDateASC;
        }

        private void TextBoxFindName_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void CheckBoxFilterPlayed_Checked(object sender, RoutedEventArgs e)
        {
            string sWhere = " AND bPlayed = '1'";
            ShowItemsInListView(sWhere);
        }

        private void CheckBoxFilterPlayed_Unchecked(object sender, RoutedEventArgs e)
        {
            string sWhere = " AND bPlayed = '0'";
            ShowItemsInListView(sWhere);
        }
    }
}
