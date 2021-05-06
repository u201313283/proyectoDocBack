using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor.Web.Models
{
    public class CitaExamenViewModel
    {
        public int CitaExamenId { get; set; }
        public int CitaId { get; set; }
        public int ExamenId { get; set; }
        public string Observacion { get; set; }
    }
}
