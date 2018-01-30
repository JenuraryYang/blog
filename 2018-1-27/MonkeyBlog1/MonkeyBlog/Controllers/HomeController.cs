using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MonkeyBlog.Filters;

namespace MonkeyBlog.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
     
       
        public ActionResult Index()
        {
            MonkeyDBEntities be = new MonkeyDBEntities();
            List<Carousel_figure> clist = be.Carousel_figure.ToList();

            //退出时  清空session信息
            if (Request["way"] == "logOut")
            {
                Session.Contents.Clear();
            }



            ViewBag.clist = clist;


            return View();
        }
        public ActionResult test()
        {
            return View();
        }

    }
}
