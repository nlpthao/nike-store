using System.ComponentModel.DataAnnotations.Schema;

namespace NikeStyle.Models;
public class Product {
    public int ProductId {get;set;}
    public string? Name {get;set;}
    public int CategoryID {get;set;}
    public string? Description {get;set;}
    public decimal? Price {get;set;}
    public string? ImageUrl {get;set;}

    [NotMapped]
    public IFormFile ImageFile {get;set;}
    public int Stock {get;set;}
    public Category Category {get;set;} //Navigation property
    public ICollection<ProductTag> ProductTags {get;set;} // Many to Mnay navigation
}