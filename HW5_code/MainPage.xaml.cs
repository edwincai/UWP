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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.DataTransfer;
using Todos.Models;
using System.Reflection;
using Windows.Storage.Streams;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Todos
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            this.ViewModel = new ViewModels.TodoItemViewModel();
            updateTile();
        }

        ViewModels.TodoItemViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = (ViewModels.TodoItemViewModel)(e.Parameter);
            }
        }

        private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.TodoItem)(e.ClickedItem);
            if (Window.Current.Bounds.Width <= 800)
                Frame.Navigate(typeof(NewPage), ViewModel);
            else
            {
                if (ViewModel.SelectedItem == null)
                {
                    CreateButton.Content = "Create";
                }
                else
                {
                    Title.Text = ViewModel.SelectedItem.title;
                    Details.Text = ViewModel.SelectedItem.description;
                    DatePicker.Date = ViewModel.SelectedItem.day;
                    CreateButton.Content = "Update";
                    // ...
                }
            }
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage), ViewModel);
            
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
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
                else ViewModel.AddTodoItem(Title.Text, Details.Text, DatePicker.Date);
            }
            else ViewModel.UpdateTodoItem(Title.Text, Details.Text, DatePicker.Date);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Title.Text = "";
            Details.Text = "";
            DatePicker.Date = DateTime.Now.Date;
            CreateButton.Content = "Create";
            ViewModel.SelectedItem = null;
        }

        private void updateTile()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();
            foreach (var todo in ViewModel.AllItems)
            {

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(File.ReadAllText("tiles.xml"));
                XmlNodeList text = xml.GetElementsByTagName("text");
                XmlNodeList image = xml.GetElementsByTagName("Image");
                for(int i = 0; i < text.Count; i++)
                {
                    ((XmlElement)text[i]).InnerText = todo.title;
                    i++;
                    ((XmlElement)text[i]).InnerText = todo.description;
                }
                foreach (var element in image)
                    ((XmlElement)element).SetAttribute("src", "Assets\\TileBackground.jpg");

                var notification = new TileNotification(xml);
                updater.Update(notification);
            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
            
            dynamic ori = e.OriginalSource;
            ViewModel.SelectedItem = (TodoItem)ori.DataContext;
            ShareSourceLoad(ViewModel.SelectedItem);

        }
        private void ShareSourceLoad(TodoItem selected)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.onShareDataRequested);
        }

        private void onShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dp = args.Request.Data;
            var def = args.Request.GetDeferral();
            var photo = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/background.jpg"));
            
            
            dp.Properties.Title = ViewModel.SelectedItem.title;
            dp.Properties.Description = ViewModel.SelectedItem.description;
            dp.SetBitmap(photo);
            dp.SetText(ViewModel.SelectedItem.title);
            def.Complete();
            ;
        }
    }
}
