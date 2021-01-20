using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// The 'outer-page' for the application. Holds a navigation bar and a the Add Task (+) button.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        #region NavigationView event handlers

        /// <summary>
        /// Occurs when the Page's NavigationView is loaded. Sets the first page of the app (the
        /// Notification Page) as the current page for the NavigationView and navigates to it.
        /// </summary>
        /// <param name="sender">
        /// Initiator of the event. unused
        /// </param>
        /// <param name="e">
        /// Arguments for the event. unused.
        /// </param>
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

        /// <summary>
        /// Occurs when an item is selected from the page's NavigationView. Initiates navigation to
        /// the page represented by the invoked item.
        /// </summary>
        /// <param name="sender">
        /// The NavigationView that had an item invoked
        /// </param>
        /// <param name="args">
        /// Arguments for the event. Used for retrieving the InvokedItem.
        /// </param>
        private void navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {

            //get the textBlock that was clicked on in the NavigationView
            TextBlock ItemContent = args.InvokedItem as TextBlock;

            if (ItemContent != null) {
                //examine the tag element of the TextBlock to determine which page to navigate to
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
