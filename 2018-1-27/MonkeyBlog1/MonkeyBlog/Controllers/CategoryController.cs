using MonkeyBlog.DTO;
using MonkeyBlog.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonkeyBlog.Controllers
{
    public class CategoryController : Controller
    {
        /// <summary>
        /// 名称：添加个人类别视图
        /// 时间：18-1-19
        /// 作者：赵文涵
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCategory()
        {

            //严重登陆信息
            IsLogin();

            //int rows;
            ////如果为""就赋值为1
            //if (Request["num"] == "")
            //{
            //    rows = 1;
            //}
            ////如果为null就赋值为1
            //rows = Convert.ToInt32(Request["num"] ?? "1");

            //获取当前登陆用户的session
            int _loginId = (Session["ULogin"] as ULogin).ULogin_Id;

            //查询全部
            List<Category> lis = DTO_Category.SelectCategory(_loginId).ToList();

            ////分页后的条数
            //List<Category> lis = lis2.Skip((rows - 1) * 5).Take(5).ToList();

            ////得到所有条数
            //int allcount = lis2.Count();

            ////得到所有页数
            //int allpage = allcount / 5;

            ////判断是否除尽或页数是否为0
            //if (allcount % 5 != 0 || allpage == 0)
            //    allpage += 1;

            return View(lis);
        }


        /// <summary>
        /// 名称：添加个人类别
        /// 时间：18-1-19
        /// 作者：
        /// </summary>
        /// <param name="_ct"></param>
        public void AddCategory1(string _ct)
        {
            IsLogin();
            //获取当前登陆用户的session
            int _loginId = (Session["ULogin"] as ULogin).ULogin_Id;

            if (DTO_Category.IsContains(_loginId, _ct))
            {
                //创建要添加的数据
                Category _ca = new Category() { Category_Type = _ct, Category_Reception = 0, ULogin_Id = _loginId };

                int _saveval = DTO_Category.AddCategory(_ca);

                if (_saveval > 0)
                {
                    //返回类别添加成功，返回码000000
                    Response.Write(JsonConvert.SerializeObject(new DTO_Result() { Result_Code = "000000", Result_Title = "添加成功", value = _saveval.ToString() }));
                }
                else
                {
                    //返回类别添加失败
                    Response.Write(JsonConvert.SerializeObject(new DTO_Result() { Result_Code = new Random().Next(100000, 999999).ToString(), Result_Title = "添加失败" }));
                }
            }
            else
            {
                //返回类别已存在提示
                Response.Write(JsonConvert.SerializeObject(new DTO_Result() { Result_Code = new Random().Next(100000, 999999).ToString(), Result_Title = "‘" + _ct + "’已存在" }));
            }
        }




        /// <summary>
        /// 删除自定义类别
        /// 时间：
        /// 作者：
        /// </summary>
        /// <param name="_id"></param>
        [HttpPost]
        public void DeleteCategory1(int _id)
        {
            //验证是否登陆
            IsLogin();

            //是否获取到要操作的类别编号
            if (_id != 0)
            {
                int count = DTO_Category.DeleteCategory(_id);

                if (count > 0)
                {
                    //返回类别删除成功
                    Response.Write(JsonConvert.SerializeObject(new DTO_Result() { Result_Code = "000000", Result_Title = "删除成功" }));
                }
                else
                {
                    //返回自定义类别删除失败
                    Response.Write(JsonConvert.SerializeObject(new DTO_Result() { Result_Code = new Random().Next(100000, 999999).ToString(), Result_Title = "删除失败" }));
                }
            }
        }


        /// <summary>
        /// 修改类别名称
        /// 时间：1-22
        /// 作者：
        /// </summary>
        /// <param name="_id">类别编号</param>
        /// <param name="_ct">修改后的类别名称</param>
        [HttpPost]
        public void UpdateTypeCategory1(int _id, string _ct)
        {
            //验证是否登陆
            IsLogin();

            //获取当前登陆用户的session
            int _loginId = (Session["ULogin"] as ULogin).ULogin_Id;

            //是否获取到要操作的类别编号
            if (_id != 0)
            {
                if (DTO_Category.IsContains(_loginId, _id, _ct))
                {
                    int count = DTO_Category.UpdateCategory(new Category() { Category_Id = _id, Category_Type = _ct });

                    if (count > 0)
                    {
                        //返回类别名称修改成功
                        Response.Write(JsonConvert.SerializeObject(new DTO_Result() { Result_Code = "000000", Result_Title = "修改成功" }));
                    }
                    else
                    {
                        //返回自定义类别修改失败
                        Response.Write(JsonConvert.SerializeObject(new DTO_Result() { Result_Code = new Random().Next(100000, 999999).ToString(), Result_Title = "修改失败" }));
                    }
                }
                else
                {
                    Response.Write(JsonConvert.SerializeObject(new DTO_Result() { Result_Code = new Random().Next(100000, 999999).ToString(), Result_Title = "\"" + _ct + "\"已存在,修改失败" }));
                }
            }
        }


        /// <summary>
        /// 验证类别下是否存在内容
        /// </summary>
        /// <param name="_id">类别编号</param>
        /// <returns>查询后的结果集</returns>
        public JsonResult IsContainChild(string _id)
        {
            int _bo = DTO_Category.SelectChildCount(_id);

            if (_bo > 0)
            {
                return Json(new DTO_Result() { Result_Code = new Random().Next(100000, 999999).ToString(), Result_Title = "该类别下存在内容，是否删除？" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new DTO_Result() { Result_Code = "000000", Result_Title = "" }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// 修改显示状态
        /// </summary>
        /// <param name="_id">类别编号</param>
        /// <param name="_reception">当前显示状态</param>
        [HttpPost]
        public void UpdateReceptionCategory1(int _id, string _reception)
        {
            //验证是否登陆
            IsLogin();

            //获取当前登陆用户的session
            int _loginId = (Session["ULogin"] as ULogin).ULogin_Id;

            //是否获取到要操作的类别编号
            if (_id != 0)
            {
                if (!DTO_Category.IsContains(_loginId, _id))
                {
                    int count = DTO_Category.UpdateCategory(new Category() { Category_Id = _id, Category_Reception = _reception == "展示" ? 1 : 0 });

                    if (count > 0)
                    {
                        //返回类别名称修改成功
                        Response.Write(JsonConvert.SerializeObject(new DTO_Result() { Result_Code = "000000", Result_Title = "修改成功" }));
                    }
                    else
                    {
                        //返回自定义类别修改失败
                        Response.Write(JsonConvert.SerializeObject(new DTO_Result() { Result_Code = new Random().Next(100000, 999999).ToString(), Result_Title = "修改失败" }));
                    }
                }
            }
        }


        [HttpPost]
        public JsonResult SearchCategory(string _val)
        {
            IsLogin();

            //获取用户登录账号编号
            var _ulogin = Session["ULogin"] as ULogin;

            return Json(DTO_Category.SelectCategory(_ulogin.ULogin_Id, _val), JsonRequestBehavior.AllowGet);

            //SelectCategory
        }




        /// <summary>
        /// 名称：验证是否登陆
        /// 时间：18-1-19
        /// 作者：赵文涵
        /// </summary>
        private void IsLogin()
        {
            if (Session["ULogin"] == null)
            {
                Response.Redirect("");
            }
        }
    }
}
