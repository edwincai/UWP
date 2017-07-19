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
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage), "");
        }

        private void checkbox_Click(object sender, RoutedEventArgs e)
        {
            if (line.Opacity == 0) line.Opacity = 100;
            else line.Opacity = 0;
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            if (line2.Opacity == 0) line2.Opacity = 100;
            else line2.Opacity = 0;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TheWorkInProgress"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] as ApplicationDataCompositeValue;
                    line.Opacity = Convert.ToDouble(composite["line"]);
                    line2.Opacity = Convert.ToDouble(composite["line2"]);
                    checkbox1.IsChecked = Convert.ToBoolean(composite["checkbox1"]);
                    checkbox2.IsChecked = Convert.ToBoolean(composite["checkbox2"]);
                }
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // base.OnNavigatedFrom(e);
            bool suspending = ((App)App.Current).issuspend;
            if (suspending)
            {
                var composite = new ApplicationDataCompositeValue();
                composite["line"] = line.Opacity.ToString();
                composite["line2"] = line2.Opacity.ToString();
                composite["checkbox1"] = checkbox1.IsChecked.ToString();
                composite["checkbox2"] = checkbox2.IsChecked.ToString();
                ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] = composite;
            }
        }
    }
}
