namespace WebAPIAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}
