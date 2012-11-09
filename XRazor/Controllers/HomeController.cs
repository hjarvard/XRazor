using sdf.XPath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XRazor.Models;

namespace XRazor.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
			var model = new IndexPageViewModel();
			model.Greeting = "Hello world!";

            return View(model);
        }

    }
}
