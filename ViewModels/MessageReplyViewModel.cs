using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;
using DS3_Sprint1.Models;

namespace DS3_Sprint1.ViewModels
{
    public class MessageReplyViewModel
    {
        private List<MessageReply> _replies = new List<MessageReply>();
        public Reply Reply { get; set; }

        public Message Message { get; set; }

        public List<MessageReply> Replies
        {
            get { return _replies; }
            set { _replies = value; }
        }

        public PagedList.IPagedList<Message> Messages { get; set; }

        public class MessageReply
        {
            public int Id { get; set; }
            public int MessageId { get; set; }
            public string MessageDetails { get; set; }
            public string ReplyFrom { get; set; }

            public string ReplyMessage { get; set; }
            public DateTime ReplyDateTime { get; set; }
        }


    }
}