using Microsoft.EntityFrameworkCore;

namespace API_Template.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        // DbSets
    }
}
