using AutoMapper;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            /// esto es para el mapeo de las clases se coloca   
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<LibroCreacionDTO, Libro>();
            CreateMap<ComentarioCreacionDTO, Comentario>();

            CreateMap<Autor, AutorDTO>();
            CreateMap<Libro, LibroDTO>();
            CreateMap<Comentario, ComentarioDTO>();
        }
    }
}
