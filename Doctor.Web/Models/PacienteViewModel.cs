using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor.Web.Models
{
    public class PacienteViewModel
    {
        public int PacienteId { get; set; }

        public string Nombre { get; set; }

        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }


        public int TipoDocumentoId { get; set; }


        public string NumeroDocumento { get; set; }

        public string Direccion { get; set; }

        public string Sexo { get; set; }

       
        public string FechaNacimiento { get; set; }


        public string Telefono { get; set; }

        public string Celular { get; set; }

        public string Correo { get; set; }


    }
}
