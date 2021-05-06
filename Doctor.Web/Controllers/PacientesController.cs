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
    /// PACIENTES CONTROLLER responsible for GET/POST/DELETE/PUT 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly DbDocContext _context;

        public PacientesController(DbDocContext context)
        {
            _context = context;
        }


        // GET: api/Pacientes/List
        /// <summary>
        /// THIS GET METHOD RETURNS PACIENTE LIST
        /// </summary>
        /// <returns>ARRAY OF PACIENTES</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<PacienteViewModel>> List()
        {
            var pacienteList = await _context.Pacientes.ToListAsync();

            return pacienteList.Select(c => new PacienteViewModel
            {
                PacienteId = c.PacienteId,
                Nombre = c.Nombre,
                ApellidoPaterno = c.ApellidoPaterno,
                ApellidoMaterno = c.ApellidoMaterno,
                TipoDocumentoId = c.TipoDocumentoId,
                NumeroDocumento = c.NumeroDocumento,
                Direccion = c.Direccion,
                Sexo = c.Sexo,
                FechaNacimiento = c.FechaNacimiento,
                Telefono = c.Telefono,
                Celular = c.Celular,
                Correo = c.Correo,
            });
        }

        // GET: api/Pacientes/Show/5
        /// <summary>
        /// THIS GET METHOD RETURNS PACIENTE OBJECT BY  PACIENTEID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PACIENTE OBJECT</returns>
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Paciente>> Show([FromRoute] int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null)//Si es que no existe
            {

                return NotFound(); //NotFound404
            }

            return Ok(new PacienteViewModel
            {
                PacienteId = paciente.PacienteId,
                Nombre = paciente.Nombre,
                ApellidoPaterno = paciente.ApellidoPaterno,
                ApellidoMaterno = paciente.ApellidoMaterno,
                TipoDocumentoId = paciente.TipoDocumentoId,
                NumeroDocumento = paciente.NumeroDocumento,
                Direccion = paciente.Direccion,
                Sexo = paciente.Sexo,
                FechaNacimiento = paciente.FechaNacimiento,
                Telefono = paciente.Telefono,
                Celular = paciente.Celular,
                Correo = paciente.Correo
            });
        }

        // GET: api/Pacientes/ShowName/R00t 
        /// <summary>
        /// THIS GET METHOD RETURNS PACIENTE OBJECT BY NAME
        /// </summary>
        /// <param name="Nombre"></param>
        /// <returns>PACIENTE OBJECT</returns>
        [HttpGet("[action]/{Nombre}")]
        public async Task<ActionResult<Paciente>> ShowName([FromRoute] string Nombre)
        {

            var paciente = from m in _context.Pacientes select m;

            if (!String.IsNullOrEmpty(Nombre))
            {
                paciente = paciente.Where(s => s.Nombre.Equals(Nombre));
            }


            if (paciente.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await paciente.ToListAsync());

        }

        // GET: api/Pacientes/DoctorPaciente/17 
        /// <summary>
        /// THIS GET METHOD RETURNS PACIENTE OBJECT BY MEDICOID
        /// </summary>
        /// <param name="MedicoId"></param>
        /// <returns>PACIENTE OBJECT</returns>
        [HttpGet("[action]/{MedicoId}")]
        public async Task<ActionResult<Paciente>> DoctorPaciente([FromRoute] int MedicoId)
        {


            var paciente = from m in _context.Pacientes select m;

            if (MedicoId > 0 /*&& MedicoId != null*/)
            {
                paciente = paciente.Where(s => s.MedicoId == MedicoId );
            }


            if (paciente.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await paciente.ToListAsync());

        }

        
        // PUT: api/Pacientes/Update/5
        /// <summary>
        /// THIS PUT METHOD MODIFY PACIENTE 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPut("[action]/{PacienteId}")]
        public async Task<IActionResult> Update([FromBody] UpdatePacienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            if (model.PacienteId <= 0)
            {
                return BadRequest();
            }

            var cat = await _context.Pacientes.FirstOrDefaultAsync(c => c.PacienteId == model.PacienteId); //FirstOrDefaultAsync el primer objeto que coincide

            if (cat == null)
            {
                return NotFound();
            }

            cat.Nombre = model.Nombre;
            cat.ApellidoPaterno = model.ApellidoPaterno;
            cat.ApellidoMaterno = model.ApellidoMaterno;
            cat.NumeroDocumento = model.NumeroDocumento;
            cat.Direccion = model.Direccion;
            cat.Sexo = model.Sexo;
            cat.FechaNacimiento = model.FechaNacimiento;
            cat.Telefono = model.Telefono;
            cat.Celular = model.Celular;
            cat.Correo = model.Correo;
            cat.PacienteId = model.PacienteId;

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

        // POST: api/Pacientes/Create
        /// <summary>
        /// THIS POST METHOD CREATE PACIENTE 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> Create([FromBody] CreatePacienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            Paciente pro = new Paciente
            {
                Nombre = model.Nombre,
                ApellidoPaterno = model.ApellidoPaterno,
                ApellidoMaterno = model.ApellidoMaterno,
                TipoDocumentoId = model.TipoDocumentoId,
                NumeroDocumento = model.NumeroDocumento,
                Direccion = model.Direccion,
                Sexo = model.Sexo,
                FechaNacimiento = model.FechaNacimiento,
                Telefono = model.Telefono,
                Celular = model.Celular,
                Correo = model.Correo,
                MedicoId = model.MedicoId
            };

            _context.Pacientes.Add(pro); //agregamos, el objeto lo pongo en un insert

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

        // DELETE: api/Pacientes/Delete/5
        /// <summary>
        /// THIS DELETE METHOD DELETE PACIENTE OBJECT BY PACIENTEID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200 OR 404</returns>
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                return NotFound();
            }
            _context.Pacientes.Remove(paciente); //pone la query

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
