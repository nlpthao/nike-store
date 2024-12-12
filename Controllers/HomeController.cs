using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStyle.Data;
using NikeStyle.Models;
using System.Collections.Generic;

namespace NikeStyle.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ProductContext _context;
        public HomeController(UserManager<IdentityUser> userManager, ProductContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var tags = await _context.Tags.ToListAsync();
            var banners = new List<Banner>
            {
                new Banner {BannerId=1, BannerName = "Retro Runners", ImageUrl = "/images/retro-runner.png", Description="Just in"},
                new Banner {BannerId=2,BannerName = "Sport Meets Style", ImageUrl = "/images/sport-meet-style.jpg",Description="Give Sport"},
                new Banner {BannerId=3,BannerName = "Cold Weather Running", ImageUrl = "/images/cold-weather-running.jpg",Description="Run in the Rain"},
                new Banner {BannerId=4,BannerName = "Women Nike Fleece", ImageUrl = "/images/women-nike-fleece.png",Description="Comfortable everywhere"}
            };

            ViewBag.Products = products ?? new List<Product>();
            ViewBag.Banners = banners ?? new List<Banner>();
            ViewBag.Categories = categories ?? new List<Category>();
            ViewBag.Tags = tags ?? new  List<Tag>();
            
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                // if (user != null)
                // {
                //     ViewData["FirstName"] = user.FirstName;
                // }
            }
            return View();

        }
    }
}
