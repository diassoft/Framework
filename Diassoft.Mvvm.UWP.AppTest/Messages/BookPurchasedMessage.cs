using Diassoft.Mvvm.UWP.AppTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diassoft.Mvvm.UWP.AppTest.Messages
{
    public class BookPurchasedMessage: Diassoft.Mvvm.Messenger.MessageBase
    {

        public Book Book { get; set; }

        public BookPurchasedMessage(Book book): this(null, book) { }

        public BookPurchasedMessage(object sender, Book book): base(sender)
        {
            Book = book;
        }

    }
}
