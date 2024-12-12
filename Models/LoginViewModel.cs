using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
namespace NikeStyle.Models;
public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email {get;set;}
    
    [Required(ErrorMessage ="Password is required.")]
    [DataType(DataType.Password)]
    public string Password{get;set;}
    public bool RememberMe {get;set;}
    // URL to redirect to after sucessfully login
    public string ReturnUrl {get;set;}

    // List of external lgoin providers (Google, Facebook)
    public IList<AuthenticationScheme> ExternalLogins {get;set;}

}