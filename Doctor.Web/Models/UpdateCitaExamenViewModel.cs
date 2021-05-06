using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor.Web.Models
{
    public class UpdateCitaExamenViewModel
    {
        public int CitaExamenId { get; set; }
        public int CitaId { get; set; }
        public int ExamenId { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Observacion { get; set; }

    }
}
