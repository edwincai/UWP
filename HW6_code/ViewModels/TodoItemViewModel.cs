using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace Todos.ViewModels
{
    class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; }  }

        public TodoItemViewModel()
        {
            // 加入两个用来测试的item
            //this.allItems.Add(new Models.TodoItem("123", "123",DateTime.Now.Date));
            //this.allItems.Add(new Models.TodoItem("456", "456",DateTime.Now.Date));
            using (var db = new SQLiteConnection("demo.db"))
            {
                using (var statement = db.Prepare("SELECT Title,Description,Date From TodoItem"))
                {
                    while(SQLiteResult.ROW == statement.Step())
                    {
                        this.allItems.Add(new Models.TodoItem((string)statement[0], (string)statement[1], DateTimeOffset.Parse(statement[2].ToString())));
                    }
                }
            }
        }

        public void AddTodoItem(string title, string description, System.DateTimeOffset date)
        {
            this.allItems.Add(new Models.TodoItem(title, description,date));

            var db = App.conn;
            using(var todoitem = db.Prepare("INSERT INTO TodoItem(Title,Description,Date) VALUES(?,?,?)"))
            {
                todoitem.Bind(1, title);
                todoitem.Bind(2, description);
                todoitem.Bind(3, date.ToString());
                todoitem.Step();
            }
        }

        public void RemoveTodoItem(string id)
        {
            // DIY
            var db = App.conn;
            using (var statement = db.Prepare("DELETE FROM TodoItem WHERE Title = ?"))
            {
                statement.Bind(1, id);
                statement.Step();
            }
            this.allItems.Remove(this.SelectedItem);
            // set selectedItem to null after remove
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string title, string description, System.DateTimeOffset date)
        {
            // DIY
            var index = this.allItems.IndexOf(this.selectedItem);
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.day = date;
            this.allItems.Remove(this.SelectedItem);
            this.allItems.Insert(index, this.selectedItem);
            this.selectedItem = null;
            this.selectedItem = null;
            var db = App.conn;
            using (var statement = db.Prepare("UPDATE TodoItem SET title = ?,description = ?, date = ? "))
            {
                statement.Bind(1, title);
                statement.Bind(2, description);
                statement.Bind(3, date.ToString());
                statement.Step();
            }
        }

    }
}
