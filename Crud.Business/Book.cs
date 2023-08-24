namespace Crud.Business
{
    public class Book
    {
        public int IdBook { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}