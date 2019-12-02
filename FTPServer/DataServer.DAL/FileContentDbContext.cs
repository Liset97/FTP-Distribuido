using Microsoft.EntityFrameworkCore;
using DataServer.BusinessModel;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataServer.DAL
{
    public class FileContentDbContext : DbContext
    {
        private string connectionString;

        public DbSet<FileContent> Files { get; set; }

        public FileContentDbContext(IConfiguration config) : this("DataServer", config) { }

        public FileContentDbContext(string connectionStringName, IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            if (connectionStringName == null) throw new ArgumentNullException(nameof(connectionStringName));
            if (connectionStringName.Trim() == string.Empty) throw new ArgumentException($"{nameof(connectionStringName)} can not be empty");

            connectionString = config.GetConnectionString(connectionStringName);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FileContentConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(connectionString);
        }
    }

    
}
