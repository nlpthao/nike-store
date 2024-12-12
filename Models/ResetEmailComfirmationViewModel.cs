using System.ComponentModel.DataAnnotations;
public class ResendEmailConfirmationViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; }
}
