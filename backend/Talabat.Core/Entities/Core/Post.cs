﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Entities.Core
{
    public class Post : BaseEntity
    {

        public Post() 
        {
			DatePosted = DateTime.Now;
			IsDeleted = false;
			InsertDate = DateTime.Now;
			UpdateDate = DateTime.Now;
			DeleteDate = null;
		}

        public string content { get; set; }
        public int likeCount { get; set; }
        public DateTime DatePosted { get; set; } 
        public string AuthorId { get; set; }

        public ICollection<Comment>? Comments { get; set; } = new HashSet<Comment>();
    }
}
