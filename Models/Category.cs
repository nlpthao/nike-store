namespace NikeStyle.Models;
public class Category {
    public int CategoryID {get;set;}
    public string? Name {get;set;}
    public int? ParentCategoryID {get;set;} //Self Referencing foreign key
    public Category ParentCategory {get;set;} //Navigation property for parent
    public ICollection<Category> SubCategories {get;set;} //Navigation property for Subcategories
    public ICollection<Product> Products {get;set;} //Navigation properties for products

}