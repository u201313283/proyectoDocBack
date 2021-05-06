using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Doctor.Entities
{
    public class TipoDocumento
    {
        public int TipoDocumentoId { get; set; }

        public string Nombre { get; set; }

        public virtual ICollection<Paciente> Paciente { get; set; }
    }
}
