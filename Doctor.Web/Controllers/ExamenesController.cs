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
    /// EXAMENES CONTROLLER responsible for GET/POST/DELETE/PUT 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExamenesController : ControllerBase
    {
        private readonly DbDocContext _context;

        public ExamenesController(DbDocContext context)
        {
            _context = context;
        }

        // GET: api/Examenes/List
        /// <summary>
        /// THIS GET METHOD RETURNS EXAMENES LIST
        /// </summary>
        /// <returns>ARRAY OF EXAMENES</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<ExamenViewModel>> List()
        {
            var examenList = await _context.Examenes.ToListAsync();

            return examenList.Select(c => new ExamenViewModel
            {
                ExamenId = c.ExamenId,
                Nombre = c.Nombre
            });
        }

        // GET: api/Examenes/Show/5
        /// <summary>
        /// THIS GET METHOD RETURNS EXAMENES OBJECT BY EXAMENID
        /// </summary>
        /// <param name="ExamenId"></param>
        /// <returns>OBJECT OF EXAMENES</returns>
        [HttpGet("[action]/{ExamenId}")]
        public async Task<ActionResult<Examen>> Show([FromRoute] int ExamenId)
        {
            var examen = await _context.Examenes.FindAsync(ExamenId);

            if (examen == null)//Si es que no existe
            {

                return NotFound(); //NotFound404
            }

            return Ok(new ExamenViewModel
            {
                ExamenId = examen.ExamenId,
                Nombre = examen.Nombre,
            });
        }
    }
}
