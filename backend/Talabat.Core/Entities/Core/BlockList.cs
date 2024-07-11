using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Core
{
    public class BlockList : BaseEntity
    {
        public BlockList()
        {
            IsDeleted = false;
            InsertDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            DeleteDate = null;
        }
        public string UserId { get; set; }
        public string BlockedId { get; set; }
    }
}
