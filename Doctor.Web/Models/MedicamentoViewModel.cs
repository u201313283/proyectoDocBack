using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor.Web.Models
{
    public class MedicamentoViewModel
    {
        public int MedicamentoId { get; set; }

        public string Nombre { get; set; }

        public int EnfermedadId { get; set; }
    }
}
