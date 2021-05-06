using Doctor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Data
{
    public class DbDocContext : DbContext
    {
        public DbSet<Cita> Citas { get; set; }
        public DbSet<CitaExamen> CitasExamenes { get; set; }
        public DbSet<Diagnostico> Diagnosticos { get; set; }
        public DbSet<Enfermedad> Enfermedades { get; set; }
        public DbSet<Examen> Examenes { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Receta> Recetas { get; set; }

        public DbSet<TipoDocumento> TipoDocumentos { get; set; }

        public DbDocContext(DbContextOptions<DbDocContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CitaMap());
            modelBuilder.ApplyConfiguration(new CitaExamenMap());
            modelBuilder.ApplyConfiguration(new DiagnosticoMap());
            modelBuilder.ApplyConfiguration(new EnfermedadMap());
            modelBuilder.ApplyConfiguration(new ExamenMap());
            modelBuilder.ApplyConfiguration(new MedicamentoMap());
            modelBuilder.ApplyConfiguration(new MedicoMap());
            modelBuilder.ApplyConfiguration(new PacienteMap());
            modelBuilder.ApplyConfiguration(new RecetaMap());
            modelBuilder.ApplyConfiguration(new TipoDocumentoMap());
        }
    }
}
