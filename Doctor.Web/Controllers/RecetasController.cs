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
    /// RECETAS CONTROLLER responsible for GET/POST/DELETE/PUT 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RecetasController : ControllerBase
    {
        private readonly DbDocContext _context;

        public RecetasController(DbDocContext context)
        {
            _context = context;
        }

        // GET: api/Recetas/List
        /// <summary>
        /// THIS GET METHOD RETURNS RECETAS LIST
        /// </summary>
        /// <returns>ARRAY OF RECETAS</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<RecetaViewModel>> List()
        {
            var recetaList = await _context.Recetas.ToListAsync();

            return recetaList.Select(c => new RecetaViewModel
            {
                RecetaId = c.RecetaId,
                MedicamentoId = c.MedicamentoId,
                Frecuencia = c.Frecuencia,
                Duracion = c.Duracion,
                Cantidad = c.Cantidad,
                CitaId = c.CitaId
            });
        }

        // GET: api/Recetas/Show/5
        /// <summary>
        /// THIS GET METHOD RETURNS RECETAS OBJECT BY RECETAID
        /// </summary>
        /// <param name="RecetaId"></param>
        /// <returns>OBJECT OF RECETAS</returns>
        [HttpGet("[action]/{RecetaId}")]
        public async Task<ActionResult<Receta>> Show([FromRoute] int RecetaId)
        {
            var receta = await _context.Recetas.FindAsync(RecetaId);

            if (receta == null)//Si es que no existe
            {

                return NotFound(); //NotFound404
            }

            return Ok(new RecetaViewModel
            {
                RecetaId = receta.RecetaId,
                MedicamentoId = receta.MedicamentoId,
                Frecuencia = receta.Frecuencia,
                Duracion = receta.Duracion,
                Cantidad = receta.Cantidad,
                CitaId = receta.CitaId
            });
        }

        // GET: api/Recetas/RecetaCita/17 
        /// <summary>
        /// THIS GET METHOD RETURNS Recetas OBJECT BY CitaId
        /// </summary>
        /// <param name="CitaId"></param>
        /// <returns>CITAS OBJECT</returns>
        [HttpGet("[action]/{CitaId}")]
        public async Task<ActionResult<Receta>> RecetaCita([FromRoute] int CitaId)
        {




            //var receta = from m in _context.Recetas select m;
            var receta = from recetas in _context.Recetas
                              join medicamentos in _context.Medicamentos on recetas.MedicamentoId equals medicamentos.MedicamentoId
                         select new { recetas.RecetaId, recetas.MedicamentoId, recetas.Frecuencia, recetas.Duracion, recetas.Cantidad, recetas.CitaId, medicamentos.Nombre };

            if (CitaId > 0 /*&& MedicoId != null*/)
            {
                receta = receta.Where(s => s.CitaId == CitaId);
            }


            if (receta.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await receta.ToListAsync());

        }

        // PUT: api/Recetas/Update/5
        /// <summary>
        /// THIS PUT METHOD MODIFY DIAGNOSTICOS BY RECETAID
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPut("[action]/{RecetaId}")]
        public async Task<IActionResult> Update([FromBody] UpdateRecetasViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            if (model.RecetaId <= 0)
            {
                return BadRequest();
            }

            var cat = await _context.Recetas.FirstOrDefaultAsync(c => c.RecetaId == model.RecetaId); //FirstOrDefaultAsync el primer objeto que coincide

            if (cat == null)
            {
                return NotFound();
            }

            cat.RecetaId = model.RecetaId;
            cat.MedicamentoId = model.MedicamentoId;
            cat.Frecuencia = model.Frecuencia;
            cat.Duracion = model.Duracion;
            cat.Cantidad = model.Cantidad;
            cat.CitaId = model.CitaId;

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

        // POST: api/Recetas/Create
        /// <summary>
        /// THIS POST METHOD CREATE DIAGNOSTICOS 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> Create([FromBody] CreateRecetasViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            Receta pro = new Receta
            {
                MedicamentoId = model.MedicamentoId,
                Frecuencia = model.Frecuencia,
                Duracion = model.Duracion,
                Cantidad = model.Cantidad,
                CitaId = model.CitaId
            };

            _context.Recetas.Add(pro); //agregamos, el objeto lo pongo en un insert

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

        // DELETE: api/Recetas/Delete/5
        /// <summary>
        /// THIS DELETE METHOD DELETE DIAGNOSTICOS BY RECETAID
        /// </summary>
        /// <param name="RecetaId"></param>
        /// <returns>200 OR 404</returns>
        [HttpDelete("[action]/{RecetaId}")]
        public async Task<ActionResult> Delete([FromRoute]int RecetaId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            var receta = await _context.Recetas.FindAsync(RecetaId);

            if (receta == null)
            {
                return NotFound();
            }
            _context.Recetas.Remove(receta); //pone la query

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
