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
using task_scheduler_presentation.Views;
using Windows.UI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// Page for creating a new TaskItem
    /// </summary>
    //AddTaskButtonControl implements IAddTaskView but uses its AddTaskFlyoutControl as a
    //surrogate for actually handling user interaction.
    public sealed partial class AddTaskButtonControl : UserControl , IAddTaskView{
        public AddTaskButtonControl() {
            InitializeComponent();

            addTaskControl.Owner = this;

            ClearFields();
        }

        public string Title { get => addTaskControl.Title; }
        public string Description { get => addTaskControl.Description; }
        public DateTime StartTime { get => addTaskControl.StartTime; }
        public Color Color { get => addTaskControl.Color; }
        public string FrequencyType { get => addTaskControl.SelectedFrequencyType; }
        public TimeSpan CustomFrequency { get => addTaskControl.CustomFrequency; }
        public string ApplicationErrorMessage { set => addTaskControl.ApplicationErrorMessage = value; }
        public bool ApplicationError { set => addTaskControl.ApplicationError = value; }

        public void ClearFields() {
            //set fields of add task control back to defaults
            addTaskControl?.ClearFields();
        }

        public void CloseSelf() {
            addTaskFlyout?.Hide();
        }
    }
}
