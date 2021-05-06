using Doctor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Data
{
    class RecetaMap : IEntityTypeConfiguration<Receta>
    {
        public void Configure(EntityTypeBuilder<Receta> builder)
        {
            builder.ToTable("Receta")
                .HasKey(c => c.RecetaId);
            builder.Property(c => c.MedicamentoId)
                .HasColumnName("MedicamentoId");
            builder.Property(c => c.Frecuencia)
                .HasColumnName("Frecuencia");
            builder.Property(c => c.Duracion)
                .HasColumnName("Duracion");
            builder.Property(c => c.Cantidad)
                .HasColumnName("Cantidad");
            builder.Property(c => c.CitaId)
                .HasColumnName("CitaId");
        }
    }
}
