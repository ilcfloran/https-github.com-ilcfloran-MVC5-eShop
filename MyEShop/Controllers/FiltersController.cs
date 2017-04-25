using MyEShop.DataAccess.ModelConfigs;
using System.Linq;
using System.Web.Mvc;

namespace MyEShop.Controllers
{
    public class FiltersController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
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


    }
}