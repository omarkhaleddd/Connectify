﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Core
{
	public class Notification : BaseEntity
	{
        public Notification()
        {
            IsDeleted = false;
            InsertDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            DeleteDate = null;
        }
        public string content { get; set; }
		public string userId { get; set; }
		public string type { get; set; }
		public DateTime notificationDate { get; set; } = DateTime.Now;

	}
}
