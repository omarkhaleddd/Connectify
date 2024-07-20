using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
            Likes = null;
            ReportCount = 0;
            Privacy = 1; // 0 : only me , 1 : friends only , 2 : public
		}
        public string content { get; set; }
        public DateTime DatePosted { get; set; } 
        public string AuthorId { get; set; }
		public string AuthorName { get; set; }
		public ICollection<PostLikes>? Likes { get; set; } = new HashSet<PostLikes>();
        public ICollection<Comment>? Comments { get; set; } = new HashSet<Comment>();
		public ICollection<Repost>? Reposts { get; set; } = new HashSet<Repost>();
        [JsonIgnore]
        public ICollection<FileNames>? FileName { get; set; } = new HashSet<FileNames>(); // navigational property
        public int ReportCount { get; set; }
        public int Privacy { get; set; }
    }
}
