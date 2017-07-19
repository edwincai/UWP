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
using System.Net.Http;
using Windows.Data.Xml.Dom;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Week7Demo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            temperature.Text = "";
            runner.Text = "";
            queryAsync(number.Text);
        }

        async void queryAsync(string city)
        {
            string url = "http://api.k780.com:88/?app=weather.future&weaid=" + city
                + "&appkey=24605&sign=e7da39fad6bc0c6cbfa29fc2f46e919d&format=xml";
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(url);
            XmlDocument document = new XmlDocument();
            document.LoadXml(result);
            XmlNodeList list = document.GetElementsByTagName("temperature");
            IXmlNode node = list.Item(0);
            temperature.Text = node.InnerText;
            list = document.GetElementsByTagName("weather");
            node = list.Item(0);
            runner.Text = node.InnerText;
        }
    }
}
