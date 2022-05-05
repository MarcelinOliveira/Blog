namespace BlogVisualStudio.Models
{
    public class Tag
    {
        public List<Post> Posts { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}