﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Core
    {
        public class ReportedPost : BaseEntity
        {
            ReportedPost()
            {
                Status = "pending";
                IsDeleted = false;
                InsertDate = DateTime.Now;
                UpdateDate = DateTime.Now;
                DeleteDate = null;

            }
            [ForeignKey("Post")]
            public int PostId { get; set; }
            public Post Post { get; set; }
            public string Status { get; set; }
        }
    }

