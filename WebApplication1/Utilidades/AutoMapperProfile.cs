using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Entidades;

namespace WebApplication1.Utilidades
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Autor, AutorDTO>();
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTOconLibros>().ForMember(AutorDTO => AutorDTO.Libros,opciones=> opciones.MapFrom(MapAutorDTOLibros));

            CreateMap<Libro, LibroGetDTO>();
            CreateMap<LibroPost, Libro>().ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroConAutores>().ForMember(libroDTO=> libroDTO.Autores, opciones=> opciones.MapFrom(MapLibroDTOAutores));

            CreateMap<LibroPachtDTO,Libro>().ReverseMap();

            CreateMap<ComentarioPostDTO, Comentario>();

            CreateMap<Comentario, ComentarioDTO>();
        }


        private List<LibroGetDTO> MapAutorDTOLibros(Autor autor, AutorDTOconLibros autorDTO)
        {
            var resultado = new List<LibroGetDTO>();
            if (autor.AutoresLibros == null)
            {
                return resultado;
            }

            foreach (var autorid in autor.AutoresLibros)
            {
                resultado.Add(new LibroGetDTO() { Id = autorid.LibroId,Titulo=autorid.Libro.Titulo });

            }
            return resultado;
        }

        private List<AutoresLibros> MapAutoresLibros(LibroPost libroPost, Libro libro)
        {
            var resultados = new List<AutoresLibros>();

            if(libroPost.AutoresIds == null)
            {
                return resultados;
            }

            foreach(var autorid in libroPost.AutoresIds)
            {
                resultados.Add(new AutoresLibros() { AutorId=autorid });

            }
            return resultados;
        }


        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroGetDTO libroGetDTO)
        {
            var resultados = new List<AutorDTO>();

            if (libro.AutoresLibros == null)
            {
                return resultados;
            }

            foreach (var autorid in libro.AutoresLibros)
            {
                resultados.Add(new AutorDTO() { id = autorid.AutorId,Nombre=autorid.Autor.Nombre });

            }
            return resultados;
        }
    }
}
