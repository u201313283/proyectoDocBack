using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Doctor.Entities
{
    public class Medico
    {
        public int MedicoId { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 50 caracteres")]
        public string Nombre { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 50 caracteres")]
        public string Apellido { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 50 caracteres")]
        public string Clinica { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 50 caracteres")]
        public string Usuario { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 50 caracteres")]
        public string Password { get; set; }

        public virtual ICollection<Cita> Cita { get; set; }
        public virtual ICollection<Paciente> Paciente { get; set; }

    }
}
