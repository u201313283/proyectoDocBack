using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Doctor.Entities
{
    public class Enfermedad
    {
        public int EnfermedadId { get; set; }

        public string Nombre { get; set; }


        public virtual ICollection<Diagnostico> Diagnostico { get; set; }
        public virtual ICollection<Medicamento> Medicamento { get; set; }
    }
}
