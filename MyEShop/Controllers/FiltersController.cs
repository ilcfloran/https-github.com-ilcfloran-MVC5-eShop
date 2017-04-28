using MyEShop.Core.Models;
using MyEShop.DataAccess.ModelConfigs;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TreeUtility;

namespace MyEShop.Controllers
{
    public class FiltersController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        private List<GroupFilter> GetListOfNodes(List<GroupFilter> cmnt)
        {
            List<GroupFilter> allComments = cmnt;

            List<GroupFilter> comments = new List<GroupFilter>();
            foreach (GroupFilter sourceComment in allComments)
            {
                GroupFilter c = new GroupFilter();
                c.Id = sourceComment.Id;
                c.Title = sourceComment.Title;
                if (sourceComment.ParentId != null)
                {
                    c.Parent = new GroupFilter();
                    c.Parent.Id = (int)sourceComment.ParentId;
                }
                c.CategoriesGroupFilters = sourceComment.CategoriesGroupFilters;
                c.FilterItems = new List<FilterItem>();
                foreach (var item in sourceComment.FilterItems)
                {
                    c.FilterItems.Add(item);
                }
                c.FilterItems = sourceComment.FilterItems;
                comments.Add(c);
            }
            return comments;
        }

        public ActionResult FiltersByCategory(int categoryId)
        {

            var groupFilters = from c in db.CategoriesGroupFilters
                               join g in db.GroupFilter on c.GroupFilterId equals g.ParentId
                               join f in db.FilterItems on g.Id equals f.GroupFilterId
                               where c.CategoryId == categoryId
                               select g;

            groupFilters = groupFilters.Distinct();

            //var filters = from cgf in db.CategoriesGroupFilters
            //              join gf in db.GroupFilter on cgf.GroupFilterId equals gf.Id

            //var groupFilters = db.GroupFilter.Join(db.CategoriesGroupFilters, g => g.ParentId, c => c.GroupFilterId, a => new { a.Id, a.FilterItems, a.Title, })
            //    .Where(c => c.CategoryId = categoryId)
            //    .Select(g);
            //from p in product
            //join pc in productcategory on p.Id equals pc.ProdId
            //join c in category on pc.CatId equals c.Id
            //select new
            //{
            //    ProdId = p.Id, // or pc.ProdId
            //    CatId = c.CatId
            //    // other assignments
            //};


            return PartialView("_FiltersByCategory", groupFilters);
        }

        public ActionResult FiltersByCategoryAndProduct(int categoryId, int ProductId)
        {

            var groupFilters = from c in db.CategoriesGroupFilters
                               join g in db.GroupFilter on c.GroupFilterId equals g.ParentId
                               join f in db.FilterItems on g.Id equals f.GroupFilterId
                               where c.CategoryId == categoryId
                               select g;

            groupFilters = groupFilters.Distinct();

            var filterItems = db.Products.Include("FilterItems").Where(p => p.Id == ProductId).Select(p => p.FilterItems).SingleOrDefault();

            ViewBag.Filters = filterItems;

            return PartialView("_FiltersByCategoryAndProduct", groupFilters);
        }

        [HttpGet]
        public ActionResult ManageFilters()
        {

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var groupFilters = db.GroupFilter.Include("FilterItems").ToList();


                    IEnumerable<GroupFilter> listOfNodes = GetListOfNodes(groupFilters);
                    IList<GroupFilter> gFilters = TreeHelper.ConvertToForest(listOfNodes);

                    foreach (var item in gFilters)
                    {
                        foreach (var x in item.Children)
                        {
                            foreach (var node in listOfNodes)
                            {
                                if (node.Id == x.Id)
                                {
                                    node.FilterItems = x.FilterItems;
                                }
                            }
                        }
                    }

                    //ViewBag.ProductId = id;
                    return View(gFilters);
                }
            }

            return RedirectToAction("Index", "Manage");
        }


        [HttpGet]
        public ActionResult EditFilters(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var filter = db.FilterItems.Where(f => f.Id == id).SingleOrDefault();
                    if (filter == null)
                    {
                        return RedirectToAction("Index", "Manage");

                    }

                    return View(filter);
                }

            }

            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public ActionResult EditFilters(string Title, int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var filter = db.FilterItems.Where(f => f.Id == Id).SingleOrDefault();
                    if (filter == null)
                    {
                        return RedirectToAction("Index", "Manage");
                    }

                    filter.Title = Title;
                    db.SaveChanges();
                    return RedirectToAction("ManageFilters", "Filters");
                }

            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpGet]
        public ActionResult DeleteFilters(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var filter = db.FilterItems.Where(f => f.Id == id).SingleOrDefault();
                    if (filter == null)
                    {
                        return RedirectToAction("Index", "Manage");

                    }

                    return View(filter);
                }

            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public ActionResult DeleteFilters(int Id, string dummy)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var filter = db.FilterItems.Where(f => f.Id == Id).SingleOrDefault();
                    if (filter == null)
                    {
                        return RedirectToAction("Index", "Manage");
                    }

                    db.FilterItems.Remove(filter);
                    db.SaveChanges();
                    return RedirectToAction("ManageFilters", "Filters");
                }

            }
            return RedirectToAction("Index", "Manage");
        }


        [HttpGet]
        public ActionResult AddFilters(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    ViewBag.GroupFilterId = id;
                    return View();
                }
            }
            return RedirectToAction("Index", "Manage");
        }


        public ActionResult AddFilters(string Title, int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var groupFilter = db.GroupFilter.Where(f => f.Id == id).SingleOrDefault();
                    if (groupFilter == null)
                    {
                        return RedirectToAction("Index", "Manage");
                    }

                    var filter = new FilterItem();
                    filter.Title = Title;
                    filter.GroupFilterId = id;
                    filter.GroupFilter = groupFilter;
                    db.FilterItems.Add(filter);
                    db.SaveChanges();
                    return RedirectToAction("ManageFilters", "Filters");
                }

            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpGet]
        public ActionResult EditFilterGroup(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    ViewBag.GroupFilterId = id;
                    return View();
                }
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public ActionResult EditFilterGroup(string Title, int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var groupFilter = db.GroupFilter.Where(f => f.Id == id).SingleOrDefault();
                    if (groupFilter == null)
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                    groupFilter.Title = Title;
                    db.SaveChanges();
                    return RedirectToAction("ManageFilters", "Filters");
                }
            }
            return RedirectToAction("Index", "Manage");
        }




    }
}






