using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonkeyBlog.DTO
{
    public class MessageWindow_helper
    {
        /// <summary>
        /// 评论者编号
        /// </summary>
        public int? Windom_Comment_ID { get; set; }
        /// <summary>
        /// 博主编号
        /// </summary>
        public int? blog_id { get; set; }
        /// <summary>
        /// 评论者编号
        /// </summary>
        public int? Comment_user { get; set; }

        /// <summary>
        /// 回复消息
        /// </summary>
        public string Comment_Content { get; set; }
        /// <summary>
        /// 消息发送时间
        /// </summary>
        public DateTime? Message_SendDate { get; set; }

        /// <summary>
        /// 评论者头像
        /// </summary>
        public string Img_Head { get; set; }
        /// <summary>
        /// 账号名
        /// </summary>
        public string name { get; set; }

    }

    public class Message_ShowWindow {

        /// <summary>
        /// 评论者编号
        /// </summary>
        public int? Comment_ID { get; set; }

        public string Message_name { get; set; }

        public List<MessageWindow_helper> mwhList { get; set; }


    }
}