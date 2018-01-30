using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonkeyBlog.DTO
{
    public class BC_CommentDTO
    {
        public int? BC_CommentId { get; set; }//评论的编号
        public int? BC_ArticleID { get; set; }//文章的编号
        public string BC_Comment { get; set; }//评论内容
        public DateTime? BC_Comment_Time { get; set; }//评论时间
        public string Write_Title { get; set; }//文章题目
        public string Ulogin_BlogName { get; set; }//评论文章的人的名称
    }
}