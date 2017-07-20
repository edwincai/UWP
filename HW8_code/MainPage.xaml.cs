using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace mediaplayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaPlayer _mediaPlayer = new MediaPlayer();
        //TimelineController 属性设置为时间线控制器对象。
        MediaTimelineController _mediaTimelineController;
        TimeSpan duration;
        MediaSource mediaSource;
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        //在 PositionChanged 的处理程序中，更新滑块值以反映时间线控制器的当前位置。
        private async void _mediaTimelineController_PositionChanged(MediaTimelineController sender, object args)
        {
            if (duration != TimeSpan.Zero)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    //timeLine.Value = sender.Position.TotalSeconds / (float)duration.TotalSeconds;
                    timeLine.Value = sender.Position.TotalSeconds;
                });
            }
        }


        //使用 OpenOperationCompleted 处理程序有机会发现媒体源内容的持续时间。 
        //确定持续时间后，Slider 控件的最大值设置为媒体项的总秒数。 
        //此值在 RunAsync 调用中设置，以确保它在 UI 线程上运行
        private async void MediaSource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            duration = sender.Duration.GetValueOrDefault();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
             {
                 timeLine.Minimum = 0;
                 timeLine.Maximum = duration.TotalSeconds;
                 timeLine.StepFrequency = 1;
             });
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaTimelineController == null)
            {
                var K = new MessageDialog("Please Pickup a file!").ShowAsync();
            }
            else
            {
                _mediaTimelineController.Start();
            }
        }

        private void resume_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaTimelineController == null)
            {
                var K = new MessageDialog("Please Pickup a file!").ShowAsync();
            }
            else
            {
                if (_mediaTimelineController.State == MediaTimelineControllerState.Running)
                {
                    _mediaTimelineController.Pause();
                    resume.Content = "Resume";
                }
                else
                {
                    _mediaTimelineController.Resume();
                    resume.Content = "Pause";
                }
            }
        }

        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            if(slider != null)
            {
                double value = slider.Value / 100;
                _mediaPlayer.Volume = value;
               
            }
        }

        //拖动进度条
        private void timeLine_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
           Slider slider = sender as Slider;
            if (slider != null)
            {
                TimeSpan value = TimeSpan.FromSeconds(slider.Value);
                _mediaPlayer.TimelineController.Position = value;

            }

        }

        //显示声控按钮
        private void Volume_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if(element != null)
            {
                FlyoutBase.ShowAttachedFlyout(element);
            }
        }
        
        //全屏效果
        private void fullScreen_Click(object sender, RoutedEventArgs e)
        {
            
            var view = ApplicationView.GetForCurrentView();
            //_mediaPlayerElement.IsFullWindow = !_mediaPlayerElement.IsFullWindow;
            if (view.IsFullScreenMode)
            {
               
                view.ExitFullScreenMode();
                command.Visibility = Visibility.Visible;
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
               
            }
            else
            {
                if (view.TryEnterFullScreenMode())
                {
                    command.Visibility = Visibility.Visible;
                    ApplicationView.PreferredLaunchWindowingMode =
                        ApplicationViewWindowingMode.FullScreen;
                   
                }
            }
             
        }

        private async void openFile_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new Windows.Storage.Pickers.FileOpenPicker();

            //Add filetype filters.  In this case wmv and mp4.
            filePicker.FileTypeFilter.Add(".wmv");
            filePicker.FileTypeFilter.Add(".mp4");
            filePicker.FileTypeFilter.Add(".mkv");
            filePicker.FileTypeFilter.Add(".avi");
            filePicker.FileTypeFilter.Add(".mp3");

            //Set picker start location to the video library
            filePicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;

            //Retrieve file from picker
            StorageFile file = await filePicker.PickSingleFileAsync();

            if (file != null)
            {
                mediaSource = MediaSource.CreateFromStorageFile(file);
            }
            else
            {
                var K = new MessageDialog("Please Pickup a file!").ShowAsync();
            }
            mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
            _mediaPlayer.Source = mediaSource;

            _mediaPlayerElement.SetMediaPlayer(_mediaPlayer);
            //_mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri(""));

            _mediaTimelineController = new MediaTimelineController();
            _mediaPlayer.CommandManager.IsEnabled = false;
            _mediaPlayer.TimelineController = _mediaTimelineController;
            _mediaTimelineController.PositionChanged += _mediaTimelineController_PositionChanged;
        }
    }
}
