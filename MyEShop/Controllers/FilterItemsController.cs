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

namespace MyEShop.Controllers
{
    public class FilterItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        private List<FilterItemVM> GetListOfNodes()
        {
            List<FilterItem> sourcefilterItems = db.FilterItems.ToList();

            List<FilterItemVM> filterItems = new List<FilterItemVM>();
            foreach (FilterItem sourceFilterItems in sourcefilterItems)
            {
                FilterItemVM c = new FilterItemVM();
                c.Id = sourceFilterItems.Id;
                c.Title = sourceFilterItems.Title;
                if (sourceFilterItems.ParentFilterItemId != null)
                {
                    c.Parent = new FilterItemVM();
                    c.Parent.Id = (int)sourceFilterItems.ParentFilterItemId;
                }
                filterItems.Add(c);
            }
            return filterItems;
        }


        private string EnumerateNodes(IFilterItem parent)
        {
            // Init an empty string
            string content = String.Empty;

            // Add <li> category name
            content += "<li class=\"treenode\">";
            content += parent.Title;

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
                    IFilterItem child = parent.Children[i];
                    content += EnumerateNodes(child);
                }

                // If this iteration's index points past the children, end the </ul>
                if (numberOfChildren > 0 && i == numberOfChildren)
                    content += "</ul>";
            }

            // Return the content
            return content;
        }




        public async Task<ActionResult> Index()
        {
            return View(await db.FilterItems.ToListAsync());
        }



        public ActionResult FiltersByCategory()
        {
            IEnumerable<FilterItemVM> listOfNodes = GetListOfNodes();
            IList<FilterItemVM> filterItems = TreeHelper.ConvertToForest(listOfNodes);

            return View("_FiltersByCategory", filterItems);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilterItem filterItem = await db.FilterItems.FindAsync(id);
            if (filterItem == null)
            {
                return HttpNotFound();
            }
            return View(filterItem);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,GroupFilterId")] FilterItem filterItem)
        {
            if (ModelState.IsValid)
            {
                db.FilterItems.Add(filterItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(filterItem);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilterItem filterItem = await db.FilterItems.FindAsync(id);
            if (filterItem == null)
            {
                return HttpNotFound();
            }
            return View(filterItem);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,GroupFilterId")] FilterItem filterItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filterItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(filterItem);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilterItem filterItem = await db.FilterItems.FindAsync(id);
            if (filterItem == null)
            {
                return HttpNotFound();
            }
            return View(filterItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FilterItem filterItem = await db.FilterItems.FindAsync(id);
            db.FilterItems.Remove(filterItem);
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
