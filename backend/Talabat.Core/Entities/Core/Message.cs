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
        public string userId { get; set; }
        public string displayName { get; set; }
        public DateTime messageDate { get; set; } = DateTime.Now;

    }
}
