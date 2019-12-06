using DataServer.BusinessModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataServer.DAL
{
    public class FileContentConfiguration : IEntityTypeConfiguration<FileContent>
    {
        public void Configure(EntityTypeBuilder<FileContent> builder)
        {
            builder.ToTable("FilesB");

            builder.Property(x => x.Key)
                .IsRequired();
            builder.HasKey(x => x.Key);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.UpdateDate)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired();
        }
    }
}
