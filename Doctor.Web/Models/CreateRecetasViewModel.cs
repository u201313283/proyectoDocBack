using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor.Web.Models
{
    public class CreateRecetasViewModel
    {
        public int MedicamentoId { get; set; }
        public int Frecuencia { get; set; }
        public int Duracion { get; set; }
        public int Cantidad { get; set; }
        public int CitaId { get; set; }
    }
}
