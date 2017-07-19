using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
namespace Todos.Models
{
    class TodoItem
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool completed { get; set; }
        public System.DateTimeOffset day;

        //日期字段自己写

        public TodoItem(string title, string description, System.DateTimeOffset date)
        {
            this.id = Guid.NewGuid().ToString(); //生成id
            this.title = title;
            this.description = description;
            this.completed = false; //默认为未完成
            this.day = date;
        }

        public TodoItem()
        {
            id = "1";
            title = "";
            description = "";
            day = DateTime.Now.Date;
            completed = false;

        }
    }
    class Converter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool myValue = (bool)value;
            if (myValue)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
