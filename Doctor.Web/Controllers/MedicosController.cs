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
using Microsoft.Extensions.Configuration;

namespace Doctor.Web.Controllers 
{
    /// <summary>
    /// MEDICOS CONTROLLER responsible for GET/POST/DELETE/PUT 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly DbDocContext _context;

        public MedicosController(DbDocContext context)
        {
            _context = context;
        }

        // GET: api/Medicos/List
        /// <summary>
        /// THIS GET METHOD RETURNS MEDICOS LIST
        /// </summary>
        /// <returns>ARRAY OF MEDICOS</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<MedicoViewModel>> List()
        {
            var medicoList = await _context.Medicos.ToListAsync();

            return medicoList.Select(c => new MedicoViewModel
            {
                MedicoId = c.MedicoId,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Usuario = c.Usuario,
                Clinica = c.Clinica,
                Password = c.Password
            });
        }

        // GET: api/Medicos/Show/5
        /// <summary>
        /// THIS GET METHOD RETURNS MEDICOS OBJECT BY MEDICOID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OBJECT OF MEDICOS</returns>
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Medico>> Show([FromRoute] int id)
        {
            var medico = await _context.Medicos.FindAsync(id);

            if (medico == null)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            if (medico.Nombre == null)
            {
                medico.Nombre = "Null";
            }

            if (medico.Apellido == null)
            {
                medico.Apellido = "Null";
            }

            if (medico.Usuario == null)
            {
                medico.Usuario = "Null";
            }

            if (medico.Password == null)
            {
                medico.Password = "Null";
            }
            return Ok(new MedicoViewModel
            {
                MedicoId = medico.MedicoId,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Usuario = medico.Usuario,
                Clinica = medico.Clinica,
                Password = medico.Password
            });
        }

        // GET: api/Medicos/Authenticate
        /// <summary>
        /// THIS GET METHOD RETURNS MEDICOS OBJECT BY USUARIO AND PASSWORD IN BODY
        /// </summary>
        /// <param name="model"></param>
        /// <returns>OBJECT OF MEDICOS</returns>
        [HttpPost("[action]")]
        public ActionResult<Medico> Authenticate([FromBody]LoginMedicoViewModel model)
        {
            //var user = _userService.Authenticate(medico.Usuario, medico.Password);

            var med = from m in _context.Medicos select m;

            var objEncrypt = new Encrypt();
            string pass = objEncrypt.EncryptText(model.Password, new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Values")["Password"]);

            

            if (!String.IsNullOrEmpty(model.Usuario) && !String.IsNullOrEmpty(pass))
            {
                med = med.Where(s => s.Usuario.Equals(model.Usuario) && s.Password.Equals(pass));
            }
            

            if (med.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok( med.Where( s => s.Usuario.Equals(model.Usuario) && s.Password.Equals(pass)));
        }

        // GET: api/Medicos/Login/usuario=?&contrasena=? 
        /// <summary>
        /// THIS GET METHOD RETURNS MEDICOS OBJECT BY USUARIO AND PASSWORD
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns>OBJECT OF MEDICOS</returns>
        [HttpGet("[action]/usuario={Usuario}&contrasena={Password}")]
        public async Task<ActionResult<Medico>> Login([FromRoute] string Usuario, [FromRoute] string Password)
        {
            // Proyecto proyecto = _context.Proyectos.Where(p => p.NombreProyecto.Equals(pNombreProyecto)).FirstOrDefault();

            var medico = from m in _context.Medicos select m;

            var objEncrypt = new Encrypt();
            string pass = objEncrypt.EncryptText(Password, new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Values")["Password"]);


            if (!String.IsNullOrEmpty(Usuario) && !String.IsNullOrEmpty(pass))
            {
                medico = medico.Where(s => s.Usuario.Equals(Usuario) && s.Password.Equals(pass));
            }


            if (medico.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await medico.ToListAsync());

        }

        // GET: api/Medicos/obtenerNombre/5
        /// <summary>
        /// THIS GET METHOD RETURNS MEDICOS OBJECT BY MEDICOID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Medico>> obtenerNombre([FromRoute] int id)
        {
            var medico = await _context.Medicos.FindAsync(id);

            if (medico == null)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            if (medico.Nombre == null)
            {
                medico.Nombre = "Null";
            }

            return Ok(new obtenerNombreViewModel
            {

                Nombre = medico.Nombre,
            });
        }

        // GET: api/Medicos/ShowName/R00t_5layer
        /// <summary>
        /// THIS GET METHOD RETURNS MEDICOS OBJECT BY USUARIO
        /// </summary>
        /// <param name="Usuario"></param>
        /// <returns>OBJECT OF MEDICOS</returns>
        [HttpGet("[action]/{NombreUsuario}")]
        public async Task<ActionResult<Medico>> ShowName([FromRoute] string Usuario)
        {
            var medico = await _context.Medicos.FindAsync(Usuario);

            if (medico == null)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            var objEncrypt = new Encrypt();
            string pass = objEncrypt.EncryptText(medico.Password, new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Values")["Password"]);

            return Ok(new MedicoViewModel
            {
                MedicoId = medico.MedicoId,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Usuario = medico.Usuario,
                Clinica = medico.Clinica,
                Password = pass
            });
        }

        // PUT: api/Medicos/Update/5
        /// <summary>
        /// THIS PUT METHOD MODIFY MEDICOS BY MEDICOID
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateMedicoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            if (model.MedicoId <= 0)
            {
                return BadRequest();
            }

            var use = await _context.Medicos.FirstOrDefaultAsync(c => c.MedicoId == model.MedicoId); //FirstOrDefaultAsync el primer objeto que coincide

            if (use == null)
            {
                return NotFound();
            }

            var objEncrypt = new Encrypt();
            string pass = objEncrypt.EncryptText(model.Password, new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Values")["Password"]);

            use.Nombre = model.Nombre;
            use.Apellido = model.Apellido;
            use.Clinica = model.Clinica;
            use.Usuario = model.Usuario;
            use.Password = pass;

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



        // POST: api/Medicos/Create
        /// <summary>
        /// THIS POST METHOD CREATE MEDICOS 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> Create([FromBody] CreateMedicoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            var objEncrypt = new Encrypt();
            string pass = objEncrypt.EncryptText(model.Password, new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Values")["Password"]);

            Medico use = new Medico
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Clinica = model.Clinica,
                Usuario = model.Usuario,
                Password = pass
            };

            _context.Medicos.Add(use); //agregamos, el objeto lo pongo en un insert

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


    }
}
