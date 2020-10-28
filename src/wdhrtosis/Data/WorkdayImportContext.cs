using EA.Mulesoft.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace wdhrtosis.Data
{
    public class WorkdayImportContext : DbContext
    {
        public WorkdayImportContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Worker> Workers { get; set; }
    }
}
