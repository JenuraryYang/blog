using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace MonkeyBlog.DTO.Hubs
{
    public class MessageHub:Hub
    {

        public void SendMessage(string content, int comment_ID, int comment_user)
        {
           MonkeyDBEntities  blog = new MonkeyDBEntities();

            //向数据库添加信息

            string dtime = DateTime.Now.ToString();

            string dttime_two= dtime.Substring(dtime.LastIndexOf(" ")+1);

            Comment comm_Window_Insert = new Comment() {
            Comment_Time = Convert.ToDateTime(dttime_two),
            Comment_Content=content,
            Parent_Comment_Id=comment_ID,
            Comment_User=comment_user,
            ULogin_Id=comment_user
            };
            //添加信息
            blog.Entry(comm_Window_Insert).State = System.Data.EntityState.Added;

            int count = blog.SaveChanges();
            //获取头像
            string img = blog.Users_Details.Where(a => a.Users_Details_ID == comment_user).Select(a => a.Users_Details_Photo).First();

            Clients.Caller.getMessage(dtime, content, img);

        }


        /// <summary>
        /// 前后台关联成功之后执行
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

    }
}