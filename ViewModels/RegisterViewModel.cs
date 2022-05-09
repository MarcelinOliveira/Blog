using System.ComponentModel.DataAnnotations;

namespace BlogVisualStudio.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "The Name is a required field")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Email is a required field")]
    [EmailAddress(ErrorMessage = "Format not accept")]
    public string Email { get; set; }
}