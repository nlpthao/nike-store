using Microsoft.AspNetCore.Mvc;
using NikeStyle.Models;
using System.Collections.Generic;

namespace NikeStyle.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<Product>
            {
                new Product {Id=1, Name = "Retro Runners", ImageUrl = "/images/retro-runner.jpg",Description = "Just In", Category = "Shoes", Price = 99.99M},
                new Product {Id=2,Name = "Sport Meets Style", ImageUrl = "/images/sport-meet-style.jpg", Description = "Give Sport", Category = "Clothing", Price = 79.99M}
            };
            var banners = new List<Banner>
            {
                new Banner {BannerId=1, BannerName = "Retro Runners", ImageUrl = "/images/retro-runner.png", Description="Just in"},
                new Banner {BannerId=2,BannerName = "Sport Meets Style", ImageUrl = "/images/sport-meet-style.jpg",Description="Give Sport"},
                new Banner {BannerId=3,BannerName = "Cold Weather Running", ImageUrl = "/images/cold-weather-running.jpg",Description="Run in the Rain"},
                new Banner {BannerId=4,BannerName = "Women Nike Fleece", ImageUrl = "/images/women-nike-fleece.png",Description="Comfortable everywhere"}
            };

            ViewBag.Products = products;
            ViewBag.Banners = banners;
            return View();

        }
    }
}
