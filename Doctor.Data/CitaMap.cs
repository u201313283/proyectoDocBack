using Doctor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Data
{
    class CitaMap : IEntityTypeConfiguration<Cita>
    {
        public void Configure(EntityTypeBuilder<Cita> builder)
        {
            builder.ToTable("Cita")
            .HasKey(c => c.CitaId);
            builder.Property(c => c.Motivo)
                .HasColumnName("Motivo")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.Descripcion)
                .HasColumnName("Descripcion")
                .HasMaxLength(100); //.IsRequired()
            builder.Property(c => c.Sintomas)
                .HasColumnName("Sintomas")
                .HasMaxLength(100); //.IsRequired()
            builder.Property(c => c.Exploracion)
                .HasColumnName("Exploracion")
                .HasMaxLength(100); //.IsRequired()
            builder.Property(c => c.Indicacion)
                .HasColumnName("Indicacion")
                .HasMaxLength(100); //.IsRequired()
            builder.Property(c => c.FInicio)
                .HasColumnName("FInicio")
                .HasMaxLength(10);
            builder.Property(c => c.FFin)
                .HasColumnName("FFin")
                .HasMaxLength(10);
            builder.Property(c => c.Hora)
                .HasColumnName("Hora");
            builder.Property(c => c.PacienteId)
                .HasColumnName("PacienteId");
            builder.Property(c => c.MedicoId)
                .HasColumnName("MedicoId");
            builder.Property(c => c.esEliminado)
                .HasColumnName("esEliminado")
                .HasDefaultValueSql("((0))");

        }
    }
}
