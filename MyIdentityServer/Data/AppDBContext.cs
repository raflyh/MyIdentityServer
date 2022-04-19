using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyIdentityServer.Data
{
    public class AppDBContext : IdentityDbContext
    {
        public AppDBContext(DbContextOptions options):base(options)
        {

        }
    }
}
