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
    public sealed partial class PageMatchesEdit : Page
    {
        private Classes.DBClasses.DBMatch actualMatch;

        private string sTableName = "tbl_stats";
        List<Classes.DBClasses.DBStat> ListAllItems;

        private string toRemoveMatch = "match_";
        private string toRemovePos = "position_";
        private string toRemovePl = "player_";

        List<Classes.DBClasses.DBMatch> ListAllMatches;
        List<Classes.DBClasses.DBPosition> ListAllPositions;
        List<Classes.DBClasses.DBPlayer> ListAllPlayers;

        private bool bSortNumberASC = true;
        private bool bSortGoalsASC = true;
        private bool bSortAssistsASC = true;

        public PageMatchesEdit()
        {
            this.InitializeComponent();

            ResetPage();

            ShowMatches();
            ShowPositions();

            TextBoxID.IsEnabled = false;

            TextBlock block = new TextBlock
            {
                Text = "Select match from ComboBox under top buttons pane."
            };
            ListViewItems.Items.Add(block);
            ListViewItems.IsEnabled = false;
        }

        // ---------------------------
        // Page Functions
        // ---------------------------
        private void ResetPage()
        {
            if (ComboBoxMatch.SelectedIndex > -1) { ButtAdd.IsEnabled = true; }
            ButtEditSelected.IsEnabled = false;
            ButtDeleteDB.IsEnabled = false;

            Classes.PageHandling.ListViewHandling.ResetListView(ListViewItems);
            ShowItemsInListView();
            ShowPlayers();

            EnableEditableElements(false, true);
            TextBoxID.Text = "";
            ComboBoxPlayer.SelectedIndex = -1;
            ComboBoxPosition.SelectedIndex = -1;
            TextBoxNumber.Text = "";
            TextBoxMinutes.Text = "";
            TextBoxGoals.Text = "";
            TextBoxAssists.Text = "";
            TextBoxPenalties.Text = "";
            TextBoxRedCards.Text = "";
            TextBoxPlusMinus.Text = "";
            TextBoxComment.Text = "";

            ButtAddToDB.Visibility = Visibility.Collapsed;
            ButtEditDB.Visibility = Visibility.Collapsed;
        }

        private void EnableEditableElements(bool enabled, bool changeComboPlayer = false)
        {
            if (changeComboPlayer) { ComboBoxPlayer.IsEnabled = enabled; }
            ComboBoxPosition.IsEnabled = enabled;
            TextBoxNumber.IsEnabled = enabled;
            TextBoxMinutes.IsEnabled = enabled;
            TextBoxGoals.IsEnabled = enabled;
            TextBoxAssists.IsEnabled = enabled;
            TextBoxPenalties.IsEnabled = enabled;
            TextBoxRedCards.IsEnabled = enabled;
            TextBoxPlusMinus.IsEnabled = enabled;
            TextBoxComment.IsEnabled = enabled;

            ButtGoalsPlus.IsEnabled = enabled;
            ButtGoalsMinus.IsEnabled = enabled;
            ButtAssistsPlus.IsEnabled = enabled;
            ButtAssistsMinus.IsEnabled = enabled;
            ButtPenaltiesPlus.IsEnabled = enabled;
            ButtPenaltiesMinus.IsEnabled = enabled;
            ButtRedCardsPlus.IsEnabled = enabled;
            ButtRedCardsMinus.IsEnabled = enabled;
        }

        // ---------------------------
        // ListView Section
        // ---------------------------
        private void ShowItemsInListView(string sWhere = "", string sOrder = "", bool bASC = true)
        {
            ListAllItems = new List<Classes.DBClasses.DBStat>();

            try
            {
                StatBoss.Classes.UIClasses.UIStat.ShowItemsInListView(ListViewItems, ListAllItems, actualMatch.nID, sWhere, sOrder, bASC);
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

            EnableEditableElements(false, true);

            try
            {
                Classes.DBClasses.DBStat selectedItem = StatBoss.Classes.UIClasses.UIStat.GetSelectedStat(e, ListAllItems);

                TextBoxID.Text = selectedItem.nID.ToString();
                StatBoss.Classes.UIClasses.UIPlayer.ShowSpecificPlayerInComboBox(ComboBoxPlayer, selectedItem.nIDPlayer, toRemovePl);
                ComboBoxPosition.SelectedIndex = Classes.PageHandling.ComboBoxHandling.GetIDIntoComboBox(ComboBoxPosition, selectedItem.nIDPosition, toRemovePos);
                TextBoxNumber.Text = selectedItem.nNumber.ToString();
                TextBoxMinutes.Text = selectedItem.nMinutes.ToString();
                TextBoxGoals.Text = selectedItem.nGoals.ToString();
                TextBoxAssists.Text = selectedItem.nAssists.ToString();
                TextBoxPenalties.Text = selectedItem.nPenalties.ToString();
                TextBoxRedCards.Text = selectedItem.nRedCards.ToString();
                TextBoxPlusMinus.Text = selectedItem.nPlusMinus.ToString();
                TextBoxComment.Text = selectedItem.sComment.ToString();
            }
            catch (Exception)
            {
            }
        }

        // ---------------------------
        // ComboBoxes Section
        // ---------------------------
        private void ShowMatches()
        {
            ListAllMatches = new List<Classes.DBClasses.DBMatch>();
            StatBoss.Classes.UIClasses.UIMatch.ShowInComboBox(ListAllMatches, ComboBoxMatch, toRemoveMatch);
        }

        private void ShowPlayers()
        {
            ListAllPlayers = new List<Classes.DBClasses.DBPlayer>();

            try
            { 
                StatBoss.Classes.UIClasses.UIPlayer.ShowUnusedPLayersInComboBox(ListAllPlayers, ComboBoxPlayer, toRemovePl, actualMatch.nID);
            }
            catch (Exception)
            {
            }
        }

        private void ShowPositions()
        {
            ListAllPositions = new List<Classes.DBClasses.DBPosition>();
            StatBoss.Classes.UIClasses.UIPosition.ShowInComboBox(ListAllPositions, ComboBoxPosition, toRemovePos);
        }

        private void ComboBoxMatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetPage();

            actualMatch = StatBoss.Classes.UIClasses.UIMatch.GetSelectedMatchFromComboBox(e, ListAllMatches, ComboBoxMatch, toRemoveMatch);
            ListViewItems.IsEnabled = true;
            ShowItemsInListView();

            ShowPlayers();
        }

        // ---------------------------
        // Buttons Section
        // ---------------------------
        private void ButtAdd_Click(object sender, RoutedEventArgs e)
        {
            ButtEditSelected.IsEnabled = false;
            ButtDeleteDB.IsEnabled = false;

            ListViewItems.SelectedIndex = -1;
            Classes.PageHandling.ComboBoxHandling.ResetComboBox(ComboBoxPlayer);
            ShowPlayers();

            TextBoxID.Text = (DataAccess.GetMaxID(sTableName) + 1).ToString();
            ComboBoxPlayer.SelectedIndex = -1;
            ComboBoxPosition.SelectedIndex = -1;
            TextBoxNumber.Text = "0";
            TextBoxMinutes.Text = "0";
            TextBoxGoals.Text = "0";
            TextBoxAssists.Text = "0";
            TextBoxPenalties.Text = "0";
            TextBoxRedCards.Text = "0";
            TextBoxPlusMinus.Text = "0";
            TextBoxComment.Text = " ";

            ButtAddToDB.Visibility = Visibility.Visible;
            ButtEditDB.Visibility = Visibility.Collapsed;

            EnableEditableElements(true, true);
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
            Classes.PageHandling.DialogsHandling.DisplayDeleteItemDialog(sTableName, int.Parse(TextBoxID.Text), ResetPage, false);
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
                var stat = new Classes.DBClasses.DBStat
                {
                    nID = int.Parse(TextBoxID.Text),
                    nIDSeason = StatBoss.Classes.MainVariables.NIDActualSeason,
                    nIDMatch = actualMatch.nID,
                    nIDUserTeam = StatBoss.Classes.MainVariables.NIDActualTeam,
                    nIDPlayer = Classes.PageHandling.ComboBoxHandling.GetIDFromComboBox(ComboBoxPlayer.SelectedItem, toRemovePl),
                    nIDPosition = Classes.PageHandling.ComboBoxHandling.GetIDFromComboBox(ComboBoxPosition.SelectedItem, toRemovePos),
                    nNumber = int.Parse(TextBoxNumber.Text),
                    nMinutes = int.Parse(TextBoxMinutes.Text),
                    nGoals = int.Parse(TextBoxGoals.Text),
                    nAssists = int.Parse(TextBoxAssists.Text),
                    nPenalties = int.Parse(TextBoxPenalties.Text),
                    nRedCards = int.Parse(TextBoxRedCards.Text),
                    nPlusMinus = int.Parse(TextBoxPlusMinus.Text),
                    sComment = TextBoxComment.Text
                };

                stat.ChangeDB(action);
                ResetPage();
            }
            else
            {
                Classes.PageHandling.DialogsHandling.DisplayNoCorrectFields();
            }
        }

        private void ButtGoalsPlus_Click(object sender, RoutedEventArgs e)
        {
            UpdateNumberValue(TextBoxGoals, true);
        }

        private void ButtGoalsMinus_Click(object sender, RoutedEventArgs e)
        {
            UpdateNumberValue(TextBoxGoals, false);
        }

        private void ButtAssistsPlus_Click(object sender, RoutedEventArgs e)
        {
            UpdateNumberValue(TextBoxAssists, true);
        }

        private void ButtAssistsMinus_Click(object sender, RoutedEventArgs e)
        {
            UpdateNumberValue(TextBoxAssists, false);
        }

        private void ButtPenaltiesPlus_Click(object sender, RoutedEventArgs e)
        {
            UpdateNumberValue(TextBoxPenalties, true);
        }

        private void ButtPenaltiesMinus_Click(object sender, RoutedEventArgs e)
        {
            UpdateNumberValue(TextBoxPenalties, false);
        }

        private void ButtRedCardsPlus_Click(object sender, RoutedEventArgs e)
        {
            UpdateNumberValue(TextBoxRedCards, true);
        }

        private void ButtRedCardsMinus_Click(object sender, RoutedEventArgs e)
        {
            UpdateNumberValue(TextBoxRedCards, false);
        }

        private void ButtPlusMinusPlus_Click(object sender, RoutedEventArgs e)
        {
            UpdateNumberValue(TextBoxPlusMinus, true, true);
        }

        private void ButtPlusMinusMinus_Click(object sender, RoutedEventArgs e)
        {
            UpdateNumberValue(TextBoxPlusMinus, false, true);
        }

        private void UpdateNumberValue(TextBox box, bool bPlus, bool bNegative = false)
        {
            if (int.TryParse(box.Text, out int code))
            {
                int number = int.Parse(box.Text);
                number = bPlus ? number + 1 : number - 1;
                if (!bNegative) { number = (number < 0) ? 0 : number; }

                box.Text = number.ToString();
            }
        }

        // ---------------------------
        // Filters and Sorts Section
        // ---------------------------
        private void ButtSortNumber_Click(object sender, RoutedEventArgs e)
        {
            ShowItemsInListView("", "s.nNumber", bSortNumberASC);

            bSortNumberASC = !bSortNumberASC;
            bSortGoalsASC = true;
            bSortAssistsASC = true;
        }

        private void ButtSortGoals_Click(object sender, RoutedEventArgs e)
        {
            ShowItemsInListView("", "s.nGoals", bSortGoalsASC);

            bSortNumberASC = true;
            bSortGoalsASC = !bSortGoalsASC;
            bSortAssistsASC = true;
        }

        private void ButtSortAssists_Click(object sender, RoutedEventArgs e)
        {
            ShowItemsInListView("", "s.nAssistance", bSortAssistsASC);

            bSortNumberASC = true;
            bSortGoalsASC = true;
            bSortAssistsASC = !bSortAssistsASC;
        }

        private void TextBoxFindName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowItemsInListView(TextBoxFindName.Text);
        }
    }
}

