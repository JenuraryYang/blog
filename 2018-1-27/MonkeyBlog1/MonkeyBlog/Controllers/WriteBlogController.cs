using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MonkeyBlog.DTO;

using Newtonsoft.Json;
using MonkeyBlog.Filters;

namespace MonkeyBlog.Controllers
{
    public class WriteBlogController : Controller
    {
        //
        // GET: /WriteBlog/


        /// <summary>
        /// 写博客首页
        /// </summary>
        /// <returns></returns>
        [IsLoginFilter]
        public ActionResult WriteBlogIndex()
        {

            ULogin u = new ULogin() { ULogin_Id = 1 };//Session["ULogin"] as  ULogin;

            //查询所有的后台博客类别
            List<Backstage_Category> _bclist = DTO_Backstage_Category.SelectBackstage_Category();
            //查询所有自定类别
            List<Category> clist = DTO_Category.SelectCategory(u.ULogin_Id);

            @ViewBag._bclist = _bclist;
            @ViewBag.clist = clist;

            return View();
        }

        /// <summary>
        /// 写博客
        /// </summary>
        /// <param name="Category_Type_way">判断是否有添加新的个人分类的标签</param>
        /// <returns></returns>
           [IsLoginFilter]
        [ValidateInput(false)]
        public JsonResult PublishWriteBlog(bool Category_Type_way)
        {
            MonkeyDBEntities db = new MonkeyDBEntities();
            //接收JSON串
            string jsonstr = Request["json"];

            //反序列化JSON串
            Write_blog wb = JsonConvert.DeserializeObject<Write_blog>(jsonstr);
            //赋值
            wb.ULogin_Id = 1;
            wb.Category_Id = Request["Category_Idstr"];//接收自定义分类的字符串
            wb.Write_Content = Request["Write_Content"];//文章内容
            wb.Write_Private = Convert.ToInt32(Request["Write_Private"]);
            wb.Write_IsDel = Convert.ToInt32(Request["Write_IsDel"]);
         
            
            //判断是否添加新的个人分类
            if (Category_Type_way == true)
            {
                //反序列化字符串
                Category ca = JsonConvert.DeserializeObject<Category>(jsonstr);

                //实力例集合
                List<Category> calist = new List<Category>();
                //分割字符串
                string[] Category_Typelist = ca.Category_Type.Split(',');


                //查询所有自定义个人分类
                List<Category> calist2 = db.Category.AsNoTracking().Where(a => a.ULogin_Id == 1).ToList();

                //循环分割后的分类数组
                for (int i = 0; i < Category_Typelist.Length; i++)
                {
                    //根据类名查询是否有相同的自定义个人分类
                    Category ca2 = calist2.Where(a => a.Category_Type == Category_Typelist[i]).FirstOrDefault();
                    //判断该分类是否已经存在
                    if (ca2 != null)
                    {
                        return Json("类名为'" + ca.Category_Type + "'的个人分类已经存在，重新添加或选择个人分类", JsonRequestBehavior.AllowGet);

                    }
                       
                    ca = new Category();

                   ca.ULogin_Id = 1;//用户编号
                   ca.Category_Reception = 0;//是否显示在前台0显示,1不显示
                   ca.Category_Type = Category_Typelist[i];
                    ca.Category_Type = Category_Typelist[i];
                    //添加对象到对象的集合
                    calist.Add(ca);

                }

                //便利集合
                foreach (var item in calist)
                {
                    db.Category.Add(item);    //添加到上下文中
                    int row = db.SaveChanges();//保存到数据库

                    if (row <= 0)
                    {
                        return Json("添加新的个人分类有错", JsonRequestBehavior.AllowGet);
                    }

                    if (wb.Category_Id == "" || wb.Category_Id == null)
                    {
                        wb.Category_Id += "" + item.Category_Id;
                    }
                    else
                    {

                        wb.Category_Id += "," + item.Category_Id;//得到新的个人分类字符串
                    }

                }

            }


            //调用添加博客文章的方法
            int count = DTO_Write_blog.AddBlog(wb);

            //判断是否添加成功
            if (count <= 0)
            {
                return Json("保存失败", JsonRequestBehavior.AllowGet);

            }
            return Json(count, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 修改博客页面
        /// </summary>
        /// <returns></returns>
           [IsLoginFilter]
        public ActionResult UpdateWriteBlogIndex(int Write_blog_Id)
        {
            ULogin u = Session["ULogin"]  as ULogin;//Session["ULogin"] as  ULogin;

            MonkeyDBEntities db = new MonkeyDBEntities();

            //查询所有的后台博客类别
            List<Backstage_Category> _bclist = DTO_Backstage_Category.SelectBackstage_Category();
            //查询所有自定类别
            List<Category> clist = DTO_Category.SelectCategory(u.ULogin_Id);

            Write_blog wb = db.Write_blog.Where(a => a.Write_blog_Id == Write_blog_Id).FirstOrDefault();

            string[] strlist = wb.Category_Id.Split(',');//这篇文章的类型数组

            @ViewBag._bclist = _bclist;
            @ViewBag.clist = clist;
            @ViewBag.wb = wb;
            @ViewBag.Mycalist = strlist;

            return View();
        }

        /// <summary>
        /// 执行修改博客的方法
        /// </summary>
        /// <param name="Category_Type_way">判断是否有添加新的个人分类的标签</param>
        /// <returns></returns>
     [IsLoginFilter]
        [ValidateInput(false)]
        public JsonResult UpdateWriteBlog(bool Category_Type_way)
        {
     
            MonkeyDBEntities db = new MonkeyDBEntities();
            try
            {

                //接收JSON串
                string jsonstr = Request["json"];
          
                //反序列化JSON串
                Write_blog wb = JsonConvert.DeserializeObject<Write_blog>(jsonstr);

                //赋值
                wb.Category_Id = Request["Category_Idstr"];//接收自定义分类的字符串
                wb.Write_Content = Request["Write_Content"];//文章内容
                wb.Write_Private = Convert.ToInt32(Request["Write_Private"]);
                wb.Write_IsDel = Convert.ToInt32(Request["Write_IsDel"]);
                wb.Write_State = 0;//审核状态
                wb.Write_Createdate = DateTime.Now;//创建时间
                if (wb.Write_IsDel == 1)
                {
                    wb.Write_State = null;
                }
                //判断是否添加新的个人分类
                if (Category_Type_way == true)
                {
 
                    //反序列化字符串
                    Category ca = JsonConvert.DeserializeObject<Category>(jsonstr);

                    //实力例集合
                    List<Category> calist = new List<Category>();
                    //分割字符串
                    string[] Category_Typelist = ca.Category_Type.Split(',');           

                    //查询所有自定义个人分类
                    List<Category> calist2 = db.Category.AsNoTracking().Where(a => a.ULogin_Id == 1).ToList();

                    //循环分割后的分类数组
                    for (int i = 0; i < Category_Typelist.Length; i++)
                    {
                        //根据类名查询是否有相同的自定义个人分类
                        Category ca2 = calist2.Where(a => a.Category_Type == Category_Typelist[i]).FirstOrDefault();
                        //判断该分类是否已经存在
                        if (ca2 != null)
                        {
                            return Json("类名为'" + ca.Category_Type + "'的个人分类已经存在，重新添加或选择个人分类", JsonRequestBehavior.AllowGet);

                        }
                        ca = new Category();

                        ca.ULogin_Id = 1;//用户编号
                        ca.Category_Reception = 0;//是否显示在前台0显示,1不显示
                        ca.Category_Type = Category_Typelist[i];
                        //添加对象到对象的集合
                        calist.Add(ca);

                    }

                    //便利集合
                    foreach (var item in calist)
                    {
                        db.Category.Add(item);    //添加到上下文中
                        int row = db.SaveChanges();//保存到数据库
                       
                        if (row <= 0)
                        {
                            return Json("添加新的个人分类有错", JsonRequestBehavior.AllowGet);
                        }
                        if (wb.Category_Id == "" || wb.Category_Id == null)
                        {
                            wb.Category_Id += "" + item.Category_Id;
                        }
                        else
                        {

                            wb.Category_Id += "," + item.Category_Id;//得到新的个人分类字符串
                        }
                    }
      

                }

                //获取给定实体
                var update = db.Entry(wb);

                //设置给定实体的状态
                update.State = System.Data.EntityState.Unchanged;

                //指定要修改的字段
                update.Property("Write_Title").IsModified = true;
                update.Property("Write_ArticleType").IsModified = true;
                update.Property("Write_Content").IsModified = true;
                update.Property("Backstage_Category_Id").IsModified = true;
                update.Property("Write_Label").IsModified = true;
                update.Property("Category_Id").IsModified = true;
                update.Property("Write_Title").IsModified = true;
                update.Property("Write_Private").IsModified = true;
                update.Property("Write_Createdate").IsModified = true;
                update.Property("Write_State").IsModified = true;
                update.Property("Write_IsDel").IsModified = true;

            }
            catch (Exception ex)
            {

                string str = ex.Message;
            }

            //保存到数据库
            int count = db.SaveChanges();

            if (count <= 0)
            {
                return Json("修改失败", JsonRequestBehavior.AllowGet);
            }

            return Json(count, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 博客详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailsWriteBlogIndex()
        {
            MonkeyDBEntities db = new MonkeyDBEntities();
            Write_blog wb = db.Write_blog.Where(a => a.Write_blog_Id ==10).FirstOrDefault();
 
            @ViewBag.wb = wb;
    
            return View();
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <returns></returns>
          [IsLoginFilter]
        public JsonResult DelWriteBlog(int Write_blog_Id)
        {
            MonkeyDBEntities db = new MonkeyDBEntities();
       
       
              //调用删除的方法
            int count = DTO_Write_blog.DeleteBlog(Write_blog_Id);


            return Json(count,JsonRequestBehavior.AllowGet);
        }


        public ActionResult SearchWriteBlogAllIndex()
        {
                      
            return View();
        }

        //public void onlyOne()
        //{
           ////反序列化字符串
            //Category ca = JsonConvert.DeserializeObject<Category>(jsonstr);

            //ca.ULogin_Id = 1;//用户编号
            //ca.Category_Reception = 0;//是否显示在前台0显示,1不显示

            //int CategoryId = DTO_Category.AddCategory(ca);//调用添加个人分类的方法

            ////判断该分类是否已经存在
            //if (CategoryId == -1)
            //{
            //    return Json("类名为'" + ca.Category_Type + "'的个人分类已经存在，重新添加或选择个人分类", JsonRequestBehavior.AllowGet);
            //}
            ////拼接字符串
            //wb.Category_Id += "," + CategoryId.ToString();


        //}

    }
}
