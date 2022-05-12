using System.ComponentModel.DataAnnotations;

namespace BlogVisualStudio.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "The Password is a required field")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "The Email is a required field")]
    [EmailAddress(ErrorMessage = "Format not accept")]
    public string? Email { get; set; }
}