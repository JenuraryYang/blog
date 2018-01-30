using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using MonkeyBlog.DTO;
using MonkeyBlog.Filters;

namespace MonkeyBlog.Controllers
{
    public class PersonalBlogController : Controller
    {
        //
        // GET: /PersonalBlog/

        /// <summary>
        /// 个人博客页面
        /// </summary>
        /// <param name="searchcontent"></param>
        /// <returns></returns>
        public ActionResult PersonalBlogIndex(string searchcontent)
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
            if (Request["Write_ArticleType"] == "")
            {
                Write_ArticleType = -1;
            }

            Write_ArticleType = Convert.ToInt32(Request["Write_ArticleType"] ?? "-1");


            ULogin ul2 = Session["ULogin"] as ULogin;//查看人的人id

            ULogin ul = new ULogin { ULogin_Id = 1 };//该博客的id          


            //创建一个返回值为bool型的委托
            Func<Write_blog, bool> getpage = a => a.Write_IsDel == 2 && a.Write_State == 2&&a.ULogin_IdIsManager=="否" && (Write_ArticleType == -1 || a.Write_ArticleType == Write_ArticleType) && (string.IsNullOrEmpty(searchcontent) || a.Write_Content.Contains(searchcontent.Trim()) || a.Write_Title.Contains(searchcontent.Trim()) || a.Write_Label.Contains(searchcontent.Trim()));

            //创建以及博客集合
            List<Write_blog> wblist = new List<Write_blog>();

            //查询博主的所有文章
            List<Write_blog> allwblist = db.Write_blog.AsNoTracking().Where(a => a.ULogin_Id == ul.ULogin_Id).ToList();


            //判断是否为本人看的
            if (ul2 == null || ul.ULogin_Id != ul2.ULogin_Id)
            {
                //不是本人查看时（不能查看私有的博客）
                //getpage = a => a.Write_IsDel == 2 && a.Write_State == 2 && a.Write_Private == 1 && (Write_ArticleType == -1 || a.Write_ArticleType == Write_ArticleType) && (string.IsNullOrEmpty(searchcontent.Trim()) || a.Write_Content.Contains(searchcontent) || a.Write_Title.Contains(searchcontent.Trim()) || a.Write_Label.Contains(searchcontent.Trim()));

                wblist = allwblist.Where(getpage).Where(a => a.Write_Private == 1).OrderByDescending(a => a.Write_blog_Id).Skip((rows - 1) * 5).Take(5).ToList();

            }
            else
            {
                //本人查看时
                wblist = allwblist.Where(getpage).OrderByDescending(a => a.Write_blog_Id).Skip((rows - 1) * 5).Take(5).ToList();
            }

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
            int allcount = db.Write_blog.Where(getpage).ToList().Count();
            //得到所有页数
            int allpage = allcount / 5;
            //判断是否除尽或页数是否为0
            if (allcount % 5 != 0 || allpage == 0)
            {
                allpage = allpage + 1;
            }

            //查询所有的个人分类
            List<Category> calist = DTO_Category.SelectCategory(ul.ULogin_Id);

            //查询所有的博客里面的分类
            List<string> caidlist = allwblist.Select(a => a.Category_Id).ToList();

            //定义以个人分类编号作为键，个人分类下面的博客数作为值的数组
            // int[] cateidcount = new int[calist.Count()];

            Dictionary<int, int> dic_cateidcount = new Dictionary<int, int>();

            //定义一个装所有博客文章里面个人分类的集合
            List<int> listid = new List<int>();

            //遍历所有的博客里面的分类
            foreach (var item in caidlist)
            {
                //分割字符串
                string[] id = item.Split(',');
                //遍历数组
                for (int i = 0; i < id.Length; i++)
                {
                    //添加到集合
                    listid.Add(Convert.ToInt32(id[i]));
                }
            }


            //遍历个人分类
            foreach (var item in calist)
            {
                //根据跟人分类的编号查询博客的文章数
                int count2 = listid.Where(a => a == item.Category_Id).Count();
                //以键值对的形式添加到集合
                dic_cateidcount[item.Category_Id] = count2;
            }

            //传值到页面
            ViewBag.allpage = allpage;
            ViewBag.rows = rows;
            ViewBag.bccountlist = bccountlist;
            ViewBag.wblist = wblist;
            ViewBag.searchcontent = searchcontent;
            ViewBag.Write_ArticleType = Write_ArticleType;
            ViewBag.cateidcount = dic_cateidcount;
            ViewBag.calist = calist;
            ViewBag.LoginUser = ul2;//登陆的账号
            ViewBag.WatchUser = ul;//看博客的账号
            return View();
        }


        /// <summary>
        /// 别人评论我的页面
        /// </summary>
        /// <param name="searchcontent"></param>
        /// <returns></returns>
      
        public ActionResult PeraonalCommentIndex()
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

            ULogin ul = Session["ULogin"] as ULogin;
     
