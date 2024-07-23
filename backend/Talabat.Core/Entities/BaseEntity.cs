using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Talabat.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime InsertDate { get; set; } = new DateTime();
        public DateTime UpdateDate { get; set; } = new DateTime();
        public DateTime? DeleteDate { get; set; }
        [StringLength(100)]
        public string? InsertBy { get; set; }
        [StringLength(100)]
        public string? UpdateBy { get; set; }
        public string? DeleteBy { get; set; }
    }

}