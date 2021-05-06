using Doctor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Data
{
    class PacienteMap : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            builder.ToTable("Paciente")
            .HasKey(c => c.PacienteId);
            builder.Property(c => c.Nombre)
                .HasColumnName("Nombre")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.ApellidoPaterno)
                .HasColumnName("ApellidoPaterno")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.ApellidoMaterno)
                .HasColumnName("ApellidoMaterno")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.TipoDocumentoId)
                .HasColumnName("TipoDocumentoId");
            builder.Property(c => c.NumeroDocumento)
                .HasColumnName("NumeroDocumento")
                .HasMaxLength(19); //.IsRequired()
            builder.Property(c => c.Direccion)
                .HasColumnName("Direccion")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.Sexo)
                .HasColumnName("Sexo")
                .HasMaxLength(10); //.IsRequired()
            builder.Property(c => c.FechaNacimiento)
                .HasColumnName("FechaNacimiento")
                .HasMaxLength(10);
            builder.Property(c => c.Telefono)
                .HasColumnName("Telefono")
                .HasMaxLength(15); //.IsRequired()
            builder.Property(c => c.Celular)
                .HasColumnName("Celular")
                .HasMaxLength(15); //.IsRequired()
            builder.Property(c => c.Correo)
                .HasColumnName("Correo")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.MedicoId)
                .HasColumnName("MedicoId");
        }
    }
}
