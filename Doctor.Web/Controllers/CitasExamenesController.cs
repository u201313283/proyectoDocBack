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
    /// CITASEXAMENES CONTROLLER responsible for GET/POST/DELETE/PUT 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CitasExamenesController : ControllerBase
    {
        private readonly DbDocContext _context;

        public CitasExamenesController(DbDocContext context)
        {
            _context = context;
        }

        // GET: api/CitasExamenes/List
        /// <summary>
        /// THIS GET METHOD RETURNS CITASEXAMENES LIST
        /// </summary>
        /// <returns>ARRAY OF CITASEXAMENES</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<CitaExamenViewModel>> List()
        {
            var citaExamenList = await _context.CitasExamenes.ToListAsync();

            return citaExamenList.Select(c => new CitaExamenViewModel
            {
                CitaExamenId = c.CitaExamenId,
                CitaId = c.CitaId,
                ExamenId = c.ExamenId,
                Observacion = c.Observacion,
            });
        }

        // GET: api/CitasExamenes/Show/5
        /// <summary>
        /// THIS GET METHOD RETURNS CITASEXAMENES OBJECT BY ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OBJECT OF CITASEXAMENES</returns>
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<CitaExamen>> Show([FromRoute] int id)
        {
            var citaExamen = await _context.CitasExamenes.FindAsync(id);

            if (citaExamen == null)//Si es que no existe
            {

                return NotFound(); //NotFound404
            }

            return Ok(new CitaExamenViewModel
            {
                CitaExamenId = citaExamen.CitaExamenId,
                CitaId = citaExamen.CitaId,
                ExamenId = citaExamen.ExamenId,
                Observacion = citaExamen.Observacion,
            });
        }

        // GET: api/CitasExamenes/ShowCitaExamen/Lesión
        /// <summary>
        /// THIS GET METHOD RETURNS CITASEXAMENES OBJECT BY OBSERVACION
        /// </summary>
        /// <param name="Observacion"></param>
        /// <returns>OBJECT OF CITASEXAMENES </returns>
        [HttpGet("[action]/{Observacion}")]
        public async Task<ActionResult<CitaExamen>> ShowCitaExamen([FromRoute] string Observacion)
        {

            var citaExamen = from m in _context.CitasExamenes select m;

            if (!String.IsNullOrEmpty(Observacion))
            {
                citaExamen = citaExamen.Where(s => s.Observacion.Equals(Observacion));
            }


            if (citaExamen.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await citaExamen.ToListAsync());

        }


        // PUT: api/CitasExamenes/Update/5
        /// <summary>
        /// THIS PUT METHOD MODIFY CITASEXAMENES BY CITAEXAMENID 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPut("[action]/{CitaExamenId}")]
        public async Task<IActionResult> Update([FromBody] UpdateCitaExamenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            if (model.CitaExamenId <= 0)
            {
                return BadRequest();
            }

            var cat = await _context.CitasExamenes.FirstOrDefaultAsync(c => c.CitaExamenId == model.CitaExamenId); //FirstOrDefaultAsync el primer objeto que coincide

            if (cat == null)
            {
                return NotFound();
            }

            cat.CitaExamenId = model.CitaExamenId;
            cat.CitaId = model.CitaId;
            cat.ExamenId = model.ExamenId;
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

        // POST: api/CitasExamenes/Create
        /// <summary>
        /// THIS POST METHOD CREATE CITASEXAMENES 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> Create([FromBody] CreateCitaExamenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            CitaExamen pro = new CitaExamen
            {
                CitaId = model.CitaId,
                ExamenId = model.ExamenId,
                Observacion = model.Observacion,
            };

            _context.CitasExamenes.Add(pro); //agregamos, el objeto lo pongo en un insert

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

        // DELETE: api/CitasExamenes/Delete/5
        /// <summary>
        /// THIS DELETE METHOD DELETE CITASEXAMENES BY CITAEXAMENID
        /// </summary>
        /// <param name="CitaExamenId"></param>
        /// <returns>200 OR 404</returns>
        [HttpDelete("[action]/{CitaExamenId}")]
        public async Task<ActionResult> Delete([FromRoute]int CitaExamenId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            var citaExamen = await _context.CitasExamenes.FindAsync(CitaExamenId);

            if (citaExamen == null)
            {
                return NotFound();
            }
            _context.CitasExamenes.Remove(citaExamen); //pone la query

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
