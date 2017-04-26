using MyEShop.DataAccess.ModelConfigs;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyEShop.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var products = db.Products.Where(p => p.EndDate == null || p.EndDate > DateTime.Now).Take(6).ToList();

            return View(products);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}