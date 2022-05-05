using System.ComponentModel.DataAnnotations.Schema;

namespace BlogVisualStudio.Models
{
    [Table("Post")]
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string Summary { get; set; }
        public string Slug { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public Category Category { get; set; }
        public List<Tag> Tags { get; set; }
    }
}