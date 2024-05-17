using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Core
{
    public class Message : BaseEntity
    {
        public string messageText { get; set; }
        public string senderId { get; set; }
        public string senderName { get; set; }
        public string recieverId { get; set; }
        public string recieverName { get; set; }
        public DateTime messageDate { get; set; } = DateTime.Now;

    }
}
