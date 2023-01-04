using Microsoft.EntityFrameworkCore;
using VocaSRS.Context.Entities;

namespace VocaSRS.Context
{
    public class VocaContext : DbContext
    {
        public VocaContext(DbContextOptions<VocaContext> options) : base(options)
        {
        }

        public DbSet<Vocabulary> Vocabularies { get; set; }
    }
}
