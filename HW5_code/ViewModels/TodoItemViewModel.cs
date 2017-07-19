using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.allItems.Add(new Models.TodoItem("123", "123",DateTime.Now.Date));
            this.allItems.Add(new Models.TodoItem("456", "456",DateTime.Now.Date));
        }

        public void AddTodoItem(string title, string description, System.DateTimeOffset date)
        {
            this.allItems.Add(new Models.TodoItem(title, description,date));
        }

        public void RemoveTodoItem(string id)
        {
            // DIY
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
        }

    }
}
