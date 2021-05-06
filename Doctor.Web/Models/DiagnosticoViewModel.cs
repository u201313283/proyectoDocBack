using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor.Web.Models
{
    public class DiagnosticoViewModel
    {
        public int DiagnosticoId { get; set; }
        public int EnfermedadId { get; set; }
        public int CitaId { get; set; }
        public string Observacion { get; set; }
    }
}
