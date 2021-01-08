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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace task_scheduler_presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        #region NavigationView event handlers
        private void navigation_Loaded(object sender, RoutedEventArgs e) {
            // set the initial SelectedItem
            foreach (NavigationViewItemBase item in navigation.MenuItems) {
                if (item is NavigationViewItem && item.Tag.ToString() == "Notification_Page") {
                    navigation.SelectedItem = item;
                    break;
                }
            }
            contentFrame.Navigate(typeof(Views.NotificationPage));
        }

        private void navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {

            TextBlock ItemContent = args.InvokedItem as TextBlock;

            if (ItemContent != null) {
                switch (ItemContent.Tag) {
                    case "Nav_Task":
                        contentFrame.Navigate(typeof(Views.TaskPage));
                        break;

                    case "Nav_Notification":
                        contentFrame.Navigate(typeof(Views.NotificationPage));
                        break;
                }
            }
        }
        #endregion

    }
}
