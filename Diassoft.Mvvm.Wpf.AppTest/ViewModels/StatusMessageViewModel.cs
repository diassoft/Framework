using Diassoft.Mvvm.Messenger;
using Diassoft.Mvvm.Wpf.AppTest.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diassoft.Mvvm.Wpf.AppTest.ViewModels
{
    public class StatusMessageViewModel: Diassoft.Mvvm.ViewModelBase
    {
        public StatusMessageViewModel(): this(null) { }

        public StatusMessageViewModel(MessageDispatcher dispatcher): base(dispatcher)
        {

        }

        private string _Message;

        public string Message
        {
            get { return _Message; }
            set { SetProperty<string>(ref _Message, value, nameof(Message)); }
        }

        private MessageTypes _MessageType;

        public MessageTypes MessageType
        {
            get { return _MessageType; }
            set { SetProperty<MessageTypes>(ref _MessageType, value, nameof(MessageType)); }
        }

        public override void WireCommands()
        {
            
        }

        public override void SubscribeToMessages()
        {
            base.Dispatcher?.Subscribe<BookPurchasedMessage>(this, BookPurchasedMessage_Callback);
            base.Dispatcher?.Subscribe<BooksLoadedMessage>(this, BooksLoadedMessage_Callback);
        }


        private void BookPurchasedMessage_Callback(object sender, MessageBase message)
        {
            var bookMessage = (BookPurchasedMessage)message;

            Message = $"Congratulations, you have just purchased {bookMessage.Book.Title}, from {bookMessage.Book.Author}!!";
            MessageType = MessageTypes.Success;
        }

        private void BooksLoadedMessage_Callback(object sender, MessageBase message)
        {
            Message = "Books loaded successfully!!!";
            MessageType = MessageTypes.Normal;
        }
    }

    public enum MessageTypes: int
    {
        Normal = 0,
        Success = 1,
        Warning = 2,
        Error = 3
    }
}
