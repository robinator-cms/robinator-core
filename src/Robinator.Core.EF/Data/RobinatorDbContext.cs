using Microsoft.EntityFrameworkCore;
using Robinator.Core.Areas.Pages;

namespace Robinator.Core.EF.Data
{
    public class RobinatorDbContext : DbContext, IDbContext
    {
        public RobinatorDbContext(DbContextOptions<RobinatorDbContext> options) : base(options)
        {
        }
        public DbSet<Page> Pages { get; set; }
    }
}
