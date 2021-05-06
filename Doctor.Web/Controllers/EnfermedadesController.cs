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
    /// ENFERMEDADES CONTROLLER responsible for GET/POST/DELETE/PUT 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EnfermedadesController : ControllerBase
    {
        private readonly DbDocContext _context;

        public EnfermedadesController(DbDocContext context)
        {
            _context = context;
        }

        // GET: api/Enfermedades/List
        /// <summary>
        /// THIS GET METHOD RETURNS ENFERMEDADES LIST
        /// </summary>
        /// <returns>ARRAY OF ENFERMEDADES</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<EnfermedadViewModel>> List()
        {
            var enfermedadList = await _context.Enfermedades.ToListAsync();

            return enfermedadList.Select(c => new EnfermedadViewModel
            {
                EnfermedadId = c.EnfermedadId,
                Nombre = c.Nombre
            });
        }

        // GET: api/Enfermedades/Show/5
        /// <summary>
        /// THIS GET METHOD RETURNS ENFERMEDADES OBJECT BY ENFERMEDADID
        /// </summary>
        /// <param name="EnfermedadId"></param>
        /// <returns>OBJECT OF ENFERMEDADES</returns>
        [HttpGet("[action]/{EnfermedadId}")]
        public async Task<ActionResult<Enfermedad>> Show([FromRoute] int EnfermedadId)
        {
            var enfermedad = await _context.Enfermedades.FindAsync(EnfermedadId);

            if (enfermedad == null)//Si es que no existe
            {

                return NotFound(); //NotFound404
            }

            return Ok(new EnfermedadViewModel
            {
                EnfermedadId = enfermedad.EnfermedadId,
                Nombre = enfermedad.Nombre,
            });
        }


        // GET: api/Enfermedades/ShowName/Cancer 
        /// <summary>
        /// THIS GET METHOD RETURNS ENFERMEDAD OBJECT BY NAME
        /// </summary>
        /// <param name="Nombre"></param>
        /// <returns>ENFERMEDAD OBJECT</returns>
        [HttpGet("[action]/{Nombre}")]
        public async Task<ActionResult<Enfermedad>> ShowName([FromRoute] string Nombre)
        {

            var enfermedad = from m in _context.Enfermedades select m;

            if (!String.IsNullOrEmpty(Nombre))
            {
                enfermedad = enfermedad.Where(s => s.Nombre.Equals(Nombre));
            }


            if (enfermedad.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await enfermedad.ToListAsync());

        }
    }
}
