using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Doctor.Entities
{
    public class Examen
    {
        public int ExamenId { get; set; }

        public string Nombre { get; set; }

        public virtual ICollection<CitaExamen> CitaExamen { get; set; }
    }
}
