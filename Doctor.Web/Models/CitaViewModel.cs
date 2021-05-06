using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor.Web.Models
{
    public class CitaViewModel
    {
        public int CitaId { get; set; }

        public string Motivo { get; set; }

        public string Descripcion { get; set; }

        public string Sintomas { get; set; }

        public string Exploracion { get; set; }

        public string FInicio { get; set; }
        public string FFin { get; set; }
        public string Hora { get; set; }
        public string Indicacion { get; set; }

        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
    }
}
