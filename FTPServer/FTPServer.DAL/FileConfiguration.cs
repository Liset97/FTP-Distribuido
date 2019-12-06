using FTPServer.BusinessModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FTPServer.DAL
{
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.ToTable("Files");

            builder.Property(x => x.Key)
                .IsRequired();
            builder.HasKey(x => x.Key);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.PathName)
                .IsRequired();

            builder.Property(x => x.UpdateDate)
                .IsRequired();

            builder.Property(x => x.FileType)
                .IsRequired();

            builder.Property(x => x.Length)
                .IsRequired();

            builder.Property(x => x.ContentKey)
                .IsRequired();

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Files);

            builder.Property(x => x.IsDeleted);
        }
    }
}
