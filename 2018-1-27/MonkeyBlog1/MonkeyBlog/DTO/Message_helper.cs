using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonkeyBlog.DTO
{
    public class Message_helper
    {
        /// <summary>
        /// 评论详情
        /// </summary>
        public Comment Commt { get; set; }
        /// <summary>
        /// 评论者详情
        /// </summary>
        public Users_Details UserD { get; set; }
    }
}