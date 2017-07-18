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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Animal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private delegate string AnimalSaying(object sender, myEventArgs e);//声明一个委托
        private event AnimalSaying Say;//委托声明一个事件
        private int times = 0;

        public MainPage()
        {
            this.InitializeComponent();
        }

        interface Animal
        {
            //方法
            string saying(object sender, myEventArgs e);
            //属性
            int A { get; set; }
        }

        class cat : Animal
        {
            TextBlock word;
            TextBox input;
            private int a;

            public cat(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "Cat: " + "I am a cat." + "\n";
                return "";
            }
            public int A
            {
                get { return a; }
                set { this.a = value; }
            }
        }

        class dog : Animal
        {
            TextBlock word;
            TextBox input;
            private int a;

            public dog(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "Dog: " + "I am a dog.\n";
                return "";
            }
            public int A
            {
                get { return a; }
                set { this.a = value; }
            }
        }

        class pig : Animal
        {
            TextBlock word;
            TextBox input;
            private int a;

            public pig(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "Pig: " + "I am a pig.\n";
                return "";
            }
            public int A
            {
                get { return a; }
                set { this.a = value; }
            }
        }
        private cat c;
        private dog d;
        private pig p;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            words.Text = "";
            c = new cat(words);
            d = new dog(words);
            p = new pig(words);
            Random ra = new Random();
            int n = ra.Next(1, 4);
            if (n == 1) Say += new AnimalSaying(c.saying);
            if (n == 2) Say += new AnimalSaying(d.saying);
            if (n == 3) Say += new AnimalSaying(p.saying);
            Say(this, new myEventArgs(times++));
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            words.Text = "";
            c = new cat(words);
            d = new dog(words);
            p = new pig(words);
            if (input.Text == "cat") Say += new AnimalSaying(c.saying);
            if (input.Text == "dog") Say += new AnimalSaying(d.saying);
            if (input.Text == "pig") Say += new AnimalSaying(p.saying);
            //执行事件
            Say(this, new myEventArgs(times++));//事件中传递参数times
            input.Text = string.Empty;
        }

        //自定义一个Eventargs传递事件参数
        
        class myEventArgs : EventArgs
        {
            public int t = 0;
            public myEventArgs(int tt)
            {
                this.t = tt;
            }
        }
    }
}
