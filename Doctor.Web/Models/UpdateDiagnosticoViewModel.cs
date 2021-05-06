using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor.Web.Models
{
    public class UpdateDiagnosticoViewModel
    {
        public int DiagnosticoId { get; set; }
        public int EnfermedadId { get; set; }
        public int CitaId { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Observacion { get; set; }
    }
}
