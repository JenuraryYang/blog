using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonkeyBlog.DTO
{
    public class DTO_Personal_Follow
    {
        public int ?Users_Details_ID{ get; set; }//用户Id
        public string Users_Details_Name { get; set; }//用户昵称

        public string FollowUser_Remark { get; set; }//备注
        public DateTime FollowUser_FollowDate { get; set; }//关注时间
        public int ?FollowUser_User1 { get; set; }//关注人
        public int ?FollowUser_User2 { get; set; }//被注人
    }
}