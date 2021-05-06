using Doctor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Data
{
    class DiagnosticoMap : IEntityTypeConfiguration<Diagnostico>
    {
        public void Configure(EntityTypeBuilder<Diagnostico> builder)
        {
            builder.ToTable("Diagnostico")
            .HasKey(c => c.DiagnosticoId);
            builder.Property(c => c.EnfermedadId)
                .HasColumnName("EnfermedadId");
            builder.Property(c => c.CitaId)
                .HasColumnName("CitaId");
            builder.Property(c => c.Observacion)
                .HasColumnName("Observacion")
                .HasMaxLength(100); //.IsRequired()
        }
    }
}
