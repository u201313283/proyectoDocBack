using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor.Web.Models
{
    public class UpdateCitaViewModel
    {
        public int CitaId { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 50 caracteres")]
        public string Motivo { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Descripcion { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Sintomas { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamaño entre 3 a 100 caracteres")]
        public string Exploracion { get; set; }

       
        public string FInicio { get; set; }
        public string FFin { get; set; }
        public string Hora { get; set; }
        public string Indicacion { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
    }
}
