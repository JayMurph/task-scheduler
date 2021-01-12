﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using task_scheduler_presentation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using task_scheduler_presentation.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskPage : Page, ITasksView{
        public ObservableCollection<TaskItemModel> TaskItems { get; set; } = new ObservableCollection<TaskItemModel>();

        //list of observable task models
        public TaskPage() {
            this.InitializeComponent();

            App.UserController.ViewTasks(this);

            //should this be done by the controller?????????????
            this.taskListView.ItemsSource = TaskItems;
        }

        public event EventHandler Closing;
        private void OnClosing() {
            Closing?.Invoke(this, null);
        }

        public void TaskCreatedCallback(object source, TaskItemModel taskItemModel) {
            TaskItems.Add(taskItemModel);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            Debug.WriteLine("Navigating From");
            OnClosing();
            base.OnNavigatingFrom(e);
        }
    }
}