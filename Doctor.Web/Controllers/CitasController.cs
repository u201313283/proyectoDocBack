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
using System.Net.Mail;
using System.Net;

namespace Doctor.Web.Controllers
{
    /// <summary>
    /// CITAS CONTROLLER responsible for GET/POST/DELETE/PUT 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly DbDocContext _context;

        public CitasController(DbDocContext context)
        {
            _context = context;
        }

        // GET: api/Citas/List
        /// <summary>
        /// THIS GET METHOD RETURNS CITAS LIST
        /// </summary>
        /// <returns>ARRAY OF CITAS</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<CitaViewModel>> List()
        {
            var citaList = await _context.Citas.ToListAsync();

            return citaList.Select(c => new CitaViewModel
            {
                CitaId = c.CitaId,
                Motivo = c.Motivo,
                Descripcion = c.Descripcion,
                Sintomas = c.Sintomas,
                Exploracion = c.Exploracion,
                FInicio = c.FInicio,
                FFin = c.FFin,
                Hora = c.Hora,
                Indicacion = c.Indicacion,
                PacienteId = c.PacienteId,
                MedicoId = c.MedicoId,
            });
        }

        // GET: api/Citas/Show/5
        /// <summary>
        /// THIS GET METHOD RETURNS CITAS OBJECT BY CITAID
        /// </summary>
        /// <param name="CitaId"></param>
        /// <returns>OBJECT OF CITAS</returns>
        [HttpGet("[action]/{CitaId}")]
        public async Task<ActionResult<Cita>> Show([FromRoute] int CitaId)
        {
            var cita = await _context.Citas.FindAsync(CitaId);

            if (cita == null)//Si es que no existe
            {

                return NotFound(); //NotFound404
            }

            return Ok(new CitaViewModel
            {
                CitaId = cita.CitaId,
                Motivo = cita.Motivo,
                Descripcion = cita.Descripcion,
                Sintomas = cita.Sintomas,
                Exploracion = cita.Exploracion,
                FInicio = cita.FInicio,
                FFin = cita.FFin,
                Hora = cita.Hora,
                Indicacion = cita.Indicacion,
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId,
            });
        }

        // GET: api/Citas/HistorialClinico/17&5
        /// <summary>
        /// THIS GET METHOD RETURNS CITAS OBJECT BY  PatientID and CitaId
        /// </summary>
        /// <param name="PacienteId"></param>
        /// /// <param name="MedicoId"></param>   
        /// <returns>CITAS OBJECT</returns>
        [HttpGet("[action]/{PacienteId}&{MedicoId}")]
        public async Task<ActionResult<Cita>> HistorialClinico([FromRoute] int PacienteId, [FromRoute] int MedicoId)
        {




            var cita = from citas in _context.Citas
                       join diagnostico in _context.Diagnosticos on citas.CitaId equals diagnostico.CitaId
                       join enfermedad in _context.Enfermedades on diagnostico.EnfermedadId equals enfermedad.EnfermedadId
                       join receta in _context.Recetas on citas.CitaId equals receta.CitaId
                       join medicamento in _context.Medicamentos on receta.MedicamentoId equals medicamento.MedicamentoId
                       select new
                       {
                           citas.CitaId,
                           citas.Motivo,
                           citas.Descripcion,
                           citas.Sintomas,
                           //citas.Exploracion,
                           citas.FInicio,
                           //citas.FFin,
                           citas.Hora,
                           citas.MedicoId,
                           //citas.Indicacion,
                           citas.PacienteId,
                           //citas.MedicoId,
                           citas.esEliminado,
                           diagnostico.Observacion, //--
                           medicamento.Nombre, //--
                       };






            if (MedicoId > 0 /*&& MedicoId != null*/)
            {
                cita = cita.Where(s => s.MedicoId == MedicoId && s.PacienteId == PacienteId && s.esEliminado == false);
            }

            /*
            if (cita.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }
            */
            return Ok(await cita.ToListAsync());

        }

