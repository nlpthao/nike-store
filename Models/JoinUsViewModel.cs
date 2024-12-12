using System.ComponentModel.DataAnnotations;
namespace NikeStyle.Models;
    public class JoinUsViewModel
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email {get;set;}
    }