using Microsoft.AspNetCore.Identity;

namespace Fin.Core.Entities
{
    public class FinsightUser: IdentityUser
    {
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
