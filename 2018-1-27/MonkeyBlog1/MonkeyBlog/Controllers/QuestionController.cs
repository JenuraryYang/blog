using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonkeyBlog.Controllers
{
    public class QuestionController : Controller
    {
        //
        // GET: /Question/

        public ActionResult QuestionIndex()
        {
            return View();
        }
        public ActionResult Questions()
        {
            return View();
        }
    }
}
