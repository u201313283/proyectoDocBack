using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Entities
{
    public class Receta
    {
        public int RecetaId { get; set; }
        public int MedicamentoId { get; set; }
        public int Frecuencia { get; set; }
        public int Duracion { get; set; }
        public int Cantidad { get; set; }
        public int CitaId { get; set; }

        public virtual Cita Cita { get; set; }
        public virtual Medicamento Medicamento { get; set; }



    }
}
