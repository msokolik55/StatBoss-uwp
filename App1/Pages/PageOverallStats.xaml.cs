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
        List<App1.Classes.DBClasses.OverallStat> ListAllItems;

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
        private void ShowItemsInListView(string sWhere = "", string sOrder = "")
        {
            ListAllItems = new List<App1.Classes.DBClasses.OverallStat>();

            try
            {
                new App1.Classes.DBClasses.OverallStat().ShowItemsInListView(ListViewItems, ListAllItems, sWhere, sOrder);
            }
            catch (Exception)
            {
            }
        }

        private void ListViewItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                App1.Classes.DBClasses.OverallStat selectedItem = new App1.Classes.DBClasses.OverallStat().GetSelectedOverallStat(e, ListAllItems);

                TextBoxFirstName.Text = selectedItem.sFirstName;
                TextBoxSurname.Text = selectedItem.sSurname;
                TextBoxMatches.Text = selectedItem.nMatches.ToString();
                TextBoxMinutes.Text = selectedItem.nMinutes.ToString();
                TextBoxGoals.Text = selectedItem.nGoals.ToString();
                TextBoxAssists.Text = selectedItem.nAssists.ToString();
                TextBoxPenalties.Text = selectedItem.nPenalties.ToString();
                TextBoxRedCards.Text = selectedItem.nRedCards.ToString();
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
            string sFindName = TextBoxFindName.Text;
            string sWhere = " AND (p.sSurname || ' ' || p.sFirstName LIKE '%" + sFindName + "%')";
            ShowItemsInListView(sWhere);
        }

        private void ButtSortGoals_Click(object sender, RoutedEventArgs e)
        {
            string sOrder = " ORDER BY nGoals";
            sOrder += bSortGoalsASC == true ? " ASC" : " DESC";
            ShowItemsInListView("", sOrder);

            bSortGoalsASC = !bSortGoalsASC;
            bSortAssistsASC = true;
        }

        private void ButtSortAssists_Click(object sender, RoutedEventArgs e)
        {
            string sOrder = " ORDER BY nAssists";
            sOrder += bSortAssistsASC == true ? " ASC" : " DESC";
            ShowItemsInListView("", sOrder);

            bSortGoalsASC = true;
            bSortAssistsASC = !bSortAssistsASC;
        }
    }

}
