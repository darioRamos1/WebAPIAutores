using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;
using WebAPIAutores.Filtros;
    
namespace WebAPIAutores.Controllers
{
    [ApiController]
    //[Route("api/[controller]")] // asi se puede obtener el nombre de la clase controller y colocarlo como ruta
    [Route("api/autores")]
   
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper )
        {
            this.context = context;
            this.mapper = mapper;
        }

       [HttpGet] //api/autores
        [AllowAnonymous]
        [HttpGet("listado")] //api/autores/listado
       [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
 
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }
     
        [HttpGet("primerautor")]
        //puedo rescartar info de un querystring
        public async Task<ActionResult<AutorDTO>> PrimerAutor([FromHeader] int miValor, [FromQuery] string nombre)
        {
            //var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(); recordad que el include es para añadir entidades  
            var autor = await context.Autores.FirstOrDefaultAsync();

            return mapper.Map<AutorDTO>(autor);
        }


        // un ejemplo del model bulding y como obtener datos desde una parte especifica con la etiqueta fromRoute
        [HttpGet("{nombre}")]
        public async Task<ActionResult<AutorDTO>> GetId([FromRoute] string nombre)
        {
            // si deseo traer una lista de autores es de la siguiente manera 

            var autor = await context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
            //   var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (autor == null)
            {
                return NotFound();
            }
            return Ok(autor);
        }

        [HttpPost]
        // este etiqueta quiere decir que vendra desde el cuerpo de la peticion
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {

            var existeAutorConElmismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);
            if (existeAutorConElmismoNombre)
            {
                return BadRequest($"Ya existe el autor con este nombre {autorCreacionDTO.Nombre}");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
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