        // GET: api/Citas/DoctorCita/17 
        /// <summary>
        /// THIS GET METHOD RETURNS CITAS OBJECT BY MEDICOID
        /// </summary>
        /// <param name="MedicoId"></param>
        /// <returns>CITAS OBJECT</returns>
        [HttpGet("[action]/{MedicoId}")]
        public async Task<ActionResult<Cita>> DoctorCita([FromRoute] int MedicoId)
        {


            
            
            var cita = from citas in _context.Citas
                        join paciente in _context.Pacientes on citas.PacienteId equals paciente.PacienteId
                        join medico in _context.Medicos on citas.MedicoId equals medico.MedicoId
                        select new {citas.CitaId, citas.Motivo,citas.Descripcion,citas.Sintomas,citas.Exploracion,
                            citas.FInicio, citas.FFin, citas.Hora, citas.Indicacion,citas.PacienteId,
                            paciente.Nombre, paciente.Correo, citas.MedicoId, citas.esEliminado, medico.Apellido };






            if (MedicoId > 0 /*&& MedicoId != null*/)
            {
                cita = cita.Where(s => s.MedicoId == MedicoId && s.esEliminado == false);
            }


            if (cita.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await cita.ToListAsync());

        }

        // GET: api/Citas/PacienteCita/17 
        /// <summary>
        /// THIS GET METHOD RETURNS CITAS OBJECT BY PacienteId
        /// </summary>
        /// <param name="PacienteId"></param>
        /// <returns>CITAS OBJECT</returns>
        [HttpGet("[action]/{PacienteId}")]
        public async Task<ActionResult<Cita>> PacienteCita([FromRoute] int PacienteId)
        {




            var cita = from citas in _context.Citas
                       join paciente in _context.Pacientes on citas.PacienteId equals paciente.PacienteId
                       select new
                       {
                           citas.CitaId,
                           citas.Motivo,
                           citas.Descripcion,
                           citas.Sintomas,
                           citas.Exploracion,
                           citas.FInicio,
                           citas.FFin,
                           citas.Hora,
                           citas.Indicacion,
                           citas.PacienteId,
                           paciente.Nombre,
                           citas.MedicoId,
                           citas.esEliminado
                       };






            if (PacienteId > 0 /*&& MedicoId != null*/)
            {
                cita = cita.Where(s => s.PacienteId == PacienteId && s.esEliminado == false);
            }


            if (cita.Count() == 0)//Si es que no existe
            {
                return NotFound(); //NotFound404
            }

            return Ok(await cita.ToListAsync());

        }

