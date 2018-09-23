using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diassoft.Mvvm.Wpf.AppTest.Models
{
    public class Book: Diassoft.Mvvm.ObservableObjectBase
    {

        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { SetProperty<string>(ref _Title, value); }
        }


        private string _Author;

        public string Author
        {
            get { return _Author; }
            set { SetProperty<string>(ref _Author, value); }
        }


        private double _Price;

        public double Price
        {
            get { return _Price; }
            set { SetProperty<double>(ref _Price, value); }
        }

        public Book(): this(String.Empty, String.Empty, 0) { }

        public Book(string title, string author, double price)
        {
            Title = title;
            Author = author;
            Price = price;
        }

    }
}
