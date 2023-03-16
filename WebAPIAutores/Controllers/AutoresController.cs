using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entidades;
using WebAPIAutores.Servicios;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    //[Route("api/[controller]")] // asi se puede obtener el nombre de la clase controller y colocarlo como ruta
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ServicioTransient servicioTransient;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, IServicio servicio, ServicioScoped servicioScoped,
            ServicioSingleton servicioSingleton, ServicioTransient servicioTransient, ILogger<AutoresController> logger)
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            this.servicioTransient = servicioTransient;
            this.logger = logger;
        }

        [HttpGet] //api/autores
        [HttpGet("listado")] //api/autores/listado
        [HttpGet("/listado")] // listado

        public async Task<ActionResult<List<Autor>>> Get()
        {
            logger.LogInformation("Estamos obteniendo los autores");
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }
        [HttpGet("GUID")]
      //  [Authorize]
        public ActionResult ObtenerGuids()
        {
            return Ok(new
            {
                AutoresControllerTransient = servicioTransient.Guid,
                ServicioA_Transient = servicio.ObtenerTransient(),
                AutoresControllerScoped = servicioScoped.Guid,
                ServicioA_Scoped = servicio.ObtenerScoped(),
                AutoresControllerSinglenton = servicioSingleton.Guid,
                ServicioA_Singleton = servicio.ObtenerSingleton(),
            });
        }
        [HttpGet("primerautor")]
        //puedo rescartar info de un querystring
        public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor, [FromQuery] string nombre)
        {
            return await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync();
        }
        // un ejemplo del model bulding y como obtener datos desde una parte especifica con la etiqueta fromRoute
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> GetId([FromRoute] string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (autor == null)
            {
                return NotFound();
            }
            return Ok(autor);
        }

        [HttpPost]
        // este etiqueta quiere decir que vendra desde el cuerpo de la peticion
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {

            var existeAutorConElmismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);
            if (existeAutorConElmismoNombre)
            {
                return BadRequest($"Ya existe el autor con este nombre {autor.Nombre}");
            }
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] // esta manera puedo añadir a la ruta de la url  una variable 
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
                return NotFound();

            if (autor.Id != id)
            {
                return BadRequest("el id del autor no existe ");
            }
            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
                return NotFound();
            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok("Borrado completo");
        }
    }
}
