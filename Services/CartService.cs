using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NikeStyle.Data;
using NikeStyle.Models;
using NikeStyle.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using NikeStyle.Models;

namespace NikeStyle.Services
{
    public class CartService : ICartService
    {
        private readonly ProductContext _context;

        public CartService(ProductContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(int productId, int quantity)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem { ProductId = productId, Quantity = quantity };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync()
        {
            var cartItems = await _context.CartItems.ToListAsync();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalAmountAsync()
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .SumAsync(ci => ci.Quantity * ci.Product.Price.GetValueOrDefault());

        }
    }
}
