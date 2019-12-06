using FTPServer.BusinessModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;

namespace FTPServer.DAL
{
    public class DirectoryDbContext : DbContext
    {
        private string connectionString;

        public DbSet<Directory> Directories { get; set; }

        public DirectoryDbContext(IConfiguration config) : this("FTPServer", config) { }

        public DirectoryDbContext(string connectionStringName, IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            if (connectionStringName == null) throw new ArgumentNullException(nameof(connectionStringName));
            if (connectionStringName.Trim() == string.Empty) throw new ArgumentException($"{nameof(connectionStringName)} can not be empty");

            connectionString = config.GetConnectionString(connectionStringName);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FileConfiguration());
            builder.ApplyConfiguration(new DirectoryConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(connectionString);
        }
    }

    
}
