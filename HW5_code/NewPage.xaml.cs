using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;


namespace Todos
{
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
        }

        private ViewModels.TodoItemViewModel ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var i = new MessageDialog("Welcome!").ShowAsync();
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
            ViewModel = ((ViewModels.TodoItemViewModel)e.Parameter);
            if (ViewModel.SelectedItem == null)
            {
                createButton.Content = "Create";
                //var i = new MessageDialog("Welcome!").ShowAsync();
            }
            else
            {
                createButton.Content = "Update";
                Title.Text = ViewModel.SelectedItem.title;
                Details.Text = ViewModel.SelectedItem.description;
                DatePicker.Date = ViewModel.SelectedItem.day;
                // ...
            }
        }
        private void CreateButton_Clicked(object sender, RoutedEventArgs e)
        {
            // check the textbox and datapicker
            if (ViewModel.SelectedItem == null)
            {
                if (Title.Text == "")
                {
                    var K = new MessageDialog("Title error!").ShowAsync();
                }
                if (Details.Text == "")
                {
                    var K = new MessageDialog("details error!").ShowAsync();
                }
                if (DatePicker.Date < DateTime.Now.Date)
                {
                    var K = new MessageDialog("dates error!").ShowAsync();
                }
                else
                {
                    ViewModel.AddTodoItem(Title.Text, Details.Text, DatePicker.Date);
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
            // if ok
            else UpdateButton_Clicked(sender, e);
            //
        }
        private  void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                if (ViewModel.SelectedItem != null)
                {
                    ViewModel.RemoveTodoItem(Title.Text);
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
        }


        private void UpdateButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                // check then update
                if (Title.Text == "")
                {
                    var K = new MessageDialog("Title error!").ShowAsync();
                }
                if (Details.Text == "")
                {
                    var K = new MessageDialog("details error!").ShowAsync();
                }
                if (DatePicker.Date < DateTime.Now.Date)
                {
                    var K = new MessageDialog("dates error!").ShowAsync();
                }
                else
                {
                    ViewModel.UpdateTodoItem(Title.Text, Details.Text, DatePicker.Date);
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
        }
    }
}
