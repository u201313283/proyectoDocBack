using Doctor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Data
{
    class TipoDocumentoMap : IEntityTypeConfiguration<TipoDocumento>
    {
        public void Configure(EntityTypeBuilder<TipoDocumento> builder)
        {
            builder.ToTable("TipoDocumento")
                .HasKey(c => c.TipoDocumentoId);
            builder.Property(c => c.Nombre)
                .HasColumnName("Nombre")
                .HasMaxLength(20); //.IsRequired()
        }
    }
}
