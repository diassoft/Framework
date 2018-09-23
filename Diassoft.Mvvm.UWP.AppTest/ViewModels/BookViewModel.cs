using Diassoft.Mvvm.UWP.AppTest.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Diassoft.Mvvm.Command;
using Diassoft.Mvvm.UWP.AppTest.Messages;

namespace Diassoft.Mvvm.UWP.AppTest.ViewModels
{
    public class BookViewModel: Diassoft.Mvvm.ViewModelBase
    {
        public BookViewModel(): this(null) { }

        public BookViewModel(Messenger.MessageDispatcher dispatcher): base(dispatcher)
        {
            Books = new ObservableCollection<Book>();
            SelectedBook = null;
        }

        #region ViewModelBase Objects

        public override void WireCommands()
        {
            LoadBooksCommand = new RelayCommand(param => LoadBooksCommand_Execute(param), param => LoadBooksCommand_CanExecute(param));
            BuyBookCommand = new RelayCommand(param => BuyBookCommand_Execute(param), param => BuyBookCommand_CanExecute(param));
        }

        public override void SubscribeToMessages()
        {
            
        }

        #endregion ViewModelBase Objects

        #region Properties

        private ObservableCollection<Book> _Books;
        public ObservableCollection<Book> Books
        {
            get { return _Books; }
            set { SetProperty<ObservableCollection<Book>>(ref _Books, value, nameof(Books)); }
        }


        private Book _SelectedBook;

        public Book SelectedBook
        {
            get { return _SelectedBook; }
            set { SetProperty<Book>(ref _SelectedBook, value); }
        }


        private string _StatusMessage;

        public string StatusMessage
        {
            get { return _StatusMessage; }
            set { SetProperty<string>(ref _StatusMessage, value); }
        }


        #endregion Properies


        #region Command LoadBooks

        private ICommand _LoadBooksCommand;

        public ICommand LoadBooksCommand
        {
            get { return _LoadBooksCommand; }
            set { SetProperty<ICommand>(ref _LoadBooksCommand, value); }
        }


        private bool LoadBooksCommand_CanExecute(object param)
        {
            return true;
        }

        private void LoadBooksCommand_Execute(object param)
        {
            // Clear the existing list of books
            Books.Clear();

            // Create a fake list of books
            for (int iAuthor = 1; iAuthor < 5; iAuthor++)
            {
                for (int iBook = 1; iBook < 10; iBook++)
                {
                    Books.Add(new Book($"Book {iBook}", $"Author {iAuthor}", iBook * iAuthor));
                }
            }

            base.Dispatcher?.Send<BooksLoadedMessage>(this, new BooksLoadedMessage());
        }

        #endregion Command LoadBooks

        #region Command BuyBook

        private ICommand _BuyBookCommand;

        public ICommand BuyBookCommand
        {
            get { return _BuyBookCommand; }
            set { SetProperty<ICommand>(ref _BuyBookCommand, value); }
        }

        private bool BuyBookCommand_CanExecute(object param)
        {
            return (SelectedBook != null);
        }

        private void BuyBookCommand_Execute(object param)
        {
            // Display a message reporting the book was purchased
            base.Dispatcher?.Send<BookPurchasedMessage>(this, new BookPurchasedMessage((Book)param));
        }

        #endregion Command BuyBook

    }
}
