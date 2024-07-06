using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace Talabat.Core.Entities.Core
    {
        public class ReportedPost : BaseEntity
        {
            public ReportedPost()
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


