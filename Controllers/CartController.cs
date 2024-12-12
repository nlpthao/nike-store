using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NikeStyle.Models;
using NikeStyle.Services.interfaces;
using NikeStyle.Data;

namespace NikeStyle.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ProductContext _context;

    public CartController(ICartService cartService,ProductContext context)
    {
        _cartService = cartService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var cartItems = await _cartService.GetCartItemsAsync();
        var totalAmount = await _cartService.GetTotalAmountAsync();

        var model = new CartViewModel
        {
            CartItems = cartItems, 
            TotalAmount = totalAmount
        };

        return View(model);
    }

    public async Task<IActionResult> Remove(int cartItemId)
    {
        await _cartService.RemoveFromCartAsync(cartItemId);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Clear()
    {
        await _cartService.ClearCartAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        await _cartService.AddToCartAsync(productId, quantity);
        return RedirectToAction("Index");
    }
}
