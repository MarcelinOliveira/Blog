using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogEF.Models
{

    public class Tag
    {
        public List<Post> Posts { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}