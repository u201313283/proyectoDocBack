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
    /// MEDICAMENTOS CONTROLLER responsible for GET/POST/DELETE/PUT 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MedicamentosController : ControllerBase
    {
        private readonly DbDocContext _context;

        public MedicamentosController(DbDocContext context)
        {
            _context = context;
        }

        // GET: api/Medicamentos/List
        /// <summary>
        /// THIS GET METHOD RETURNS MEDICAMENTOS LIST
        /// </summary>
        /// <returns>ARRAY OF MEDICAMENTOS</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<MedicamentoViewModel>> List()
        {
            var medicamentoList = await _context.Medicamentos.ToListAsync();

            return medicamentoList.Select(c => new MedicamentoViewModel
            {
                MedicamentoId = c.MedicamentoId,
                Nombre = c.Nombre,
                EnfermedadId = c.EnfermedadId
            });
        }

        // GET: api/Medicamentos/Show/5
        /// <summary>
        /// THIS GET METHOD RETURNS MEDICAMENTOS OBJECT BY MEDICAMENTOID
        /// </summary>
        /// <param name="MedicamentoId"></param>
        /// <returns>OBJECT OF MEDICAMENTOS</returns>
        [HttpGet("[action]/{MedicamentoId}")]
        public async Task<ActionResult<Medicamento>> Show([FromRoute] int MedicamentoId)
        {
            var medicamento = await _context.Medicamentos.FindAsync(MedicamentoId);

            if (medicamento == null)//Si es que no existe
            {

                return NotFound(); //NotFound404
            }

            return Ok(new MedicamentoViewModel
            {
                MedicamentoId = medicamento.MedicamentoId,
                Nombre = medicamento.Nombre,
                EnfermedadId = medicamento.EnfermedadId
            });
        }


        // GET: api/Medicamentos/ShowName/Omeprazol 
        /// <summary>
        /// THIS GET METHOD RETURNS MEDICAMENTO OBJECT BY NAME
        /// </summary>
        /// <param name="Nombre"></param>
        /// <returns>MEDICAMENTO OBJECT</returns>
        [HttpGet("[action]/{Nombre}")]
        public async Task<ActionResult<Medicamento>> ShowName([FromRoute] string Nombre)
        {

            var medicamento = from m in _context.Medicamentos select m;

            if (!String.IsNullOrEmpty(Nombre))
            {
                medicamento = medicamento.Where(s => s.Nombre.Equals(Nombre));
            }


            if (medicamento.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await medicamento.ToListAsync());

        }
    }
}
