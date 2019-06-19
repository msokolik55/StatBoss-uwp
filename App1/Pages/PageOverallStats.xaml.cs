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

namespace StatBoss.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageOverallStats : Page
    {
        List<App1.Classes.DBClasses.DBOverallStat> ListAllItems;

        private bool bSortGoalsASC = true;
        private bool bSortAssistsASC = true;

        public PageOverallStats()
        {
            this.InitializeComponent();

            ShowItemsInListView();
        }

        // ---------------------------
        // ListView Section
        // ---------------------------
        private void ShowItemsInListView(string sWhere = "", string sOrder = "", bool bASC = true)
        {
            ListAllItems = new List<App1.Classes.DBClasses.DBOverallStat>();

            try
            {
                Classes.UIClasses.UIOverallStat.ShowItemsInListView(ListViewItems, ListAllItems, sWhere, sOrder, bASC);
            }
            catch (Exception)
            {
            }
        }

        private void ListViewItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                App1.Classes.DBClasses.DBOverallStat selectedItem = Classes.UIClasses.UIOverallStat.GetSelectedOverallStat(e, ListAllItems);

                TextBoxFirstName.Text = selectedItem.sFirstName;
                TextBoxSurname.Text = selectedItem.sSurname;
                TextBoxMatches.Text = selectedItem.nMatches.ToString();
                TextBoxMinutes.Text = selectedItem.nMinutes.ToString();
                TextBoxGoals.Text = selectedItem.nGoals.ToString();
                TextBoxAssists.Text = selectedItem.nAssists.ToString();
                TextBoxPenalties.Text = selectedItem.nPenalties.ToString();
                TextBoxRedCards.Text = selectedItem.nRedCards.ToString();
                TextBoxPlusMinus.Text = selectedItem.nPlusMinus.ToString();
            }
            catch (Exception)
            {
            }
        }

        // ---------------------------
        // Filters and Sorts Section
        // ---------------------------
        private void TextBoxFindName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowItemsInListView(TextBoxFindName.Text);
        }

        private void ButtSortGoals_Click(object sender, RoutedEventArgs e)
        {
            ShowItemsInListView("", "nGoals", bSortGoalsASC);

            bSortGoalsASC = !bSortGoalsASC;
            bSortAssistsASC = true;
        }

        private void ButtSortAssists_Click(object sender, RoutedEventArgs e)
        {
            ShowItemsInListView("", "nAssists", bSortAssistsASC);

            bSortGoalsASC = true;
            bSortAssistsASC = !bSortAssistsASC;
        }
    }
}
