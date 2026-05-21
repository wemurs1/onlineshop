using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Database;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly OnlineShopContext _context;

        public ProductsController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Description,FullDesc,Price,Discount,ImageName,Gallery,Qty,Tags,VideoUrl")] Product product,
            IFormFile? MainImage,
            IFormFile[]? GalleryImages)
        {
            if (ModelState.IsValid)
            {
                if (MainImage != null)
                {
                    product.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(MainImage.FileName);
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners", product.ImageName);
                    using var stream = new FileStream(imagePath, FileMode.Create);
                    MainImage.CopyTo(stream);
                }

                if (GalleryImages != null)
                {
                    foreach (var image in GalleryImages)
                    {
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners", imageName);
                        using var stream = new FileStream(imagePath, FileMode.Create);
                        image.CopyTo(stream);
                        product.Gallery.Add(new ProductGallery { ImageName = imageName });
                    }
                }
                _context.Products.Add(product);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(x => x.Gallery).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,FullDesc,Price,Discount,ImageName,Qty,Tags,VideoUrl,Gallery")] Product product,
            IFormFile? MainImage,
            IFormFile[]? GalleryImages)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (MainImage != null)
                    {
                        string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners");
                        string path = string.Empty;

                        if (!string.IsNullOrEmpty(product.ImageName))
                        {
                            path = Path.Combine(rootPath, product.ImageName);
                            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                        }
                        else
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImage.FileName);
                            path = Path.Combine(rootPath, fileName);
                            product.ImageName = fileName;
                        }

                        using var stream = new FileStream(path, FileMode.Create);
                        MainImage.CopyTo(stream);
                    }

                    if (GalleryImages != null)
                    {
                        foreach (var galleryImage in GalleryImages)
                        {
                            var imageName = Guid.NewGuid() + Path.GetExtension(galleryImage.FileName);
                            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners", imageName);
                            using var stream = new FileStream(path, FileMode.Create);
                            galleryImage.CopyTo(stream);

                            product.Gallery.Add(new ProductGallery { ImageName = imageName });
                        }
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(g => g.Gallery).FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.Include(g => g.Gallery).FirstOrDefaultAsync(x => x.Id == id);
            if (product != null)
            {
                string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners");
                if (!string.IsNullOrEmpty(product.ImageName))
                {
                    var mainImage = Path.Combine(rootPath, product.ImageName);
                    if (System.IO.File.Exists(mainImage)) System.IO.File.Delete(mainImage);
                }
                if (product.Gallery != null && product.Gallery.Count != 0)
                {
                    foreach (var galleryImage in product.Gallery)
                    {
                        if (!string.IsNullOrEmpty(galleryImage.ImageName))
                        {
                            var image = Path.Combine(rootPath, galleryImage.ImageName);
                            if (System.IO.File.Exists(image)) System.IO.File.Delete(image);
                        }
                    }
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteGallery(int id)
        {
            var gallery = await _context.ProductGallery.FindAsync(id);
            if (gallery == null) return NotFound();

            if (!string.IsNullOrEmpty(gallery.ImageName))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners", gallery.ImageName);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }

            _context.Remove(gallery);
            await _context.SaveChangesAsync();

            return Redirect("edit/" + gallery.ProductId);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
