using Connectify.Core.Entities.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Identity
{
    public class AppUser:IdentityUser
    {
        public string DisplayName { get; set; }

        public ICollection<Post> posts { get; set; } = new HashSet<Post> ();
        public Address Address { get; set; }
    }
}
