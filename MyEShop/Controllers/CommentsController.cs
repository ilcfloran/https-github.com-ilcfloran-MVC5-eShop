using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;
using MyEShop.Web.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TreeUtility;

namespace MyEShop.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public async Task<ActionResult> Index()
        {
            var comments = db.Comments.Include(c => c.Parent).Include(c => c.User);
            return View(await comments.ToListAsync());
        }

        private List<CommentVM> GetListOfNodes(List<Comment> cmnt)
        {
            List<Comment> allComments = cmnt;

            List<CommentVM> comments = new List<CommentVM>();
            foreach (Comment sourceComment in allComments)
            {
                CommentVM c = new CommentVM();
                c.Id = sourceComment.Id;
                c.Text = sourceComment.Text;
                if (sourceComment.ParentId != null)
                {
                    c.Parent = new CommentVM();
                    c.Parent.Id = (int)sourceComment.ParentId;
                }
                c.User = sourceComment.User;
                c.ProductId = sourceComment.ProductId;
                c.date = sourceComment.date;
                comments.Add(c);
            }
            return comments;
        }

        public ActionResult CommentsByProduct(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var retrievedComments = db.Comments.Include("User").Where(c => c.ProductId == id).ToList();

            IEnumerable<CommentVM> listOfNodes = GetListOfNodes(retrievedComments);
            IList<CommentVM> comments = TreeHelper.ConvertToForest(listOfNodes);

            ViewBag.ProductId = id;
            return PartialView("_Comments", comments);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create(string message, int? commentId)
        {
            ViewBag.ParentId = new SelectList(db.Comments, "Id", "UserId");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserId,Email,Text,date,Confirmed,ProductId,ParentId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.Comments, "Id", "UserId", comment.ParentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.Comments, "Id", "UserId", comment.ParentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", comment.UserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,Email,Text,date,Confirmed,ProductId,ParentId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = new SelectList(db.Comments, "Id", "UserId", comment.ParentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Comment comment = await db.Comments.FindAsync(id);
            db.Comments.Remove(comment);
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
