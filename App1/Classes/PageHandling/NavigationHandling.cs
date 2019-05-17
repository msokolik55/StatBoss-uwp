using App1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StatBoss.Classes.PageHandling
{
    public static class NavigationHandling
    {
        public static void InitializateFrame(Frame contentFrame)
        {
            contentFrame.Navigate(typeof(Pages.PageInstructions));
        }

        public static void NavigateTo(NavigationViewItemInvokedEventArgs args, Frame contentFrame, NavigationView navMain)
        {
            switch (args.InvokedItem)
            {
                case "Instructions":
                    contentFrame.Navigate(typeof(Pages.PageInstructions));
                    break;

                case "Overall Stats":
                    contentFrame.Navigate(typeof(Pages.PageOverallStats));
                    break;

                case "Players":
                    contentFrame.Navigate(typeof(PagePlayers));
                    break;

                case "Matches":
                    contentFrame.Navigate(typeof(App1.Pages.PageMatches));
                    break;

                case "Stats in Matches":
                    contentFrame.Navigate(typeof(App1.Pages.PageMatchesEdit));
                    break;

                case "Seasons":
                    contentFrame.Navigate(typeof(App1.Pages.PageSeasons));
                    break;

                case "Teams":
                    contentFrame.Navigate(typeof(App1.Pages.PageTeams));
                    break;

                case "Positions":
                    contentFrame.Navigate(typeof(App1.Pages.PagePositions));
                    break;

                case "Opponents":
                    contentFrame.Navigate(typeof(App1.Pages.PageOpponents));
                    break;

                case "About":
                    App1.Classes.PageHandling.DialogsHandling.DisplayAbout();
                    break;
            }

            if (!args.InvokedItem.Equals("About")) { navMain.Header = args.InvokedItem; }
        }
    }
}
