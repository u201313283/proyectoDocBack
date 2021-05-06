using Doctor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Data
{
    class CitaExamenMap : IEntityTypeConfiguration<CitaExamen>
    {
        public void Configure(EntityTypeBuilder<CitaExamen> builder)
        {
            builder.ToTable("CitaExamen")
            .HasKey(c => c.CitaExamenId);
            builder.Property(c => c.CitaId)
                .HasColumnName("CitaId");
            builder.Property(c => c.ExamenId)
                .HasColumnName("ExamenId");
            builder.Property(c => c.Observacion)
                .HasColumnName("Observacion")
                .HasMaxLength(100); //.IsRequired()
        }
    }
}
