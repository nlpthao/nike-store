namespace NikeStyle.Models;
public class Tag {
    public int TagID {get;set;}
    public string? Name {get;set;}
    public string Description {get;set;}
    public int? ParentTagID {get;set;} //Navigation property for parent tag
    public Tag? ParentTag {get;set;}
    public ICollection<Tag> SubTags {get;set;} = new List<Tag>(); //Navigation property for sub-tags
    public ICollection<ProductTag> ProductTags {get;set;} = new List<ProductTag>();//Many to many navigation
}