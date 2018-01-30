using MonkeyBlog;
using MonkeyBlog.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MonkeyBlog.DTO;
using System.Security.Cryptography;

namespace MvcApplication1.Controllers
{
    public class PersonalController : Controller
    {

        //实列化数据实体模型
        MonkeyDBEntities _be = new MonkeyDBEntities();

        /// <summary>
        /// Session获取用户登录信息
        /// </summary>
        /// <returns></returns>
        public string U_Id()
        {
            ULogin ud = Session["ULogin"] as ULogin;
            string Login_Id = null;
            if (ud != null)
            {
                Login_Id = ud.ULogin_Id.ToString();
            }
            return Login_Id;
        }

        //
        // GET: /Default1/

        [IsLoginFilter]
        public ActionResult Index()
        {

            //根据用户登录，记录登录编号，获取该用户详情

            string str_U_id = U_Id();

            List<Users_Details> udlist = new List<Users_Details>();
            if (str_U_id == null)
            {
                Response.Redirect("/Login/LoginIndex");
                Response.End();
            }
            else
            {
                //根据登录编号修改个人信息
                int Login_Id = Convert.ToInt32(str_U_id);
                //根据Login_ID获取登录账号
                string Login_number = _be.ULogin.Where(a => a.ULogin_Id == Login_Id).Select(a => a.Ulogin_BlogName).FirstOrDefault();
                ViewBag.Ulogin_BlogName = Login_number;
                //根据Login_ID查询用户详情信息表
                udlist = _be.Users_Details.Where(a => a.Users_Details_ID == Login_Id).ToList();
            }


            return View(udlist);
        }

        /// <summary>
        /// 用户自己修改个人信息
        /// </summary>
        /// <param name="ud">前台修改的值</param>
        /// <returns>返回index页</returns>
        public ActionResult Update_UserInfo(Users_Details ud)
        {
            //博客账号名可能发生重复
            //用户登录编号
            string str_U_id = U_Id();
            //根据登录编号修改个人信息
            int Login_Id = Convert.ToInt32(str_U_id);
            //根据Login_ID查询个人信息
            Users_Details details = _be.Users_Details.Where(a => a.Users_Details_ID == Login_Id).FirstOrDefault();
            //修改
            details.Users_Details_Birthday = ud.Users_Details_Birthday;   //生日
            details.Users_Details_Industry = ud.Users_Details_Industry;  //行业
            details.Users_Details_Jop = ud.Users_Details_Jop;   //职位
            details.Users_Details_Name = ud.Users_Details_Name;  //昵称
            details.Users_Details_RealName = ud.Users_Details_RealName;  //真实姓名
            details.Users_Details_Sex = ud.Users_Details_Sex;   //性别
            details.Users_Details_Resume = ud.Users_Details_Resume;   //备注
            details.Users_Details_LoginUrl = ud.Users_Details_LoginUrl;   //登录地址

            int count = _be.SaveChanges();
            return RedirectToAction("Index", "Personal");
        }

        /// <summary>
        /// 点击修改头像
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult Update_Heed()
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

            if (Request.Files.Count > 0)
            {

                //先获取图片名称
                string filename = Request.Files[0].FileName;
                //取出文件后缀名
                int hz = filename.LastIndexOf('.');

                string bname = filename.Substring(hz + 1).Trim().ToLower();

                if (bname == "jpg" || bname == "png")
                {
                    string gi = Guid.NewGuid().ToString();
                    string filenames = gi + "." + bname;
                    Session["filenames"] = filenames;
                    string url = Server.MapPath("~") + @"\images\" + filenames;
                    ViewBag.url = url;
                    Request.Files[0].SaveAs(url);


                    //用户登录编号
                    string str_U_id = U_Id();
                    //根据登录编号修改个人信息
                    int Login_Id = Convert.ToInt32(str_U_id);
                    //修改
                    Users_Details Details = _be.Users_Details.Where(a => a.Users_Details_ID == Login_Id).FirstOrDefault();

                    Details.Users_Details_Photo = Session["filenames"].ToString();

                    int count = _be.SaveChanges();
                }
                else
                {
                    Response.Write("<script>alert('图片格式不正确!');window.location.href = '/Personal/index'</script>");

                    Response.End();
                }
            }

