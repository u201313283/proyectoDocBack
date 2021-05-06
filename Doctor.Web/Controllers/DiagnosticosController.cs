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
    /// CITAS CONTROLLER responsible for GET/POST/DELETE/PUT 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticosController : ControllerBase
    {
        private readonly DbDocContext _context;

        public DiagnosticosController(DbDocContext context)
        {
            _context = context;
        }

        // GET: api/Diagnosticos/List
        /// <summary>
        /// THIS GET METHOD RETURNS DIAGNOSTICOS LIST
        /// </summary>
        /// <returns>ARRAY OF DIAGNOSTICOS</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<DiagnosticoViewModel>> List()
        {
            var diagnosticoList = await _context.Diagnosticos.ToListAsync();

            return diagnosticoList.Select(c => new DiagnosticoViewModel
            {
                DiagnosticoId = c.DiagnosticoId,
                CitaId = c.CitaId,
                EnfermedadId = c.EnfermedadId,
                Observacion = c.Observacion,
            });
        }

        // GET: api/Diagnosticos/Show/5
        /// <summary>
        /// THIS GET METHOD RETURNS DIAGNOSTICOS OBJECT BY DIAGNOSTICOID
        /// </summary>
        /// <param name="DiagnosticoId"></param>
        /// <returns>OBJECT OF DIAGNOSTICOS</returns>
        [HttpGet("[action]/{DiagnosticoId}")]
        public async Task<ActionResult<Diagnostico>> Show([FromRoute] int DiagnosticoId)
        {
            var diagnostico = await _context.Diagnosticos.FindAsync(DiagnosticoId);

            if (diagnostico == null)//Si es que no existe
            {

                return NotFound(); //NotFound404
            }

            return Ok(new DiagnosticoViewModel
            {
                DiagnosticoId = diagnostico.DiagnosticoId,
                CitaId = diagnostico.CitaId,
                EnfermedadId = diagnostico.EnfermedadId,
                Observacion = diagnostico.Observacion,
            });
        }

        // GET: api/Diagnosticos/DiagnosticoCita/17 
        /// <summary>
        /// THIS GET METHOD RETURNS Diagnosticos OBJECT BY CitaId
        /// </summary>
        /// <param name="CitaId"></param>
        /// <returns>CITAS OBJECT</returns>
        [HttpGet("[action]/{CitaId}")]
        public async Task<ActionResult<Diagnostico>> DiagnosticoCita([FromRoute] int CitaId)
        {




            //var diagnostico = from m in _context.Diagnosticos select m;
            var diagnostico = from diagnosticos in _context.Diagnosticos
                              join enfermedades in _context.Enfermedades on diagnosticos.EnfermedadId equals enfermedades.EnfermedadId
                              select new { diagnosticos.DiagnosticoId, diagnosticos.EnfermedadId, diagnosticos.CitaId, diagnosticos.Observacion, enfermedades.Nombre };

            if (CitaId > 0 /*&& MedicoId != null*/)
            {
                diagnostico = diagnostico.Where(s => s.CitaId == CitaId);
            }


            if (diagnostico.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await diagnostico.ToListAsync());

        }

        // GET: api/Diagnosticos/ShowDiagnostico/Tiene...   SE PODRÍA CAMBIAR LA TABLA MÁS ADELANTE
        /// <summary>
        /// THIS GET METHOD RETURNS DIAGNOSTICOS OBJECT BY OBSERVACÍON
        /// </summary>
        /// <param name="Observacion"></param>
        /// <returns>OBJECT OF DIAGNOSTICOS</returns>
        [HttpGet("[action]/{Observacion}")]
        public async Task<ActionResult<Diagnostico>> ShowDiagnostico([FromRoute] string Observacion)
        {

            var diagnostico = from m in _context.Diagnosticos select m;

            if (!String.IsNullOrEmpty(Observacion))
            {
                diagnostico = diagnostico.Where(s => s.Observacion.Equals(Observacion));
            }
                

            if (diagnostico.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await diagnostico.ToListAsync());

        }


        // PUT: api/Diagnosticos/PUpdate/5
        /// <summary>
        /// THIS PUT METHOD MODIFY DIAGNOSTICOS BY DIAGNOSTICOID
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPut("[action]/{DiagnosticoId}")]
        public async Task<IActionResult> Update([FromBody] UpdateDiagnosticoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            if (model.DiagnosticoId <= 0)
            {
                return BadRequest();
            }

            var cat = await _context.Diagnosticos.FirstOrDefaultAsync(c => c.DiagnosticoId == model.DiagnosticoId); //FirstOrDefaultAsync el primer objeto que coincide

            if (cat == null)
            {
                return NotFound();
            }

            cat.DiagnosticoId = model.DiagnosticoId;
            cat.CitaId = model.CitaId;
            cat.EnfermedadId = model.EnfermedadId;
            cat.Observacion = model.Observacion;

            try
            {//await es para que espere
                await _context.SaveChangesAsync(); //Para guardar los cambios
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
            return Ok();
        }

        // POST: api/Diagnosticos/Create
        /// <summary>
        /// THIS POST METHOD CREATE DIAGNOSTICOS 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> Create([FromBody] CreateDiagnosticoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            Diagnostico pro = new Diagnostico
            {
                CitaId = model.CitaId,
                EnfermedadId = model.EnfermedadId,
                Observacion = model.Observacion,
            };

            _context.Diagnosticos.Add(pro); //agregamos, el objeto lo pongo en un insert

            try
            {
                await _context.SaveChangesAsync(); //acá guarda en db recién, acá ejecuta el insert recién
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
            return Ok();
        }

        // DELETE: api/Diagnosticos/Delete/5
        /// <summary>
        /// THIS DELETE METHOD DELETE DIAGNOSTICOS BY DIAGNOSTICOID
        /// </summary>
        /// <param name="DiagnosticoId"></param>
        /// <returns>200 OR 404</returns>
        [HttpDelete("[action]/{DiagnosticoId}")]
        public async Task<ActionResult> Delete([FromRoute]int DiagnosticoId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            var diagnostico = await _context.Diagnosticos.FindAsync(DiagnosticoId);

            if (diagnostico == null)
            {
                return NotFound();
            }
            _context.Diagnosticos.Remove(diagnostico); //pone la query

            try
            {
                await _context.SaveChangesAsync(); //acá guarda en db recién, acá ejecuta el el remove recién
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
            return Ok();
        }
    }
}
