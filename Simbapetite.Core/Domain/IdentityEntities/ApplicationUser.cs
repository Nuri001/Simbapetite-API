using Microsoft.AspNetCore.Identity;

namespace Simbapetite.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
