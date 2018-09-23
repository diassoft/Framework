using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diassoft.Mvvm.Wpf.AppTest.Messages
{
    public class BooksLoadedMessage : Diassoft.Mvvm.Messenger.MessageBase
    {
        public BooksLoadedMessage() : this(null) { }

        public BooksLoadedMessage(object sender) : base(sender)
        {
            
        }
    }
}
