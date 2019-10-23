using Microsoft.EntityFrameworkCore;
using Robinator.Core.EF;

namespace Robinator.Example.Components
{
    public interface IRatingDbContext : IDbContext
    {
        public DbSet<Rating> Ratings { get; set; }
    }
}