            //评论该博主的信息
            List<Blog_Comment> bcomlist = db.Blog_Comment.AsNoTracking().Where(a => a.BC_BlogID == ul.ULogin_Id && a.BC_ForUserID == ul.ULogin_Id).ToList();

            //三表联查
            var query = from b in bcomlist
                        join wb in db.Write_blog on b.BC_ArticleID equals wb.Write_blog_Id
                        join u in db.ULogin on b.BC_UserID equals u.ULogin_Id
                        select new BC_CommentDTO
                        {
                            BC_CommentId = b.BC_CommentId,
                            BC_ArticleID = b.BC_ArticleID,
                            BC_Comment = b.BC_Comment,
                            BC_Comment_Time = b.BC_Comment_Time,
                            Write_Title = wb.Write_Title,
                            Ulogin_BlogName = u.Ulogin_BlogName

                        };

            //分页
            List<BC_CommentDTO> bcdlist = query.OrderByDescending(a=>a.BC_CommentId).Skip((rows-1)*5).Take(5).ToList();

            //得到所有条数
            int allcount = query.Count();
            //得到所有页数
            int allpage = allcount / 5;
            //判断是否除尽或页数是否为0
            if (allcount % 5 != 0 || allpage == 0)
            {
                allpage = allpage + 1;
            }

            ViewBag.bcdlist = bcdlist;
            ViewBag.allpage = allpage;
            ViewBag.rows = rows;
            return View();
        }


        /// <summary>
        /// 判断该评论是否有二级的方法
        /// </summary>
        /// <param name="BC_CommentId"></param>
        /// <returns></returns>
        public JsonResult ChildrenComment(int BC_CommentId)
        {
            MonkeyDBEntities db = new MonkeyDBEntities();

            //查询该条评论的所有二级评论条数
            int  count = db.Blog_Comment.AsNoTracking().Where(a => a.BC_Comment_ParentId == BC_CommentId).Count();
   
            return Json(count,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="bc">该条评论的信息</param>
        /// <returns></returns>
              [IsLoginFilter]
        public JsonResult DelPeraonalComment(Blog_Comment bc)
        {

            MonkeyDBEntities db = new MonkeyDBEntities();

            //查询该篇文章的所有评论
            List<Blog_Comment> querybc = db.Blog_Comment.AsNoTracking().Where(a => a.BC_ArticleID == bc.BC_ArticleID).ToList();
            del(db, querybc, bc);
            
            //操作数据库
            int count = db.SaveChanges();

            return Json(count, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 删除评论的递归算法
        /// </summary>
        /// <param name="db">数据库上下对象</param>
        /// <param name="querybc">该条评论所属文章的所有评论</param>
        /// <param name="bc">该条评论的信息</param>
        public void del(MonkeyDBEntities db, List<Blog_Comment> querybc, Blog_Comment bc)
        {

            //查询该条评论的二级评论
            List<Blog_Comment> bclist1 = querybc.Where(a => a.BC_Comment_ParentId == bc.BC_CommentId).ToList();

            //设置实体状态
            db.Entry(bc).State = System.Data.EntityState.Deleted;
          
            //判断该条评论的二级评论条数是否大于等于1
            if (bclist1.Count() >= 1)
            {
                //遍历该评论的二级评论的集合
                foreach (var item in bclist1)
                {
                    //递归调用
                    del(db, querybc, item);
                }

            }

        }


        /// <summary>
        /// 我评论的
        /// </summary>
        /// <param name="searchcontent"></param>
        /// <returns></returns>

        public ActionResult MyCommentIndex()
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

            ULogin ul = Session["ULogin"] as ULogin;

            //博主评论别人的信息
            List<Blog_Comment> bcomlist = db.Blog_Comment.AsNoTracking().Where( a=>a.BC_UserID == ul.ULogin_Id).ToList();

            //三表联查
            var query = from b in bcomlist
                        join wb in db.Write_blog on b.BC_ArticleID equals wb.Write_blog_Id
                        join u in db.ULogin on b.BC_ForUserID equals u.ULogin_Id
                        select new BC_CommentDTO
                        {
                            BC_CommentId = b.BC_CommentId,
                            BC_ArticleID = b.BC_ArticleID,
                            BC_Comment = b.BC_Comment,
                            BC_Comment_Time = b.BC_Comment_Time,
                            Write_Title = wb.Write_Title,
                            Ulogin_BlogName = u.Ulogin_BlogName
                        };

            //分页
            List<BC_CommentDTO> bcdlist = query.OrderByDescending(a => a.BC_CommentId).Skip((rows - 1) * 5).Take(5).ToList();

            //得到所有条数
            int allcount = query.Count();
            //得到所有页数
            int allpage = allcount / 5;
            //判断是否除尽或页数是否为0
            if (allcount % 5 != 0 || allpage == 0)
            {
                allpage = allpage + 1;
            }

            ViewBag.bcdlist = bcdlist;
            ViewBag.allpage = allpage;
            ViewBag.rows = rows;
            return View();
        }


    }
}
