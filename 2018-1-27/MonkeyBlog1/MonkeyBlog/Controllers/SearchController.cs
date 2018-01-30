using MonkeyBlog.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonkeyBlog.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Search()
        {
            return View();
        }


        /// <summary>
        /// 查询文章
        /// </summary>
        /// <param name="_is">是否是自己，大于0为自己，等于0不是自己</param>
        /// <param name="_val">查询内容</param>
        /// <returns>查询结果</returns>
        public JsonResult SearchAll(int _is, string _val)
        {
            List<Write_blog> lis = new List<Write_blog>();

            var _sess = Session["ULogin"];

            var _ulogin = _sess as ULogin;

            if (_sess != null && _is > 0)
            {
                //查询自己的文章
                List<Write_blog> _blis = DTO_Write_blog.Select(_ulogin.ULogin_Id, 1, _val);
            }
            else if (_sess != null && _is == 0)
            {
                //查询指定用户的文章
                List<Write_blog> _blis = DTO_Write_blog.Select(_ulogin.ULogin_Id, 0, _val);
            }
            else
            {
                //查询所有用户的文章
                List<Write_blog> _blis = DTO_Write_blog.Select(0, -1, _val);
            }


            return Json(lis, JsonRequestBehavior.AllowGet);
        }
    }
}
