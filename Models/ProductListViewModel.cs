namespace NikeStyle.Models;
public class ProductListViewModel
{
    public IEnumerable<Product> Products {get;set;}
    public IEnumerable<Category> Categories {get;set;}
    public int? SelectedCategoryId {get;set;}
    public int CurrentPage {get;set;}
    public int TotalPages {get;set;}
}