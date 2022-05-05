using System.ComponentModel.DataAnnotations;

namespace BlogVisualStudio.ViewModels
{
    public class EditorCategoryViewModel
    {
        public EditorCategoryViewModel(string slug, string name)
        {
            Slug = slug;
            Name = name;
        }

        [Required(ErrorMessage = "The Name is a required camp")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Name must have contains 3-40 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Slug is a required camp")]
        public string Slug { get; set; }
    }
}