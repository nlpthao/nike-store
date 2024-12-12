using System.ComponentModel.DataAnnotations;

namespace NikeStyle.Models;
public class ForgotPasswordViewModel
{
    [Required(ErrorMessage ="Email is required")]
    [EmailAddress]
    public string Email {get;set;}
};