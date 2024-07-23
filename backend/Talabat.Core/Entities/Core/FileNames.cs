using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Core
{
    public class FileNames : BaseEntity
    {
        public FileNames()
        {
            IsDeleted = false;
            InsertDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            DeleteDate = null;
        }
        public string FileName { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
