using Doctor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Data
{
    class EnfermedadMap : IEntityTypeConfiguration<Enfermedad>
    {
        public void Configure(EntityTypeBuilder<Enfermedad> builder)
        {
            builder.ToTable("Enfermedad")
                .HasKey(c => c.EnfermedadId);
            builder.Property(c => c.Nombre)
                .HasColumnName("Nombre")
                .HasMaxLength(50); //.IsRequired()
        }
    }
}
