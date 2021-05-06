using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Doctor.Entities
{
    public class CitaExamen
    {
        public int CitaExamenId { get; set; }
        public int CitaId { get; set; }
        public int ExamenId { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Observacion { get; set; }



        public virtual Cita Cita { get; set; }

        public virtual Examen Examen { get; set; }
    }
}