        // GET: api/Citas/Notificar/Brandon&brandonalegriavivanco1998@gmail.com&22/06/2020&16:30:00
        /// <summary>
        /// THIS GET METHOD SENDS MAIL TO PATIENT, NOTIFYING THE APPOINTMENT
        /// </summary>
        /// <param name="Medico"></param>
        /// <param name="Correo"></param>
        /// <param name="Fecha"></param>
        /// <param name="Hora"></param>
        /// <returns>200 ok</returns>
        [HttpGet("[action]/{Medico}&{Correo}&{Fecha}&{Hora}")]
        public async Task<ActionResult<Cita>> Notificar([FromRoute] string Medico,[FromRoute] string Correo, [FromRoute] string Fecha, [FromRoute] string Hora)
        {


            var paciente = from m in _context.Pacientes select m;

            paciente = paciente.Where(s => s.Correo == Correo);

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(Correo));
            email.From = new MailAddress("MeDOffice.RecordatorioCitas@gmail.com");
            email.Subject = "Recordatorio de cita con el doctor " + Medico;
            email.Body = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional //EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><!--[if IE]><html xmlns='http://www.w3.org/1999/xhtml' class='ie'><![endif]--><!--[if !IE]><!--><html style='margin: 0;padding: 0;' xmlns='http://www.w3.org/1999/xhtml'><!--<![endif]--><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><title></title> <!--[if !mso]><!--><meta http-equiv='X-UA-Compatible' content='IE=edge' /><!--<![endif]--><meta name='viewport' content='width=device-width' /><style type='text/css'>@media only screen and (min-width: 620px){.wrapper{min-width:600px !important}.wrapper h1{}.wrapper h1{font-size:64px !important;line-height:63px !important}.wrapper h2{}.wrapper h2{font-size:30px !important;line-height:38px !important}.wrapper h3{}.wrapper h3{font-size:22px !important;line-height:31px !important}.column{}.wrapper .size-8{font-size:8px !important;line-height:14px !important}.wrapper .size-9{font-size:9px !important;line-height:16px !important}.wrapper .size-10{font-size:10px !important;line-height:18px !important}.wrapper .size-11{font-size:11px !important;line-height:19px !important}.wrapper .size-12{font-size:12px !important;line-height:19px !important}.wrapper .size-13{font-size:13px !important;line-height:21px !important}.wrapper .size-14{font-size:14px !important;line-height:21px !important}.wrapper .size-15{font-size:15px !important;line-height:23px !important}.wrapper .size-16{font-size:16px !important;line-height:24px !important}.wrapper .size-17{font-size:17px !important;line-height:26px !important}.wrapper .size-18{font-size:18px !important;line-height:26px !important}.wrapper .size-20{font-size:20px !important;line-height:28px !important}.wrapper .size-22{font-size:22px !important;line-height:31px !important}.wrapper .size-24{font-size:24px !important;line-height:32px !important}.wrapper .size-26{font-size:26px !important;line-height:34px !important}.wrapper .size-28{font-size:28px !important;line-height:36px !important}.wrapper .size-30{font-size:30px !important;line-height:38px !important}.wrapper .size-32{font-size:32px !important;line-height:40px !important}.wrapper .size-34{font-size:34px !important;line-height:43px !important}.wrapper .size-36{font-size:36px !important;line-height:43px !important}.wrapper .size-40{font-size:40px !important;line-height:47px !important}.wrapper .size-44{font-size:44px !important;line-height:50px !important}.wrapper .size-48{font-size:48px !important;line-height:54px !important}.wrapper .size-56{font-size:56px !important;line-height:60px !important}.wrapper .size-64{font-size:64px !important;line-height:63px !important}}</style><meta name='x-apple-disable-message-reformatting' /><style type='text/css'>/*<![CDATA[*/body{margin:0;padding:0}table{border-collapse:collapse;table-layout:fixed}*{line-height:inherit}[x-apple-data-detectors]{color:inherit !important;text-decoration:none !important}.wrapper .footer__share-button a:hover, .wrapper .footer__share-button a:focus{color:#fff !important}.btn a:hover, .btn a:focus, .footer__share-button a:hover, .footer__share-button a:focus, .email-footer__links a:hover, .email-footer__links a:focus{opacity:0.8}.preheader,.header,.layout,.column{transition:width 0.25s ease-in-out, max-width 0.25s ease-in-out}.preheader td{padding-bottom:8px}.layout,div.header{max-width:400px !important;-fallback-width:95% !important;width:calc(100% - 20px) !important}div.preheader{max-width:360px !important;-fallback-width:90% !important;width:calc(100% - 60px) !important}.snippet,.webversion{Float:none !important}.stack .column{max-width:400px !important;width:100% !important}.fixed-width.has-border{max-width:402px !important}.fixed-width.has-border .layout__inner{box-sizing:border-box}.snippet,.webversion{width:50% !important}.ie .btn{width:100%}.ie .stack .column, .ie .stack .gutter{display:table-cell;float:none !important}.ie div.preheader, .ie .email-footer{max-width:560px !important;width:560px !important}.ie .snippet, .ie .webversion{width:280px !important}.ie div.header, .ie .layout{max-width:600px !important;width:600px !important}.ie .two-col .column{max-width:300px !important;width:300px !important}.ie .three-col .column, .ie .narrow{max-width:200px !important;width:200px !important}.ie .wide{width:400px !important}.ie .stack.fixed-width.has-border, .ie .stack.has-gutter.has-border{max-width:602px !important;width:602px !important}.ie .stack.two-col.has-gutter .column{max-width:290px !important;width:290px !important}.ie .stack.three-col.has-gutter .column, .ie .stack.has-gutter .narrow{max-width:188px !important;width:188px !important}.ie .stack.has-gutter .wide{max-width:394px !important;width:394px !important}.ie .stack.two-col.has-gutter.has-border .column{max-width:292px !important;width:292px !important}.ie .stack.three-col.has-gutter.has-border .column, .ie .stack.has-gutter.has-border .narrow{max-width:190px !important;width:190px !important}.ie .stack.has-gutter.has-border .wide{max-width:396px !important;width:396px !important}.ie .fixed-width .layout__inner{border-left:0 none white !important;border-right:0 none white !important}.ie .layout__edges{display:none}.mso .layout__edges{font-size:0}.layout-fixed-width, .mso .layout-full-width{background-color:#fff}@media only screen and (min-width: 620px){.column,.gutter{display:table-cell;Float:none !important;vertical-align:top}div.preheader,.email-footer{max-width:560px !important;width:560px !important}.snippet,.webversion{width:280px !important}div.header, .layout, .one-col .column{max-width:600px !important;width:600px !important}.fixed-width.has-border,.fixed-width.x_has-border,.has-gutter.has-border,.has-gutter.x_has-border{max-width:602px !important;width:602px !important}.two-col .column{max-width:300px !important;width:300px !important}.three-col .column,.column.narrow,.column.x_narrow{max-width:200px !important;width:200px !important}.column.wide,.column.x_wide{width:400px !important}.two-col.has-gutter .column, .two-col.x_has-gutter .column{max-width:290px !important;width:290px !important}.three-col.has-gutter .column, .three-col.x_has-gutter .column, .has-gutter .narrow{max-width:188px !important;width:188px !important}.has-gutter .wide{max-width:394px !important;width:394px !important}.two-col.has-gutter.has-border .column, .two-col.x_has-gutter.x_has-border .column{max-width:292px !important;width:292px !important}.three-col.has-gutter.has-border .column, .three-col.x_has-gutter.x_has-border .column, .has-gutter.has-border .narrow, .has-gutter.x_has-border .narrow{max-width:190px !important;width:190px !important}.has-gutter.has-border .wide, .has-gutter.x_has-border .wide{max-width:396px !important;width:396px !important}}@supports (display: flex){@media only screen and (min-width: 620px){.fixed-width.has-border .layout__inner{display:flex !important}}}@media only screen and (-webkit-min-device-pixel-ratio: 2), only screen and (min--moz-device-pixel-ratio: 2), only screen and (-o-min-device-pixel-ratio: 2/1), only screen and (min-device-pixel-ratio: 2), only screen and (min-resolution: 192dpi), only screen and (min-resolution: 2dppx){.fblike{background-image:url(https://i7.createsend1.com/static/eb/master/13-the-blueprint-3/images/fblike@2x.png) !important}.tweet{background-image:url(https://i8.createsend1.com/static/eb/master/13-the-blueprint-3/images/tweet@2x.png) !important}.linkedinshare{background-image:url(https://i9.createsend1.com/static/eb/master/13-the-blueprint-3/images/lishare@2x.png) !important}.forwardtoafriend{background-image:url(https://i10.createsend1.com/static/eb/master/13-the-blueprint-3/images/forward@2x.png) !important}}@media (max-width: 321px){.fixed-width.has-border .layout__inner{border-width:1px 0 !important}.layout, .stack .column{min-width:320px !important;width:320px !important}.border{display:none}.has-gutter .border{display:table-cell}}.mso div{border:0 none white !important}.mso .w560 .divider{Margin-left:260px !important;Margin-right:260px !important}.mso .w360 .divider{Margin-left:160px !important;Margin-right:160px !important}.mso .w260 .divider{Margin-left:110px !important;Margin-right:110px !important}.mso .w160 .divider{Margin-left:60px !important;Margin-right:60px !important}.mso .w354 .divider{Margin-left:157px !important;Margin-right:157px !important}.mso .w250 .divider{Margin-left:105px !important;Margin-right:105px !important}.mso .w148 .divider{Margin-left:54px !important;Margin-right:54px !important}.mso .size-8, .ie .size-8{font-size:8px !important;line-height:14px !important}.mso .size-9, .ie .size-9{font-size:9px !important;line-height:16px !important}.mso .size-10, .ie .size-10{font-size:10px !important;line-height:18px !important}.mso .size-11, .ie .size-11{font-size:11px !important;line-height:19px !important}.mso .size-12, .ie .size-12{font-size:12px !important;line-height:19px !important}.mso .size-13, .ie .size-13{font-size:13px !important;line-height:21px !important}.mso .size-14, .ie .size-14{font-size:14px !important;line-height:21px !important}.mso .size-15, .ie .size-15{font-size:15px !important;line-height:23px !important}.mso .size-16, .ie .size-16{font-size:16px !important;line-height:24px !important}.mso .size-17, .ie .size-17{font-size:17px !important;line-height:26px !important}.mso .size-18, .ie .size-18{font-size:18px !important;line-height:26px !important}.mso .size-20, .ie .size-20{font-size:20px !important;line-height:28px !important}.mso .size-22, .ie .size-22{font-size:22px !important;line-height:31px !important}.mso .size-24, .ie .size-24{font-size:24px !important;line-height:32px !important}.mso .size-26, .ie .size-26{font-size:26px !important;line-height:34px !important}.mso .size-28, .ie .size-28{font-size:28px !important;line-height:36px !important}.mso .size-30, .ie .size-30{font-size:30px !important;line-height:38px !important}.mso .size-32, .ie .size-32{font-size:32px !important;line-height:40px !important}.mso .size-34, .ie .size-34{font-size:34px !important;line-height:43px !important}.mso .size-36, .ie .size-36{font-size:36px !important;line-height:43px !important}.mso .size-40, .ie .size-40{font-size:40px !important;line-height:47px !important}.mso .size-44, .ie .size-44{font-size:44px !important;line-height:50px !important}.mso .size-48, .ie .size-48{font-size:48px !important;line-height:54px !important}.mso .size-56, .ie .size-56{font-size:56px !important;line-height:60px !important}.mso .size-64, .ie .size-64{font-size:64px !important;line-height:63px !important}/*]]>*/</style><style type='text/css'>body{background-color:#fff}.logo a:hover,.logo a:focus{color:#859bb1 !important}.mso .layout-has-border{border-top:1px solid #ccc;border-bottom:1px solid #ccc}.mso .layout-has-bottom-border{border-bottom:1px solid #ccc}.mso .border,.ie .border{background-color:#ccc}.mso h1,.ie h1{}.mso h1,.ie h1{font-size:64px !important;line-height:63px !important}.mso h2,.ie h2{}.mso h2,.ie h2{font-size:30px !important;line-height:38px !important}.mso h3,.ie h3{}.mso h3,.ie h3{font-size:22px !important;line-height:31px !important}.mso .layout__inner,.ie .layout__inner{}.mso .footer__share-button p{}.mso .footer__share-button p{font-family:sans-serif}</style><meta name='robots' content='noindex,nofollow' /><meta property='og:title' content='My First Campaign' /></head> <!--[if mso]><body class='mso'> <![endif]--> <!--[if !mso]><!--><body class='no-padding' style='margin: 0;padding: 0;-webkit-text-size-adjust: 100%;'> <!--<![endif]--><table class='wrapper' style='border-collapse: collapse;table-layout: fixed;min-width: 320px;width: 100%;background-color: #fff;' cellpadding='0' cellspacing='0' role='presentation'><tbody><tr><td><div role='banner'><div class='preheader' style='Margin: 0 auto;max-width: 560px;min-width: 280px; width: 280px;width: calc(28000% - 167440px);'><div style='border-collapse: collapse;display: table;width: 100%;'> <!--[if (mso)|(IE)]><table align='center' class='preheader' cellpadding='0' cellspacing='0' role='presentation'><tr><td style='width: 280px' valign='top'><![endif]--><div class='snippet' style='display: table-cell;Float: left;font-size: 12px;line-height: 19px;max-width: 280px;min-width: 140px; width: 140px;width: calc(14000% - 78120px);padding: 10px 0 5px 0;color: #adb3b9;font-family: sans-serif;'></div> <!--[if (mso)|(IE)]></td><td style='width: 280px' valign='top'><![endif]--><div class='webversion' style='display: table-cell;Float: left;font-size: 12px;line-height: 19px;max-width: 280px;min-width: 139px; width: 139px;width: calc(14100% - 78680px);padding: 10px 0 5px 0;text-align: right;color: #adb3b9;font-family: sans-serif;'><p style='Margin-top: 0;Margin-bottom: 0;'>No images? <webversion style='text-decoration: underline;'>Click here</webversion></p></div> <!--[if (mso)|(IE)]></td></tr></table><![endif]--></div></div></div><div><div style='background-color: #281557;'><div class='layout three-col stack' style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;'><div class='layout__inner' style='border-collapse: collapse;display: table;width: 100%;'> <!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-full-width' style='background-color: #281557;'><td class='layout__edges'>&nbsp;</td><td style='width: 200px' valign='top' class='w160'><![endif]--><div class='column' style='text-align: left;color: #8e959c;font-size: 14px;line-height: 21px;font-family: sans-serif;max-width: 320px;min-width: 200px; width: 320px;width: calc(72200px - 12000%);Float: left;'><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 10px;font-size: 1px;'>&nbsp;</div></div><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;mso-text-raise: 11px;vertical-align: middle;'><h3 class='size-12' style='Margin-top: 0;Margin-bottom: 0;font-style: normal;font-weight: normal;color: #281557;font-size: 12px;line-height: 19px;font-family: Avenir,sans-serif;text-align: center;' lang='x-size-12'><strong><span style='color:#fff'>P&#225;gina</span></strong></h3></div></div></div> <!--[if (mso)|(IE)]></td><td style='width: 200px' valign='top' class='w160'><![endif]--><div class='column' style='text-align: left;color: #8e959c;font-size: 14px;line-height: 21px;font-family: sans-serif;max-width: 320px;min-width: 200px; width: 320px;width: calc(72200px - 12000%);Float: left;'><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 10px;font-size: 1px;'>&nbsp;</div></div></div> <!--[if (mso)|(IE)]></td><td style='width: 200px' valign='top' class='w160'><![endif]--><div class='column' style='text-align: left;color: #8e959c;font-size: 14px;line-height: 21px;font-family: sans-serif;max-width: 320px;min-width: 200px; width: 320px;width: calc(72200px - 12000%);Float: left;'><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 10px;font-size: 1px;'>&nbsp;</div></div><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;mso-text-raise: 11px;vertical-align: middle;'><h3 class='size-12' style='Margin-top: 0;Margin-bottom: 0;font-style: normal;font-weight: normal;color: #281557;font-size: 12px;line-height: 19px;font-family: Avenir,sans-serif;text-align: center;' lang='x-size-12'><span style='color:#fff'><strong>App</strong></span></h3></div></div></div> <!--[if (mso)|(IE)]></td><td class='layout__edges'>&nbsp;</td></tr></table><![endif]--></div></div></div><div style='background-color: #281557;'><div class='layout one-col stack' style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;'><div class='layout__inner' style='border-collapse: collapse;display: table;width: 100%;'> <!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-full-width' style='background-color: #281557;'><td class='layout__edges'>&nbsp;</td><td style='width: 600px' class='w560'><![endif]--><div class='column' style='text-align: left;color: #8e959c;font-size: 14px;line-height: 21px;font-family: sans-serif;'><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 10px;font-size: 1px;'>&nbsp;</div></div></div> <!--[if (mso)|(IE)]></td><td class='layout__edges'>&nbsp;</td></tr></table><![endif]--></div></div></div><div style='background-color: #4b5462;background: 0px 0px/auto auto repeat url(https://i1.createsend1.com/ei/t/22/45B/7D6/050053/csfinal/doctorBanner.jpg) #4b5462;background-position: 0px 0px;background-image: url(https://i1.createsend1.com/ei/t/22/45B/7D6/050053/csfinal/doctorBanner.jpg);background-repeat: repeat;background-size: auto auto;'><div class='layout one-col stack' style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;'><div class='layout__inner' style='border-collapse: collapse;display: table;width: 100%;'> <!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-full-width' style='background: 0px 0px/auto auto repeat url(https://i1.createsend1.com/ei/t/22/45B/7D6/050053/csfinal/doctorBanner.jpg) #4b5462;background-position: 0px 0px;background-image: url(https://i1.createsend1.com/ei/t/22/45B/7D6/050053/csfinal/doctorBanner.jpg);background-repeat: repeat;background-size: auto auto;background-color: #4b5462;'><td class='layout__edges'>&nbsp;</td><td style='width: 600px' class='w560'><![endif]--><div class='column' style='text-align: left;color: #8e959c;font-size: 14px;line-height: 21px;font-family: sans-serif;'><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 110px;font-size: 1px;'>&nbsp;</div></div><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;mso-text-raise: 11px;vertical-align: middle;'><h1 class='size-64' style='mso-text-raise: 25px;Margin-top: 0;Margin-bottom: 20px;font-style: normal;font-weight: normal;color: #000;font-size: 44px;line-height: 50px;font-family: avenir,sans-serif;text-align: center;' lang='x-size-64'><span class='font-avenir'><span style='color:#121dfc'><strong>MeDOffice</strong></span></span></h1></div></div><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 5px;font-size: 1px;'>&nbsp;</div></div><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 85px;font-size: 1px;'>&nbsp;</div></div></div> <!--[if (mso)|(IE)]></td><td class='layout__edges'>&nbsp;</td></tr></table><![endif]--></div></div></div><div style='mso-line-height-rule: exactly;line-height: 15px;font-size: 15px;'>&nbsp;</div><div class='layout one-col fixed-width stack' style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;'><div class='layout__inner' style='border-collapse: collapse;display: table;width: 100%;background-color: #ffffff;'> <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-fixed-width' style='background-color: #ffffff;'><td style='width: 600px' class='w560'><![endif]--><div class='column' style='text-align: left;color: #8e959c;font-size: 14px;line-height: 21px;font-family: sans-serif;'><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 50px;font-size: 1px;'>&nbsp;</div></div><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;mso-text-raise: 11px;vertical-align: middle;'><h2 style='Margin-top: 0;Margin-bottom: 0;font-style: normal;font-weight: normal;color: #e31212;font-size: 26px;line-height: 34px;font-family: Avenir,sans-serif;text-align: center;'><strong>RECORDATORIO DE SU CITA EL D&#205;A :&nbsp;" + Fecha + ":&nbsp;A las " + Hora + " &nbsp;</strong></h2></div></div></div> <!--[if (mso)|(IE)]></td></tr></table><![endif]--></div></div><div style='mso-line-height-rule: exactly;line-height: 20px;font-size: 20px;'>&nbsp;</div><div style='background-color: #fff;'><div class='layout two-col stack' style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;'><div class='layout__inner' style='border-collapse: collapse;display: table;width: 100%;'> <!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-full-width' style='background-color: #fff;'><td class='layout__edges'>&nbsp;</td><td style='width: 300px' valign='top' class='w260'><![endif]--><div class='column' style='text-align: left;color: #8e959c;font-size: 14px;line-height: 21px;font-family: sans-serif;max-width: 320px;min-width: 300px; width: 320px;width: calc(12300px - 2000%);Float: left;'><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 25px;font-size: 1px;'>&nbsp;</div></div><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 15px;font-size: 1px;'>&nbsp;</div></div></div> <!--[if (mso)|(IE)]></td><td style='width: 300px' valign='top' class='w260'><![endif]--><div class='column' style='text-align: left;color: #8e959c;font-size: 14px;line-height: 21px;font-family: sans-serif;max-width: 320px;min-width: 300px; width: 320px;width: calc(12300px - 2000%);Float: left;'><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 25px;font-size: 1px;'>&nbsp;</div></div></div> <!--[if (mso)|(IE)]></td><td class='layout__edges'>&nbsp;</td></tr></table><![endif]--></div></div></div><div style='mso-line-height-rule: exactly;line-height: 50px;font-size: 50px;'>&nbsp;</div><div style='background-color: #281557;'><div class='layout one-col stack' style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;'><div class='layout__inner' style='border-collapse: collapse;display: table;width: 100%;'> <!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-full-width' style='background-color: #281557;'><td class='layout__edges'>&nbsp;</td><td style='width: 600px' class='w560'><![endif]--><div class='column' style='text-align: left;color: #8e959c;font-size: 14px;line-height: 21px;font-family: sans-serif;'><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 50px;font-size: 1px;'>&nbsp;</div></div><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;mso-text-raise: 11px;vertical-align: middle;'><h1 class='size-48' style='mso-text-raise: 18px;Margin-top: 0;Margin-bottom: 20px;font-style: normal;font-weight: normal;color: #000;font-size: 36px;line-height: 43px;font-family: avenir,sans-serif;text-align: center;' lang='x-size-48'><span class='font-avenir'><font color='#ffffff'><strong>Salud y bienestar al alcance m&#225;s cercano</strong></font></span></h1></div></div><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 15px;font-size: 1px;'>&nbsp;</div></div><div style='Margin-left: 20px;Margin-right: 20px;'><div style='mso-line-height-rule: exactly;line-height: 35px;font-size: 1px;'>&nbsp;</div></div></div> <!--[if (mso)|(IE)]></td><td class='layout__edges'>&nbsp;</td></tr></table><![endif]--></div></div></div><div style='mso-line-height-rule: exactly;line-height: 20px;font-size: 20px;'>&nbsp;</div><div role='contentinfo'><div class='layout email-footer stack' style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;'><div class='layout__inner' style='border-collapse: collapse;display: table;width: 100%;'> <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-email-footer'><td style='width: 400px;' valign='top' class='w360'><![endif]--><div class='column wide' style='text-align: left;font-size: 12px;line-height: 19px;color: #adb3b9;font-family: sans-serif;Float: left;max-width: 400px;min-width: 320px; width: 320px;width: calc(8000% - 47600px);'><div style='Margin-left: 20px;Margin-right: 20px;Margin-top: 10px;Margin-bottom: 10px;'><div style='font-size: 12px;line-height: 19px;'><div>&#169;&nbsp;MeDOffice 2020 @</div></div><div style='font-size: 12px;line-height: 19px;Margin-top: 18px;'></div> <!--[if mso]>&nbsp;<![endif]--></div></div> <!--[if (mso)|(IE)]></td><td style='width: 200px;' valign='top' class='w160'><![endif]--><div class='column narrow' style='text-align: left;font-size: 12px;line-height: 19px;color: #adb3b9;font-family: sans-serif;Float: left;max-width: 320px;min-width: 200px; width: 320px;width: calc(72200px - 12000%);'><div style='Margin-left: 20px;Margin-right: 20px;Margin-top: 10px;Margin-bottom: 10px;'></div></div> <!--[if (mso)|(IE)]></td></tr></table><![endif]--></div></div><div class='layout one-col email-footer' style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;'><div class='layout__inner' style='border-collapse: collapse;display: table;width: 100%;'> <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-email-footer'><td style='width: 600px;' class='w560'><![endif]--><div class='column' style='text-align: left;font-size: 12px;line-height: 19px;color: #adb3b9;font-family: sans-serif;'><div style='Margin-left: 20px;Margin-right: 20px;Margin-top: 10px;Margin-bottom: 10px;'><div style='font-size: 12px;line-height: 19px;'> <span><preferences style='text-decoration: underline;' lang='en'>Preferences</preferences>&nbsp;&nbsp;|&nbsp;&nbsp;</span><unsubscribe style='text-decoration: underline;'>Unsubscribe</unsubscribe></div></div></div> <!--[if (mso)|(IE)]></td></tr></table><![endif]--></div></div></div><div style='line-height:40px;font-size:40px;'>&nbsp;</div></div></td></tr></tbody></table></body></html>";
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            /*                  */

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential("MeDOffice.RecordatorioCitas@gmail.com", "Fvckyourself@1821");
            string output = null;

