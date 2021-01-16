﻿using System;
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

using task_scheduler_presentation.Models;

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// Page where Notifications generated by TaskItems are displayed
    /// </summary>
    public sealed partial class NotificationPage : Page {
        public NotificationPage() {
            this.InitializeComponent();

            this.notificationListView.ItemsSource = new List<NotificationModel> { new NotificationModel { Title = "Test", Time = DateTime.Now, Color = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)) } };
        }
    }
}
