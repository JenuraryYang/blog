using MonkeyBlog.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonkeyBlog.Controllers
{
    public class BlogManagerController : Controller
    {
        //
        // GET: /BlogManager/

        /// <summary>
        /// 发布的文章
        /// </summary>
        /// <returns></returns>
        public ActionResult PublishBlogManagerIndex(string searchcontent)
        {
            MonkeyDBEntities db = new MonkeyDBEntities();
            int rows;
            //如果为""就赋值为1
            if (Request["num"] == "")
            {
                rows = 1;
            }
            //如果为null就赋值为1
            rows = Convert.ToInt32(Request["num"] ?? "1");

            int Write_ArticleType;
            if (Request["Write_ArticleType"] == "")//判断得到的文章类型是否为“”
            {
                Write_ArticleType = -1;
            }

            Write_ArticleType = Convert.ToInt32(Request["Write_ArticleType"] ?? "-1");

            //得到博主信息
            //ULogin ul= Session["ULogin"] as ULogin;
            ULogin ul = new ULogin() { ULogin_Id = 1 };

            //创建一个返回值为bool型的委托
            Func<Write_blog, bool> getpage = a => a.Write_IsDel == 2 && a.ULogin_Id == ul.ULogin_Id && a.ULogin_IdIsManager == "否" && (Write_ArticleType == -1 || a.Write_ArticleType == Write_ArticleType) && (string.IsNullOrEmpty(searchcontent) || a.Write_Content.Contains(searchcontent.Trim()) || a.Write_Title.Contains(searchcontent.Trim()) || a.Write_Label.Contains(searchcontent.Trim()));

            //创建以及博客集合
            List<Write_blog> wblist = new List<Write_blog>();

            wblist = db.Write_blog.Where(getpage).OrderByDescending(a => a.Write_blog_Id).Skip((rows - 1) * 5).Take(5).ToList();

            //创建一个装评论条数的集合
            List<int> bccountlist = new List<int>();

            //查询所有的评论
            List<Blog_Comment> bclist = db.Blog_Comment.AsNoTracking().ToList();
            //遍历博客
            foreach (var item in wblist)
            {
                //查询该博客对应的评论的条数
                int count = bclist.Where(a => a.BC_ArticleID == item.Write_blog_Id).Count();
                //添加到集合
                bccountlist.Add(count);
            }

            //得到所有条数
            int allcount = db.Write_blog.Where(getpage).Count();
            //得到所有页数
            int allpage = allcount / 5;
            //判断是否除尽或页数是否为0
            if (allcount % 5 != 0 || allpage == 0)
            {
                allpage = allpage + 1;
            }

            //得到审核未通过的信息集合
            var query = db.Auditing_Blog.Where(a => a.Auditing_Blog_State == 1);

            Dictionary<int, string> dicaud = new Dictionary<int, string>();

            //遍历文章
            foreach (var item in wblist)
            {
                if (item.Write_State == 1)
                {
                    //查询未通过的文章的审核信息
                   Auditing_Blog audb=  query.Where(a => a.Write_blog_Id == item.Write_blog_Id).FirstOrDefault();
                    //以键值对的形式把未通过审核的文章存在集合
                   dicaud[item.Write_blog_Id] = audb.Auditing_Blog_Msg;
                }
            }


            //传值到页面
            ViewBag.allpage = allpage;//总页数
            ViewBag.rows = rows;//第几页
            ViewBag.bccountlist = bccountlist;//评论条数
            ViewBag.wblist = wblist;//博客集合
            ViewBag.searchcontent = searchcontent;//搜索框
            ViewBag.Write_ArticleType = Write_ArticleType;//文章类型
            ViewBag.dicaud = dicaud;//审核未通过的审核意见
            return View();
        }


        /// <summary>
        /// 保存的文章（草稿箱）
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveBlogManagerIndex(string searchcontent)
        {

            MonkeyDBEntities db = new MonkeyDBEntities();
            int rows;
            //如果为""就赋值为1
            if (Request["num"] == "")
            {
                rows = 1;
            }
            //如果为null就赋值为1
            rows = Convert.ToInt32(Request["num"] ?? "1");

            int Write_ArticleType;
            if (Request["Write_ArticleType"] == "")//判断得到的文章类型是否为“”
            {
                Write_ArticleType = -1;
            }

            Write_ArticleType = Convert.ToInt32(Request["Write_ArticleType"] ?? "-1");

            //得到博主信息
            //ULogin ul= Session["ULogin"] as ULogin;
            ULogin ul = new ULogin() { ULogin_Id = 1 };

            //创建一个返回值为bool型的委托
            Func<Write_blog, bool> getpage = a => a.Write_IsDel == 1 && a.ULogin_Id == ul.ULogin_Id && a.ULogin_IdIsManager == "否" && (Write_ArticleType == -1 || a.Write_ArticleType == Write_ArticleType) && (string.IsNullOrEmpty(searchcontent) || a.Write_Content.Contains(searchcontent.Trim()) || a.Write_Title.Contains(searchcontent.Trim()) || a.Write_Label.Contains(searchcontent.Trim()));

            //创建以及博客集合
            List<Write_blog> wblist = new List<Write_blog>();

            wblist = db.Write_blog.Where(getpage).OrderByDescending(a => a.Write_blog_Id).Skip((rows - 1) * 5).Take(5).ToList();

            //创建一个装评论条数的集合
            List<int> bccountlist = new List<int>();

            //查询所有的评论
            List<Blog_Comment> bclist = db.Blog_Comment.AsNoTracking().ToList();
            //遍历博客
            foreach (var item in wblist)
            {
                //查询该博客对应的评论的条数
                int count = bclist.Where(a => a.BC_ArticleID == item.Write_blog_Id).Count();
                //添加到集合
                bccountlist.Add(count);
            }

            //得到所有条数
            int allcount = db.Write_blog.Where(getpage).Count();
            //得到所有页数
            int allpage = allcount / 5;
            //判断是否除尽或页数是否为0
            if (allcount % 5 != 0 || allpage == 0)
            {
                allpage = allpage + 1;
            }


            //传值到页面
            ViewBag.allpage = allpage;//总页数
            ViewBag.rows = rows;//第几页
            ViewBag.bccountlist = bccountlist;//评论条数
            ViewBag.wblist = wblist;//博客集合
            ViewBag.searchcontent = searchcontent;//搜索框
            ViewBag.Write_ArticleType = Write_ArticleType;//文章类型
            return View();
        }

        /// <summary>
        /// 删除的文章（回收站）
        /// </summary>
        /// <returns></returns>
        public ActionResult DelBlogManagerIndex(string searchcontent)
        {

            MonkeyDBEntities db = new MonkeyDBEntities();
            int rows;
            //如果为""就赋值为1
            if (Request["num"] == "")
            {
                rows = 1;
            }
            //如果为null就赋值为1
            rows = Convert.ToInt32(Request["num"] ?? "1");

            int Write_ArticleType;
            if (Request["Write_ArticleType"] == "")//判断得到的文章类型是否为“”
            {
                Write_ArticleType = -1;
            }

            Write_ArticleType = Convert.ToInt32(Request["Write_ArticleType"] ?? "-1");

            //得到博主信息
            //ULogin ul= Session["ULogin"] as ULogin;
            ULogin ul = new ULogin() { ULogin_Id = 1 };

            //创建一个返回值为bool型的委托
            Func<Write_blog, bool> getpage = a => a.Write_IsDel == 0 && a.ULogin_Id == ul.ULogin_Id && a.ULogin_IdIsManager == "否" && (Write_ArticleType == -1 || a.Write_ArticleType == Write_ArticleType) && (string.IsNullOrEmpty(searchcontent) || a.Write_Content.Contains(searchcontent.Trim()) || a.Write_Title.Contains(searchcontent.Trim()) || a.Write_Label.Contains(searchcontent.Trim()));

            //创建以及博客集合
            List<Write_blog> wblist = new List<Write_blog>();

            wblist = db.Write_blog.Where(getpage).OrderByDescending(a => a.Write_blog_Id).Skip((rows - 1) * 5).Take(5).ToList();

            //创建一个装评论条数的集合
            List<int> bccountlist = new List<int>();

            //查询所有的评论
            List<Blog_Comment> bclist = db.Blog_Comment.AsNoTracking().ToList();
            //遍历博客
            foreach (var item in wblist)
            {
                //查询该博客对应的评论的条数
                int count = bclist.Where(a => a.BC_ArticleID == item.Write_blog_Id).Count();
                //添加到集合
                bccountlist.Add(count);
            }

            //得到所有条数
            int allcount = db.Write_blog.Where(getpage).Count();
            //得到所有页数
            int allpage = allcount / 5;
            //判断是否除尽或页数是否为0
            if (allcount % 5 != 0 || allpage == 0)
            {
                allpage = allpage + 1;
            }


            //传值到页面
            ViewBag.allpage = allpage;//总页数
            ViewBag.rows = rows;//第几页
            ViewBag.bccountlist = bccountlist;//评论条数
            ViewBag.wblist = wblist;//博客集合
            ViewBag.searchcontent = searchcontent;//搜索框
            ViewBag.Write_ArticleType = Write_ArticleType;//文章类型
            return View();
        }


        /// <summary>
        /// 彻底删除文章
        /// </summary>
        /// <returns></returns>
        public JsonResult RealDelBlogManagerIndex(Write_blog wb)
        {
            MonkeyDBEntities db = new MonkeyDBEntities();

            db.Entry(wb).State = System.Data.EntityState.Deleted; ;//彻底删除博客
        
            //查询该条博客下面的所有评论
            List<Blog_Comment> bclist = db.Blog_Comment.Where(a => a.BC_ArticleID == wb.Write_blog_Id).ToList();

            //遍历该博客下面的所评论
            foreach (var item in bclist)
            {
                //删除评论
                db.Entry(item).State = System.Data.EntityState.Deleted;
            }
            //先关闭验证实体有效性（ValidateOnSaveEnabled）这个开关（默认是开启的）
            db.Configuration.ValidateOnSaveEnabled = false;

            int count = db.SaveChanges();//保存到数据库

            //在开启验证实体有效性（ValidateOnSaveEnabled）这个开关
            db.Configuration.ValidateOnSaveEnabled = true;

            return Json(count,JsonRequestBehavior.AllowGet);
        }
    }
}
