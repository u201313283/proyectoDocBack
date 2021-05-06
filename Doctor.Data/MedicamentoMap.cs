using Doctor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Data
{
    class MedicamentoMap : IEntityTypeConfiguration<Medicamento>
    {
        public void Configure(EntityTypeBuilder<Medicamento> builder)
        {
            builder.ToTable("Medicamento")
                .HasKey(c => c.MedicamentoId);
            builder.Property(c => c.Nombre)
                .HasColumnName("Nombre")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.EnfermedadId)
                .HasColumnName("EnfermedadId");
        }
    }
}
