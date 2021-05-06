using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctor.Data;
using Doctor.Entities;
using Doctor.Web.Models;

namespace Doctor.Web.Controllers
{
    /// <summary>
    /// TIPODOCUMENTO CONTROLLER responsible for GET/POST/DELETE/PUT
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TipoDocumentosController : ControllerBase
    {
        private readonly DbDocContext _context;

        public TipoDocumentosController(DbDocContext context)
        {
            _context = context;
        }

        // GET: api/TipoDocumentos/List
        /// <summary>
        /// THIS GET METHOD RETURNS TIPODOCUMENTO LIST
        /// </summary>
        /// <returns>ARRAY OF TIPODOCUMENTO</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<TipoDocumentoViewModel>> List()
        {
            var tipodocumentoList = await _context.TipoDocumentos.ToListAsync();

            return tipodocumentoList.Select(c => new TipoDocumentoViewModel
            {
                TipoDocumentoId = c.TipoDocumentoId,
                Nombre = c.Nombre
            });
        }

        // GET: api/TipoDocumentos/Show/5
        /// <summary>
        /// THIS GET METHOD RETURNS TIPODOCUMENTO OBJECT BY TIPODOCUMENTOID
        /// </summary>
        /// <param name="TipoDocumentoId"></param>
        /// <returns>OBJECT OF TIPODOCUMENTO</returns>
        [HttpGet("[action]/{TipoDocumentoId}")]
        public async Task<ActionResult<TipoDocumento>> Show([FromRoute] int TipoDocumentoId)
        {
            var tipodocumento = await _context.TipoDocumentos.FindAsync(TipoDocumentoId);

            if (tipodocumento == null)//Si es que no existe
            {

                return NotFound(); //NotFound404
            }

            return Ok(new TipoDocumentoViewModel
            {
                TipoDocumentoId = tipodocumento.TipoDocumentoId,
                Nombre = tipodocumento.Nombre,
            });
        }
    }
}
