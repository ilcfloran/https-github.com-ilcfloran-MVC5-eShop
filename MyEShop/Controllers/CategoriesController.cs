using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;
using MyEShop.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TreeUtility;

namespace MyEShop.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private List<CategoryVM> GetListOfNodes()
        {
            List<Category> sourceCategories = db.Categories.ToList();

            List<CategoryVM> categories = new List<CategoryVM>();
            foreach (Category sourceCategory in sourceCategories)
            {
                CategoryVM c = new CategoryVM();
                c.Id = sourceCategory.Id;
                c.CategoryName = sourceCategory.CategoryName;
                if (sourceCategory.ParentCategoryId != null)
                {
                    c.Parent = new CategoryVM();
                    c.Parent.Id = (int)sourceCategory.ParentCategoryId;
                }
                categories.Add(c);
            }
            return categories;
        }


        private string EnumerateNodes(ICategory parent)
        {
            // Init an empty string
            string content = String.Empty;

            // Add <li> category name
            content += "<li class=\"treenode\">";
            content += parent.CategoryName;

            if (parent.Children.Count == 0)
                content += "</li>";
            else   // If there are children, start a <ul>
                content += "<ul>";

            // Loop one past the number of children
            int numberOfChildren = parent.Children.Count;
            for (int i = 0; i <= numberOfChildren; i++)
            {
                // If this iteration's index points to a child,
                // call this function recursively
                if (numberOfChildren > 0 && i < numberOfChildren)
                {
                    ICategory child = parent.Children[i];
                    content += EnumerateNodes(child);
                }

                // If this iteration's index points past the children, end the </ul>
                if (numberOfChildren > 0 && i == numberOfChildren)
                    content += "</ul>";
            }

            // Return the content
            return content;
        }


        // GET: Categories
        public ActionResult Index()
        {

            IEnumerable<CategoryVM> listOfNodes = GetListOfNodes();
            IList<CategoryVM> topLevelCategories = TreeHelper.ConvertToForest(listOfNodes);

            return PartialView("_CategoryProducts", topLevelCategories);
        }

        [HttpGet]
        public ActionResult FetchCategories()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    IEnumerable<CategoryVM> listOfNodes = GetListOfNodes();
                    IList<CategoryVM> topLevelCategories = TreeHelper.ConvertToForest(listOfNodes);

                    return View(topLevelCategories);
                }

            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var cat = db.Categories.Where(c => c.Id == id).SingleOrDefault();
                    var groupFilters = db.GroupFilter.ToList();
                    if (cat != null)
                    {
                        //ViewBag.GroupFilters = groupFilters;
                        return View(cat);
                    }
                }
                return RedirectToAction("Index", "Manage");
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public ActionResult EditCategory(Category cat, List<int?> groupFilters)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var category = db.Categories.Where(c => c.Id == cat.Id).SingleOrDefault();
                    if (category != null)
                    {
                        category.CategoryName = cat.CategoryName;


                        var catGroupFilter = db.CategoriesGroupFilters.Where(c => c.CategoryId == cat.Id).ToList();
                        if (catGroupFilter != null)
                        {
                            List<CategoriesGroupFilters> lst = new List<CategoriesGroupFilters>();
                            foreach (var item in catGroupFilter)
                            {
                                lst.Add(item);
                            }
                            db.CategoriesGroupFilters.RemoveRange(lst);
                            db.SaveChanges();
                        }


                        if (groupFilters != null)
                        {
                            foreach (var item in groupFilters)
                            {
                                var catg = new CategoriesGroupFilters();
                                catg.CategoryId = cat.Id;
                                catg.GroupFilterId = item ?? 0;
                                db.CategoriesGroupFilters.Add(catg);
                            }
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("FetchCategories", "Categories");
                }
                return RedirectToAction("Index", "Manage");
            }
            return RedirectToAction("Index", "Manage");
        }




        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        public ActionResult Create()
        {
            ViewBag.ParentCategoryId = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ParentCategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ParentCategoryId = new SelectList(db.Categories, "Id", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentCategoryId = new SelectList(db.Categories, "Id", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParentCategoryId,CategoryName")] CategoryVM category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ParentCategoryId = new SelectList(db.Categories, "Id", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
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
