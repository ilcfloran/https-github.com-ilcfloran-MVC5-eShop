using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;

namespace MyEShop.Controllers
{
    public class FilterItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FilterItems
        public async Task<ActionResult> Index()
        {
            return View(await db.FilterItems.ToListAsync());
        }

        // GET: FilterItems/Details/5
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

        // GET: FilterItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FilterItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: FilterItems/Edit/5
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

        // POST: FilterItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: FilterItems/Delete/5
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

        // POST: FilterItems/Delete/5
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
