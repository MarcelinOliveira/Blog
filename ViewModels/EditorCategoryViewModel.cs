using System.ComponentModel.DataAnnotations;

namespace BlogVisualStudio.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O Nome é um campo requerido")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 a 40 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O Slug é um campo requerido")]
        public string Slug { get; set; }
    }
}
