using FTPServer.BusinessModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FTPServer.DAL
{
    public class DirectoryConfiguration : IEntityTypeConfiguration<Directory>
    {
        public void Configure(EntityTypeBuilder<Directory> builder)
        {
            builder.ToTable("Directories");

            builder.Property(x => x.Key)
                .IsRequired();
            builder.HasKey(x => x.Key);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.UpdateDate)
                .IsRequired();

            builder.HasMany(x => x.Directories)
                .WithOne(x => x.Parent);

            builder.HasMany(x => x.Files)
                .WithOne(x => x.Parent);

        }
    }
}
