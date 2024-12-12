using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NikeStyle.Models;
using Microsoft.AspNetCore.Identity;
namespace NikeStyle.Data {
public class ProductContext: IdentityDbContext<IdentityUser>
{
    public ProductContext(DbContextOptions<ProductContext> options): base(options) {}
    // Dbsets for each table
    public DbSet<Product> Products {get;set;}
    public DbSet<Category> Categories {get;set;}
    public DbSet<Tag> Tags {get;set;}
    public DbSet<ProductTag> ProductTags {get;set;}
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        
        //Configure self-referencing relationship for tag
        modelBuilder.Entity<Tag>()
            .HasOne(t =>t.ParentTag)
            .WithMany(t => t.SubTags)
            .HasForeignKey(t => t.ParentTagID)
            .OnDelete(DeleteBehavior.Restrict);  //Prevent cascade delete to avoid accidentally delete
        // Define relationship, such as many to many
        modelBuilder.Entity<ProductTag>()
            .HasKey(pt => new{pt.ProductID, pt.TagID}); //Composite primary key
        
        modelBuilder.Entity<ProductTag>()
            .HasOne(pt => pt.Product)
            .WithMany(p => p.ProductTags)
            .HasForeignKey(pt => pt.ProductID);
        
        modelBuilder.Entity<ProductTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.ProductTags)
            .HasForeignKey(pt => pt.TagID);
        
        // Self referencing category relationship
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryID)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany() // Assuming Product can have multiple CartItems
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete, if needed
    }

}
}