            return RedirectToAction("Index", "Personal");
        }
        /// <summary>
        /// 我的关注中心
        /// </summary>
        /// <param name="notice_type">前台发送的关注关系的类型三种可能</param>
        /// <returns></returns>
        public JsonResult Notice_Center(string notice_type)
        {

            List<Users_Details> UDlist = new List<Users_Details>();


            //用户登录编号
            string str_U_id = U_Id();
            //根据登录编号修改个人信息
            int Login_Id = Convert.ToInt32("1");

            string jsonstr = "";
            //我的关注
            if (notice_type == "MyNotice")
            {

                UDlist = _be.FollowUser.Where(a => a.FollowUser_User1 == Login_Id).ToList().Join(_be.Users_Details, a => a.FollowUser_User2, b => b.Users_Details_ID, (x, y) => new Users_Details()
                {

                    Users_Details_Photo = y.Users_Details_Photo,
                    Users_Details_Name = y.Users_Details_Name,
                    Users_Details_Resume = y.Users_Details_Resume

                }).ToList();

            }
            //谁关注我的
            if (notice_type == "HeNotice")
            {

                UDlist = _be.FollowUser.Where(a => a.FollowUser_User2 == Login_Id).ToList().Join(_be.Users_Details, a => a.FollowUser_User1, b => b.Users_Details_ID, (x, y) => new Users_Details()
                {

                    Users_Details_Photo = y.Users_Details_Photo,
                    Users_Details_Name = y.Users_Details_Name,
                    Users_Details_Resume = y.Users_Details_Resume

                }).ToList();

            }
            //相互关注
            if (notice_type == "Notices")
            {
                //关系表之间先来个级查询
                //记录我关注的账号
                List<FollowUser> Flist_My = _be.FollowUser.Where(a => a.FollowUser_User1 == Login_Id).ToList();

                //相互关注就是条件：我中有你，你中有wo
                List<FollowUser> Flist = new List<FollowUser>();
                foreach (var item in Flist_My)
                {
                    FollowUser Follow = _be.FollowUser.Where(a => a.FollowUser_User1 == item.FollowUser_User2 && a.FollowUser_User2 == item.FollowUser_User1).FirstOrDefault();

                    Flist.Add(Follow);

                }
                //获得相互关注的用户
                UDlist = Flist.Join(_be.Users_Details, a => a.FollowUser_User1, b => b.Users_Details_ID, (x, y) => new Users_Details()
                {


                    Users_Details_Photo = y.Users_Details_Photo,
                    Users_Details_Name = y.Users_Details_Name,
                    Users_Details_Resume = y.Users_Details_Resume

                }).ToList();

                //}).Join(_be.Users_Details, a =>a.Notice.FollowUser_User1, b => b.Users_Details_ID, (x, y) => new Users_Details()
                //{

                //    Users_Details_Photo = y.Users_Details_Photo,
                //    Users_Details_Name = y.Users_Details_Name,
                //    Users_Details_Resume = y.Users_Details_Resume

                //}).ToList();
            }

            //序列化
            jsonstr = JsonConvert.SerializeObject(UDlist);


            return Json(jsonstr, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 消息中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Personal_Message()
        {

            //用户登录编号,获取登录博主信息博主编号
            string str_U_id = U_Id();
            //根据登录编号修改个人信息
            int Login_Id = Convert.ToInt32(str_U_id);
            //评论表与用户评论者详情表链表查询
            //建立帮助类 Message_helper 读取别人给自己发送消息
            List<Message_helper> Mlist_ = _be.Comment.Where(a => a.ULogin_Id == Login_Id && a.Comment_User != Login_Id).Join(_be.Users_Details, a => a.Comment_User, b => b.Users_Details_ID, (x, y) => new Message_helper()
            {
                Commt = x,
                UserD = y
            }).ToList();

            //获取我给别人发送消息的
            List<Message_helper> Mess_List=  _be.Comment.Where(a=>a.Comment_User==Login_Id&&a.ULogin_Id!=Login_Id).Join(_be.Users_Details, a => a.ULogin_Id, b => b.Users_Details_ID, (x, y) => new Message_helper()
            {
                Commt = x,
                UserD = y
            }).ToList();

           List<Message_helper> Mlist= Mlist_.Union(Mess_List).ToList();


            return View(Mlist);
        }
        /// <summary>
        /// 前台发送消息
        /// </summary>
        /// <param name="jsonstr">前台拼接json字符窜</param>
        /// <returns></returns>
        public ActionResult Message_ManySend(string jsonstr)
        {
            Comment Comm_New = JsonConvert.DeserializeObject<Comment>(jsonstr);
            //添加回复消息

            string savedt = DateTime.Now.ToString().Substring(DateTime.Now.ToString().LastIndexOf(' ') + 1);

            Comment comm_insert = new Comment()
            {
                ULogin_Id = Comm_New.ULogin_Id,
                Write_blog_Id = Comm_New.Write_blog_Id,
                Comment_Content = Comm_New.Comment_Content,
                Comment_Time = Convert.ToDateTime(savedt),
                Parent_Comment_Id = Comm_New.Comment_Id,
                Comment_User = Comm_New.ULogin_Id
            };

            _be.Entry(comm_insert).State = System.Data.EntityState.Added;

            int count = _be.SaveChanges();

            return RedirectToAction("Personal_Message");
        }
        /// <summary>
        /// 双击对象列表，弹出聊天框
        /// </summary>
        /// <param name="comm_use">评论者编号</param>
        /// <param name="blog_ID">博主编号</param>
        /// <returns></returns>
        public JsonResult Message_Private(int? comm_use, int? blog_ID)
        {

            //保存聊天对象
            string name = _be.Users_Details.Where(a => a.Users_Details_ID == comm_use).Select(a => a.Users_Details_Name).First();

            //创建聊天窗的帮助类

            //获取当前评论发送给博主的信息
            List<int> ilist = _be.Comment.Where(A => A.Comment_User == comm_use).Select(A => A.Comment_Id).ToList();

            //存session记录要发送给谁信息
            Comment comment = new Comment();
            //记录最后评论者评论编号
            comment.Comment_Id = ilist[ilist.Count - 1];
            //记录博文编号
            comment.Write_blog_Id = blog_ID;

            Session["SendMessage"] = comment;


            List<Message_ShowWindow> mswList = new List<Message_ShowWindow>();

            foreach (var item in ilist)
            {
                Message_ShowWindow msw = new Message_ShowWindow();

                msw.Message_name = name;

                msw.Comment_ID = item;

                msw.mwhList = _be.Comment.Where(a => (a.Parent_Comment_Id == item || a.Comment_Id == item)&&(a.Comment_User==comm_use||a.Comment_User==blog_ID)).Select((x) => new
                {
                    Windom_Comment_ID = x.Comment_Id,
                    blog_id = x.ULogin_Id,
                    Comment_Content = x.Comment_Content,
                    Message_SendDate = x.Comment_Time,
                    Comment_user = x.Comment_User
                }).Join(_be.Users_Details, a => a.Comment_user, b => b.Users_Details_ID, (x, y) => new MessageWindow_helper()
                {
                    blog_id = x.blog_id,
                    Windom_Comment_ID = x.Windom_Comment_ID,
                    Comment_Content = x.Comment_Content,
                    Message_SendDate = x.Message_SendDate,
                    Comment_user = x.Comment_user,
                    Img_Head = y.Users_Details_Photo,
                    name = y.Users_Details_Name
                }).ToList();
                mswList.Add(msw);
            }
            //序列化
            string jsonstr = JsonConvert.SerializeObject(mswList);

            return Json(jsonstr, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 博客管理
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult Personal_Blog()
        //{

        //    //用户登录编号
        //    string str_U_id = U_Id();
        //    //根据登录编号修改个人信息
        //    int Login_Id = Convert.ToInt32(str_U_id);
        //    //根据用户Id遍历博客标题
        //    var dlist = _be.Details.Where(a => a.ULogin_Id == Login_Id).ToList();

        //    return View(dlist);
        //}

        public ActionResult Personal_Notice()
        {
            return View();
        }

        ///// <summary>
        /////全部删除博客标题列表
        ///// </summary>
        ///// <time>2018/1/22</time>
        ///// <returns></returns>
        //public JsonResult Delete_Details()
        //{

        //    //string _cid = Request["id"].Replace(","," ");
        //    //定义c数组接收前台值id
        //    string[] c = Request["id"].Split(',');
        //    List<int> lis = new List<int>();
        //    //循环解析
        //    for (int i = 0; i < c.Length - 1; i++)
        //    {
        //        lis.Add(Convert.ToInt32(c[i]));
        //    }

        //    //循环删除
        //    foreach (var item in lis)
        //    {
        //        Details _da = new Details() { Details_Id = Convert.ToInt32(item) };
        //        //指定_da对象需要在数据库中删除
        //        _be.Entry(_da).State = System.Data.EntityState.Deleted;
        //        //移除
        //        _be.Details.Remove(_da);

        //    }
        //    //Save保存受影响行数
        //    int result = _be.SaveChanges();

        //    //返回result的结果
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 单个删除博客标题列表 
        ///// <time>2018/1/22</time>
        ///// </summary>
        ///// <returns></returns>
        //public JsonResult Delete_One_Details()
        //{
        //    //获取Id
        //    int Blog_id = Convert.ToInt32(Request["blog_id"]);
        //    //实列化Details对象
        //    Details _da = new Details() { Details_Id = Blog_id };

        //    //指定_da对象需要在数据库中删除
        //    _be.Entry(_da).State = System.Data.EntityState.Deleted;
        //    //移除
        //    _be.Details.Remove(_da);
        //    //Save保存受影响行数
        //    int result = _be.SaveChanges();
        //    //返回result的结果
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}



        public ActionResult Personal_Question()
        {
            return View();
        }

        /// <summary>
        /// 账号修改
        /// </summary>
        /// <returns></returns>
        public ActionResult Account()
        {
            return View();
        }

        /// <summary>
        /// 检查旧密码是否正确
        /// </summary>
        /// <returns></returns>
        public JsonResult checkoldpassword()
        {
            //根据登录编号修改个人信息
            //用户登录编号
            string str_U_id = U_Id();
            //根据登录编号修改个人信息
            int Login_Id = Convert.ToInt32(str_U_id);
            //前台输入的旧密码
            string oldpassword = Request["oldpassword"];
            //Md5加密
            string oldpwd= MD5JM(oldpassword);
         
            //查询用户旧密码是否正确
           var result = _be.ULogin.Where(a => a.ULogin_Id == Login_Id && a.ULogin_Password == oldpwd).FirstOrDefault();

            bool falg;
            if (result != null)
            {
                falg = true;
            }
            else
            {
                falg = false;
            }

            return Json(falg, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string MD5JM(string str)
        {
            //把字符串转换为字节数组
            byte[] jmq = System.Text.Encoding.Default.GetBytes(str);

            MD5 md5 = new MD5CryptoServiceProvider();

            //通过字符串数组转换成加密后的字符串数组
            byte[] jmbehind = md5.ComputeHash(jmq);

            //密后的字节数组转换成字符串
            string strbehind = BitConverter.ToString(jmbehind).Replace("-", "");

            return strbehind;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public JsonResult EditPwd()
        {
            //根据登录编号修改个人信息
            //用户登录编号
            string str_U_id = U_Id();
            //根据登录编号修改个人信息
            int Login_Id = Convert.ToInt32(str_U_id);
            //获取新密码
            string newpwd = Request["newpassword"];
            //Md5加密
            string newpassword = MD5JM(newpwd);
            //指定_ug对象需要在数据库中修改
            ULogin _ug = new ULogin() { ULogin_Id = Login_Id, ULogin_Password = newpassword };
            //修改密码
            _be.Entry(_ug).State = System.Data.EntityState.Modified;
            //Save保存受影响行数
            int result = _be.SaveChanges();
            //返回result的结果
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
