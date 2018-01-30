using MonkeyBlog.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MonkeyBlog.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        /// <summary>
        /// 验证码超时时间
        /// </summary>
        private int OutTime = 310;


        /// <summary>
        /// 名称：登录
        /// 时间：
        /// 作者：
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginIndex()
        {
            return View();
        }





        /// <summary>
        /// 名称：注册
        /// 时间：
        /// 作者：
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }



        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetThePassword(string phone)
        {
            ViewData.Add("phone", phone);

            return View();
        }


        /// <summary>
        /// 名称：忘记密码（找回密码）
        /// 时间：
        /// 作者：
        /// </summary>
        /// <param name="phone">账号</param>
        /// <returns></returns>
        public JsonResult ForgetCode(string phone)
        {
            //判断手机号是否为空
            if (string.IsNullOrEmpty(phone))
                return Json(new DTO_Result() { Result_Code = "000", Result_Title = "请输入手机号" }, JsonRequestBehavior.AllowGet);
            //判断手机号是否符合规则
            else if (!IsMobilePhone(phone))
                return Json(new DTO_Result() { Result_Code = "000", Result_Title = "手机号格式不正确" }, JsonRequestBehavior.AllowGet);
            else
            {
                if (new MonkeyDBEntities().ULogin.Where(a => a.ULogin_Phone.Equals(phone)).Count() > 0)
                {

                    //发送验证码，获得返回值
                    //var _resCode = SendMessage(phone);
                    KeyValuePair<string, string> _resCode = new KeyValuePair<string, string>("000000","00000");

                    if (_resCode.Value.Equals("00000"))
                    {
                        //验证码发送结果
                        //在session中写入验证码内容和发送验证码时间
                        Session["sendCode"] = new DTO_Reg() { Code = _resCode.Key, Phone = phone, Time = DateTime.Now };
                        return Json(new DTO_Result() { Result_Code = "000000", Result_Title = "验证码发送成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //验证码发送结果
                        return Json(new DTO_Result() { Result_Code = "8888", Result_Title = "验证码发送失败" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new DTO_Result() { Result_Code = "8888", Result_Title = "该手机号未注册" }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        /// <summary>
        /// 名称：修改密码
        /// 时间：
        /// 作者：
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="newpassword">新密码</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public JsonResult ForgetSucc(string newpassword, string code)
        {
            //验证已获取验证码
            if (Session["sendCode"] == null)
            {
                return Json(new DTO_Result() { Result_Code = "6565", Result_Title = "验证码输入错误" },JsonRequestBehavior.AllowGet);
            }
            else if(!code.Equals((Session["sendCode"] as DTO_Reg).Code))
            {
                return Json(new DTO_Result() { Result_Code = "6565", Result_Title = "验证码输入错误" }, JsonRequestBehavior.AllowGet);
            }
            else if (string.IsNullOrEmpty(newpassword))
            {
                return Json(new DTO_Result() { Result_Code = "656665", Result_Title = "请输入密码" }, JsonRequestBehavior.AllowGet);
            }

            //创建数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            string s = (Session["sendCode"] as DTO_Reg).Phone;
            //查询用户
            ULogin _ul = _db.ULogin.Where(a => a.ULogin_Phone .Equals( s)).FirstOrDefault();

            if (_ul != null)
            {
                _ul.ULogin_Password = MD5JM(newpassword);
            }

            //提交数据
            if (_db.SaveChanges() > 0)
            {
                return Json(new DTO_Result() { Result_Code = "000000", Result_Title = "密码修改成功，请重新登录" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new DTO_Result() { Result_Code = "004400", Result_Title = "密码修改失败" }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// 名称：注册，录入数据
        /// 时间：
        /// 作者：
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Register_in(string username, string password, string repassword, string code)
        {
            //判断手机号是否为空
            if (string.IsNullOrEmpty(username))
                return Json(new DTO_Result() { Result_Code = "000", Result_Title = "请输入手机号" }, JsonRequestBehavior.AllowGet);
            //判断手机号是否符合规则
            else if (!IsMobilePhone(username))
                return Json(new DTO_Result() { Result_Code = "000", Result_Title = "手机号格式不正确" }, JsonRequestBehavior.AllowGet);
            //判断是否输入验证码
            else if (Session["sendCode"] == null || string.IsNullOrEmpty(code))
                return Json(new DTO_Result() { Result_Code = "000", Result_Title = "请输入验证码" }, JsonRequestBehavior.AllowGet);
            //判断是否输入密码
            else if (string.IsNullOrEmpty(password))
                return Json(new DTO_Result() { Result_Code = "0000", Result_Title = "请输入密码" }, JsonRequestBehavior.AllowGet);
            //判断两次输入的密码
            else if (!password.Equals(repassword))
                return Json(new DTO_Result() { Result_Code = "0000", Result_Title = "前后两次密码不一致" }, JsonRequestBehavior.AllowGet);
            //验证验证码是否输入正确
            else if (!code.Equals((Session["sendCode"] as DTO_Reg).Code))
                return Json(new DTO_Result() { Result_Code = "0000", Result_Title = "验证码输入错误" }, JsonRequestBehavior.AllowGet);
            //验证验证码是否超时
            else if ((DateTime.Now - ((Session["sendCode"] as DTO_Reg).Time)).TotalSeconds > OutTime)
                return Json(new DTO_Result() { Result_Code = "0000", Result_Title = "验证码超时" }, JsonRequestBehavior.AllowGet);

            //录入数据
            else
            {
                //创建数据库上下文对象
                MonkeyDBEntities _db = new MonkeyDBEntities();


                //验证手机号是否注册
                if (_db.ULogin.Where(a => a.ULogin_Phone == username).Count() <= 0)
                {
                    //录入
                    Users_Details _det = _db.Users_Details.Add(new Users_Details() { Users_Details_Name = username, Users_Details_Follow = 0, Users_Details_Etc = 0, Users_Details_LoginDate = DateTime.Now });


                    ULogin _ul = _db.ULogin.Add(new ULogin() { Ulogin_BlogName = username, ULogin_Password = MD5JM(password), ULogin_Phone = username, Users_Details_ID = _det.Users_Details_ID });

                    if (_db.SaveChanges() >= 2)
                    {
                        return Json(new DTO_Result() { Result_Code = "000000", Result_Title = "注册成功" });
                    }
                    else
                    {
                        _db.ULogin.Remove(_ul);
                        _db.Users_Details.Remove(_det);

                        return Json(new DTO_Result() { Result_Code = "0000", Result_Title = "注册失败" });
                    }
                }
                else
                {
                    return Json(new DTO_Result() { Result_Code = "0000", Result_Title = "该用户已存在，注册失败" });
                }
            }
        }



        /// <summary>
        /// 名称：登录验证
        /// 时间：1-26
        /// 作者：
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public JsonResult Login_Verification(string username, string password)
        {
            //创建本次请求的返回对象
            DTO_Result _res = new DTO_Result();

            //验证非空
            if (string.IsNullOrEmpty(username))
            {
                _res.Result_Code = new Random().Next(100000, 999999).ToString();
                _res.Result_Title = "手机号不能为空";
                return Json(_res, JsonRequestBehavior.AllowGet);
            }
            else if (string.IsNullOrEmpty(password))
            {
                _res.Result_Code = new Random().Next(100000, 999999).ToString();
                _res.Result_Title = "密码不能为空";
                return Json(_res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //验证手机号码格式
                if (IsMobilePhone(username))
                {
                    //创建数据库上下文对象
                    MonkeyDBEntities _db = new MonkeyDBEntities();

                    string _jmpass = MD5JM(password);

                    //查询用户信息
                    var _loginuser = _db.ULogin.Where(a => a.ULogin_Phone.Equals(username) && a.ULogin_Password.Equals(_jmpass));


                    if (_loginuser.Count() == 1)
                    {
                        //获取第一个元素
                        Session["ULogin"] = _loginuser.FirstOrDefault();
                        //提示登录成功
                        _res.Result_Code = "000000";
                        _res.Result_Title = "登录成功";
                        return Json(_res, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //提示登录失败
                        _res.Result_Code = new Random().Next(100000, 999999).ToString();
                        _res.Result_Title = "手机号或密码错误，登录失败";
                        return Json(_res, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    _res.Result_Code = new Random().Next(100000, 999999).ToString();
                    _res.Result_Title = "手机号格式不正确";
                    return Json(_res, JsonRequestBehavior.AllowGet);
                }
            }

        }



        /// <summary>
        /// 名称：发送验证码
        /// 时间：
        /// 作者：
        /// </summary>
        /// <param name="username">手机号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCode(string username)
        {
            //验证手机号非空
            if (string.IsNullOrEmpty(username))
            {
                return Json(new DTO_Result() { Result_Code = "0000", Result_Title = "请输入手机号" }, JsonRequestBehavior.AllowGet);
            }
            //验证手机号格式
            else if (!IsMobilePhone(username))
            {
                return Json(new DTO_Result() { Result_Code = "2222", Result_Title = "手机号格式不正确" }, JsonRequestBehavior.AllowGet);
            }
            //发送验证码
            else if (new MonkeyDBEntities().ULogin.Where(a => a.ULogin_Phone.Equals(username)).Count() <= 0)
            {
                //发送验证码，获得返回值
                var _resCode = SendMessage(username);

                if (_resCode.Value.Equals("00000") && !string.IsNullOrEmpty(_resCode.Value))
                {
                    //验证码发送结果
                    //在session中写入验证码内容和发送验证码时间
                    Session["sendCode"] = new DTO_Reg() { Code = _resCode.Key, Phone = username, Time = DateTime.Now };
                    return Json(new DTO_Result() { Result_Code = "000000", Result_Title = "验证码发送成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //验证码发送结果
                    return Json(new DTO_Result() { Result_Code = "8888", Result_Title = "验证码发送失败" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new DTO_Result() { Result_Code = "8888", Result_Title = "该手机号已注册" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 名称：验证手机号格式
        /// 时间：1-26
        /// 作者：
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool IsMobilePhone(string input)
        {
            Regex regex = new Regex("^1[34578]\\d{9}$");
            return regex.IsMatch(input);

        }



        //public string dl_Login(string name, string pwd)
        //{
        //    MonkeyDBEntities be = new MonkeyDBEntities();

        //    ULogin u = be.ULogin.Where(a => (a.ULogin_Phone == name) && (a.ULogin_Password == pwd)).FirstOrDefault();

        //    Session["user"] = u;

        //    if (String.IsNullOrWhiteSpace(name.Trim()))
        //    {
        //        return "nullname";
        //    }
        //    if (String.IsNullOrWhiteSpace(pwd.Trim()))
        //    {
        //        return "nullpwd";
        //    }
        //    else if (u != null)
        //    {
        //        return "成功";
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}


        //public string add_Login(string name, string pwd, string pwd2, string yzm)
        //{
        //    MonkeyDBEntities be = new MonkeyDBEntities();
        //    ULogin u = new ULogin();
        //    u.ULogin_Phone = name;
        //    u.ULogin_Password = pwd;

        //    if (String.IsNullOrWhiteSpace(name.Trim()))
        //    {
        //        return "nullname";
        //    }
        //    else if (be.ULogin.Where(a => a.ULogin_Phone == u.ULogin_Phone).FirstOrDefault() != null)
        //    {
        //        return "用户名";
        //    }
        //    else if (String.IsNullOrWhiteSpace(pwd.Trim()) && String.IsNullOrWhiteSpace(pwd2.Trim()))
        //    {
        //        return "nullpwd";
        //    }
        //    else if (pwd != pwd2)
        //    {
        //        return "密码";
        //    }
        //    else if (yzm != Convert.ToString(Session["number"]))
        //    {
        //        return "验证码";
        //    }
        //    else if (yzm == "")
        //    {
        //        return "验证码为空";
        //    }
        //    else
        //    {
        //        be.ULogin.Add(u);
        //        int cunt = be.SaveChanges();
        //        return "成功";
        //    }

        //}

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
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumber">发送的电话号码集合，电话号码之间用英文逗号隔开</param>
        /// <param name="datetimenow">时间搓</param>
        /// <param name="md5string"></param>
        private KeyValuePair<string, string> SendMessage(string phoneNumber)
        {
            //获取当前时间字符串
            string datetimenow = DateTime.Now.ToString("yyyyMMddHHmmss");

            //验证码来源
            string _str = "123456789";

            Random r = new Random();
            StringBuilder yz = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                yz.Append(_str.Substring(r.Next(_str.Length), 1));
            }

            MD5 md5 = MD5.Create();
            byte[] md5byte = md5.ComputeHash(Encoding.UTF8.GetBytes("5380ba1a2be242ddb685addbead6fe0d" + "504dbc18e237400a942b48d98f7a958d" + datetimenow));

            string md5string = "";

            foreach (var item in md5byte)
            {
                md5string += item.ToString("x2");
            }

            //创建一个http对象
            HttpClient hc = new HttpClient();

            List<KeyValuePair<string, string>> lis = new List<KeyValuePair<string, string>>();
            //开发者主账号ID（ACCOUNT SID）
            lis.Add(new KeyValuePair<string, string>("accountSid", "5380ba1a2be242ddb685addbead6fe0d"));

            //短信模板ID （smsContent与templateid必须填写一项）（具体登录官网后，在模板管理查看：模板Id）
            lis.Add(new KeyValuePair<string, string>("templateid", "165330540"));

            //短信变量，多个变量用英文逗号隔开（如：模板的格式为：您的订单{1}已经处理完成，货物即将发出，请于近{2}日内查收。只有templateid有内容时该参数才可填写）
            lis.Add(new KeyValuePair<string, string>("param", yz + ",5"));

            //短信接收端手机号码集合。用英文逗号分开，每批发送的手机号数量不得超过100个。
            lis.Add(new KeyValuePair<string, string>("to", phoneNumber));

            //时间戳。当前系统时间（24小时制），格式"yyyyMMddHHmmss"。时间戳有效时间为5分钟。
            lis.Add(new KeyValuePair<string, string>("timestamp", datetimenow));

            //签名。MD5(ACCOUNT SID + AUTH TOKEN + timestamp)。共32位（小写）。注意：MD5中的内容不包含”+”号。
            lis.Add(new KeyValuePair<string, string>("sig", md5string.ToLower()));

            //响应数据类型，JSON 或 XML 格式。默认为JSON
            //lis.Add(new KeyValuePair<string, string>("respDataType", "JSON"));

            HttpContent hcontent = new FormUrlEncodedContent(lis);

            //异步发送post请求
            HttpResponseMessage hm = hc.PostAsync("https://api.miaodiyun.com/20150822/industrySMS/sendSMS", hcontent).Result;

            return new KeyValuePair<string, string>(yz.ToString(), hm.Content.ReadAsStringAsync().Result);
        }



        /// <summary>
        /// 名称：发送验证码
        /// 时间：
        /// 作者：
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private KeyValuePair<string, string> Sms(string _username)
        {
            Random rnd = new Random();
            int num = rnd.Next(1000, 9999);

            string dt = DateTime.Now.ToString();

            HttpClient hc = new HttpClient();

            List<KeyValuePair<string, string>> kvlist = new List<KeyValuePair<string, string>>();

            kvlist.Add(new KeyValuePair<string, string>("accountSid", "c5f036f8ab9e42e7829d16da75c8b7be"));
            kvlist.Add(new KeyValuePair<string, string>("templateid", "161027802"));
            kvlist.Add(new KeyValuePair<string, string>("param", "【猿博客】登录验证码：{" + num + "}，如非本人操作，请忽略此短信。"));
            kvlist.Add(new KeyValuePair<string, string>("to", _username));
            kvlist.Add(new KeyValuePair<string, string>("timestamp", dt));
            kvlist.Add(new KeyValuePair<string, string>("sig", MD5JM("c5f036f8ab9e42e7829d16da75c8b7be" + "9f9b52e518ef4c3e9167dcf593f4631a" + dt).ToLower()));

            HttpContent content = new FormUrlEncodedContent(kvlist);

            HttpResponseMessage hrm = hc.PostAsync("https://api.miaodiyun.com/20150822/industrySMS/sendSMS", content).Result;

            string result = hrm.Content.ReadAsStringAsync().Result;

            KeyValuePair<string, string> _code = new KeyValuePair<string, string>(num.ToString(), "");

            return _code;
        }
    }
}
