using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Doctor.Entities
{
    public class Diagnostico
    {
        public int DiagnosticoId { get; set; }
        public int EnfermedadId { get; set; }
        public int CitaId { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Observacion { get; set; }


        public virtual Enfermedad Enfermedad { get; set; }

        public virtual Cita Cita { get; set; }
    }
}
