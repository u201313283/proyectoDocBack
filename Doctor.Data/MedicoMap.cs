using Doctor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Data
{
    class MedicoMap : IEntityTypeConfiguration<Medico>
    {
        public void Configure(EntityTypeBuilder<Medico> builder)
        {
            builder.ToTable("Medico")
            .HasKey(c => c.MedicoId);
            builder.Property(c => c.Nombre)
                .HasColumnName("Nombre")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.Apellido)
                .HasColumnName("Apellido")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.Clinica)
                .HasColumnName("Clinica")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.Usuario)
                .HasColumnName("Usuario")
                .HasMaxLength(50); //.IsRequired()
            builder.Property(c => c.Password)
                .HasColumnName("Password")
                .HasMaxLength(50); //.IsRequired()
        }
    }
}
