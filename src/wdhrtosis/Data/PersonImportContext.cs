using System;
using Microsoft.EntityFrameworkCore;
using wdhrtosis.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace wdhrtosis.Data
{
    public partial class PersonImportContext : DbContext
    {

        private readonly IConfiguration _config;
        private readonly ILogger<PersonImportContext> _logger;



        public PersonImportContext()
        { }


        public PersonImportContext(DbContextOptions<PersonImportContext> options, IConfiguration config, ILogger<PersonImportContext> logger)
                : base(options)
        {
            _config = config;
            _logger = logger;

            // read value(s) from appsettings using _config 
            int Timeout = _config.GetValue<int>("SSISImportContextOptions:CommandTimeout");
            if (Timeout > 0)
            {
                _logger.LogInformation($"Setting SSISImportContext Database.SetCommandTimeout: {Timeout}");
                Database.SetCommandTimeout(Timeout);
            }
            else
            {
                _logger.LogInformation($"Using Default SSISImportContext Database CommandTimeout");
            }

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<History>().HasData(new History()
            {
                Id = 1,
                CorrelationId = "First 50 days",
                ErrorMessage = null,
                LastRun = DateTimeOffset.UtcNow.AddDays(-50),
                Message = "Initial Seed",
                Success = true
            });
        }

        public DbSet<Visa> PersonVisa { get; set; }
        public DbSet<Contact> PersonContact { get; set; }
        public DbSet<History> PersonHistory { get; set; }
        public DbSet<Email> PersonEmailAddress { get; set; }
        public DbSet<Address> PersonAddress { get; set; }
        public DbSet<Name> PersonName { get; set; }
        public DbSet<EmploymentProfile> PersonEmploymentProfile { get; set; }
        public DbSet<EmploymentPosition> PersonEmploymentPosition { get; set; }
        public DbSet<ProcessSummary> PersonImportProcessSummary { get; set; }


    }
}
