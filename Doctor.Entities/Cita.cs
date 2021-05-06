using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Doctor.Entities
{
    public class Cita
    {
        public int CitaId { get; set; }


        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 50 caracteres")]
        public string Motivo { get; set; }


        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Descripcion { get; set; }


        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Sintomas { get; set; }


        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Exploracion { get; set; }


        
        public string FInicio { get; set; }
        public string FFin { get; set; }
        public string Hora { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Indicacion { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public bool? esEliminado { get; set; }

        public virtual Paciente Paciente { get; set; }

        public virtual Medico Medico { get; set; }
        



        public virtual ICollection<CitaExamen> CitaExamen { get; set; }
        public virtual ICollection<Diagnostico> Diagnostico { get; set; }
        public virtual ICollection<Receta> Receta { get; set; }
    }
}
