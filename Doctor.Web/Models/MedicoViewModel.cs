using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor.Web.Models
{
    public class MedicoViewModel
    {
        public int MedicoId { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Clinica { get; set; }
        public string Usuario { get; set; }

        public string Password { get; set; }

    }
}
