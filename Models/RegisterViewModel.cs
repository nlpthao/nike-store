using System.ComponentModel.DataAnnotations;

namespace NikeStyle.Models;
public class RegisterViewModel
{
    [Required(ErrorMessage ="Email is required")]
    [EmailAddress]
    public string Email {get;set;}
    
    [Required(ErrorMessage = "Code is required")]
    public string Code {get;set;}

    [Required(ErrorMessage = "First Name is required")]
    public string FirstName {get;set;}

    [Required(ErrorMessage = "Last Name is required")]
    public string LastName {get;set;}

    [Required(ErrorMessage ="Password is required")]
    [DataType(DataType.Password)]
    public string Password {get;set;}
    
    [Required(ErrorMessage = "Password Confirmation is required")]
    [DataType(DataType.Password)]
    [Compare("Password",ErrorMessage = "The password and confirmation password do not match")]    
    public string ConfirmPassword {get;set;}

    [Required(ErrorMessage = "Shopping preference is required")]
    public string ShoppingPreference {get;set;}

    [Required(ErrorMessage = "Month is required")]
    [Range(1,12)]
    public string Month {get;set;}
    [Required(ErrorMessage = "Day is required")]
    [Range(1,31)]
    public string Day {get;set;}
    [Required(ErrorMessage = "Year is required")]
    [Range(1900,2100)]
    public string Year {get;set;}
    public bool AgreeToEmails {get;set;}

    [Required(ErrorMessage = "You must agree the terms")]
    public bool AgreeToTerms {get;set;}
}