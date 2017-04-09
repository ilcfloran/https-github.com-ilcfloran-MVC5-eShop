using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyEShop.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Filter(List<int> filterItemschk, decimal maxPrice = 100000, int CategoryId = 0, decimal minPrice = 5, int page = 1)
        {
            List<Product> productsVM = new List<Product>();

            var _minPrice = minPrice >= 5 ? minPrice : 5;
            var _maxPrice = maxPrice >= 10 ? maxPrice : 100000;


            if (CategoryId == 0)
            {
                return RedirectToAction("Index");
            }

            var products = db.Products.Include("FilterItems").Where(p => p.CategoryId == CategoryId);

            if (filterItemschk != null)
            {
                if (filterItemschk.Count() > 0)
                {
                    var filterItems = db.FilterItems.Where(f => filterItemschk.Contains(f.Id));
                    foreach (var item in products)
                    {
                        if (item.FilterItems.Intersect(filterItems).Count() == filterItems.Count())
                        {
                            productsVM.Add(item);
                        }
                    }
                }
            }
            else
            {
                productsVM.AddRange(products);
            }

            if (_minPrice >= 5 && _maxPrice >= 10)
            {
                var q = productsVM.Where(p => p.Price >= _minPrice && p.Price <= _maxPrice).ToList();

                productsVM.Clear();
                productsVM.AddRange(q);
            }

            int take = 1;
            var count = productsVM.Count();

            int skip = 0;
            if (count > take)
            {
                skip = (take * page) - take;
            }


            var TotalPages = Math.Ceiling((decimal)(count / take));
            ViewBag.TotalPages = TotalPages;

            var _page = page <= TotalPages ? page : TotalPages;

            ViewBag.Page = _page;
            ViewBag.CategoryId = CategoryId;

            return PartialView("_ProductItems", productsVM.OrderBy(p => p.Id).Skip(skip).Take(take));
        }

        public ActionResult CategoryItems(int id)
        {
            ViewBag.CategoryId = id;
            return View();

        }

        //public ActionResult ProductItems(int id)
        //{
        //    IEnumerable<Product> items = db.Products.Where(p => p.CategoryId == id).ToList();
        //    ViewBag.CategoryId = id;

        //    if (items == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return PartialView("_ProductItems", items);

        //}

        public ActionResult Create()
        {
            return View();
        }


        public ActionResult Search(string searchText, int searchPage = 1)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                RedirectToAction("Index", "Home");
            }

            List<Product> productsVM = new List<Product>();

            productsVM = db.Products.Where(p => p.Name.Contains(searchText) || p.Text.Contains(searchText)).ToList();

            int take = 1;
            var count = productsVM.Count();

            int skip = 0;
            if (count > take)
            {
                skip = (take * searchPage) - take;
            }

            var TotalPages = Math.Ceiling((decimal)(count / take));
            ViewBag.TotalPages = TotalPages;
            var _page = searchPage <= TotalPages ? searchPage : TotalPages;
            ViewBag.SearchPage = _page;
            ViewBag.SearchTerm = searchText;

            return View(productsVM.OrderBy(p => p.Id).Skip(skip).Take(take));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserId,Name,Text,Description,Image,Price,Visit,date,EndDate,weight,Onsale,OnSalePrice,CategoryId,Count")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,Name,Text,Description,Image,Price,Visit,date,EndDate,weight,Onsale,OnSalePrice,CategoryId,Count")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
