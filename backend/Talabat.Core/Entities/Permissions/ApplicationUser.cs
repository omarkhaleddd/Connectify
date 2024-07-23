using Microsoft.AspNetCore.Identity;
namespace Talabat.Core.Entities.Permissions
{
    [Table("Perm_ApplicationUser")]
    public class ApplicationUser : IdentityUser
    { 

        [MaxLength(100)]
        public string FullName { get; set; } 

        public bool IsDeleted { get; set; } = false;
        public DateTime InsertDate { get; set; } = new DateTime();
        public DateTime UpdateDate { get; set; } = new DateTime();
        public DateTime? DeleteDate { get; set; }
        [StringLength(100)]
        public string? InsertBy { get; set; }
        [StringLength(100)]
        public string? UpdateBy { get; set; }
        [StringLength(100)]
        public string? DeleteBy { get; set; }
    }
}
