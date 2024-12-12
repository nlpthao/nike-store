using Microsoft.AspNetCore.Mvc;
using NikeStyle.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NikeStyle.Models;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc.Rendering;
using NikeStyle.Services.interfaces;
using NikeStyle.Services;
namespace NikeStyle.Controllers {
    public class ProductController : Controller
    {
        private readonly ProductContext _context;
        private const int PageSize = 3;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICartService _cartService;
        public ProductController(ProductContext context, IWebHostEnvironment webHostEnvironment, ICartService cartService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _cartService = cartService;
        }
        public async Task<IActionResult> Index(int page =1, int? categoryId = null){
            var query = _context.Products.Include(p => p.Category).AsQueryable();
            if (categoryId.HasValue)
            {
                // retrieve all categories that are subcategories 
                var categoryIds = await _context.Categories
                    .Where(c => c.CategoryID == categoryId || c.ParentCategoryID == categoryId)
                    .Select(c=> c.CategoryID)
                    .ToListAsync();
                
                query = query.Where(p=> categoryIds.Contains(p.CategoryID));
            }
            var totalItems = await query.CountAsync();
            var products = await query
                .OrderBy(p=>p.Name)
                .Skip((page-1)*PageSize)
                .Take(PageSize)
                .ToListAsync();
            
            var model = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems/(double)PageSize),
                Categories = await _context.Categories.ToListAsync(),
                SelectedCategoryId = categoryId
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Add()
        {
            // Fetch all categories
            var categories = _context.Categories.ToList();

            // Prepare SelectList with hierarchy display
            var categorySelectList = new List<SelectListItem>();

            foreach (var category in categories.Where(c => c.ParentCategoryID == null))
            {
                // Add the parent category
                categorySelectList.Add(new SelectListItem
                {
                    Value = category.CategoryID.ToString(),
                    Text = category.Name
                });

                // Add each subcategory under this parent category with indentation
                foreach (var subCategory in categories.Where(c => c.ParentCategoryID == category.CategoryID))
                {
                    categorySelectList.Add(new SelectListItem
                    {
                        Value = subCategory.CategoryID.ToString(),
                        Text = $"-- {subCategory.Name}"
                    });
                }
            }

            ViewBag.Categories = categorySelectList;
            return View(new Product());
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(_context.Categories,"CategoryID","Name",product.CategoryID);
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            if (!ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    var extension = Path.GetExtension(model.ImageFile.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    model.ImageUrl = "/images/products/" + Path.GetFileName(filePath);
                }

                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category) // Make sure to include the Category
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(); // Return a 404 page if the product isn't found
            }

            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // ADD TO CART
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            await _cartService.AddToCartAsync(productId, quantity);
            return RedirectToAction("Index", "Cart");
        }





    }
}