            try
            {
                smtp.Send(email);
                email.Dispose();
                output = "Corre electrónico fue enviado satisfactoriamente.";
            }
            catch (Exception ex)
            {
                output = ex.ToString();
            }
            /*

            if (!String.IsNullOrEmpty(Correo))
            {
                
            }
            */


            return Ok(await paciente.ToListAsync());

        }

        // PUT: api/Citas/Update/5
        /// <summary>
        /// THIS PUT METHOD MODIFY CITAS BY CITAID 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPut("[action]/{CitaId}")]
        public async Task<IActionResult> Update([FromBody] UpdateCitaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            if (model.CitaId <= 0)
            {
                return BadRequest();
            }

            var cat = await _context.Citas.FirstOrDefaultAsync(c => c.CitaId == model.CitaId); //FirstOrDefaultAsync el primer objeto que coincide

            if (cat == null)
            {
                return NotFound();
            }

            cat.CitaId = model.CitaId;
            cat.Motivo = model.Motivo;
            cat.Descripcion = model.Descripcion;
            cat.Sintomas = model.Sintomas;
            cat.Exploracion = model.Exploracion;
            cat.FInicio = model.FInicio;
            cat.FFin = model.FFin;
            cat.Hora = model.Hora;
            cat.Indicacion = model.Indicacion;
            cat.PacienteId = model.PacienteId;
            cat.MedicoId = model.MedicoId;

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

        // POST: api/Citas/Create
        /// <summary>
        /// THIS POST METHOD CREATE CITA 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OR 404</returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> Create([FromBody] CreateCitaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            Cita pro = new Cita
            {
                Motivo = model.Motivo,
                Descripcion = model.Descripcion,
                Sintomas = model.Sintomas,
                Exploracion = model.Exploracion,
                FInicio = model.FInicio,
                FFin = model.FFin,
                Hora = model.Hora,
                Indicacion = model.Indicacion,
                PacienteId = model.PacienteId,
                MedicoId = model.MedicoId
        };

            _context.Citas.Add(pro); //agregamos, el objeto lo pongo en un insert

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

        // PUT: api/Citas/Delete/5
        /// <summary>
        /// THIS PUT METHOD MODIFY CITAS BY CITAID
        /// </summary>
        /// <param name="CitaId"></param>
        /// <returns>200 OR 404</returns>
        [HttpPut("[action]/{CitaId}")]
        public async Task<ActionResult> Delete([FromRoute]int CitaId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //error 404
            }

            var cat = await _context.Citas.FirstOrDefaultAsync(c => c.CitaId == CitaId); //FirstOrDefaultAsync el primer objeto que coincide

            if (cat == null)
            {
                return NotFound();
            }

            cat.esEliminado = true;

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
    }
}
