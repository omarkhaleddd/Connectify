using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Entities.Core
{
	public class FriendRequest : BaseEntity
	{
        public FriendRequest()
        {
            IsDeleted = false;
            InsertDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            DeleteDate = null;
        }
        public string SenderId { get; set; }
		public string Recieverid { get; set; }
	}
}
