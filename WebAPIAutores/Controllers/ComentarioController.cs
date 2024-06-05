using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/comentarios")]
    public class ComentarioController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ComentarioController(ApplicationDbContext dbContext , IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroID)
        {
         var comentarios = await dbContext.Comentarios.Where(x => x.LibroId == libroID).ToListAsync();
            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }
        [HttpPost]
        public async Task<ActionResult> Post(int libroId , ComentarioCreacionDTO comentarioDTO)
        {
            var existeLibro = await dbContext.Libros.AnyAsync(X => X.Id == libroId);
            if (!existeLibro)
            {
                return NotFound("no existe el libro");
            }
            var comentario = mapper.Map<Comentario>(comentarioDTO);
            comentario.LibroId = libroId;
            dbContext.Add(comentario);
            await dbContext.SaveChangesAsync();
            return Ok(comentario);
        }
    }
}
