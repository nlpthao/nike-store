using System.ComponentModel.DataAnnotations;
namespace NikeStyle.Models;
public class ResendEmailConfirmationViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; }
}
