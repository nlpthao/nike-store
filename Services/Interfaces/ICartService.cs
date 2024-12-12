using NikeStyle.Models;
namespace NikeStyle.Services.interfaces;
public interface ICartService
{
    Task AddToCartAsync(int productId, int quantity);
    Task RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync();
    Task<List<CartItem>> GetCartItemsAsync();
    Task<decimal> GetTotalAmountAsync();
}
