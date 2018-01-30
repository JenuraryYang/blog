using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonkeyBlog.DTO
{
    /// <summary>
    /// 名称：博客文章管理
    /// 时间：1-17
    /// 作者：
    /// </summary>
    public class DTO_Write_blog
    {
        /// <summary>
        /// 查询指定用户所有未删除的文章（用户自己查看自己的文章）
        /// 时间：1-17
        /// </summary>
        /// <param name="_loginId">登陆用户编号</param>
        /// <returns></returns>
        public static List<Write_blog> SelectU(int _loginId)
        {
            return new MonkeyDBEntities().Write_blog.Where(a => a.ULogin_Id == _loginId && a.Write_IsDel != 0).ToList();
        }



        /// <summary>
        /// 查询指定用户的所有删除的文章（用户查看自己的回收站）
        /// 时间：1-17
        /// </summary>
        /// <param name="_loginId">用户编号</param>
        /// <returns></returns>
        public static List<Write_blog> SelectDel(int _loginId)
        {
            return new MonkeyDBEntities().Write_blog.Where(a => a.ULogin_Id == _loginId && a.Write_IsDel == 0).ToList();
        }



        /// <summary>
        /// 查询指定用户的公开、发布且审核通过的文章（访客访问）
        /// 时间：1-17
        /// </summary>
        /// <param name="_uid">用户编号（登陆编号）</param>
        /// <returns></returns>
        public static List<Write_blog> SelectUA(int _uid)
        {
            return new MonkeyDBEntities().Write_blog.Where(a => a.ULogin_Id == _uid && a.Write_Private == 1 && a.Write_IsDel == 2 && a.Write_State == 2).ToList();
        }



        /// <summary>
        /// 查询指定用户的公开、发布且审核通过的文章内容（访客访问）
        /// 时间：1-17
        /// </summary>
        /// <param name="_uid">文章编号</param>
        /// <returns></returns>
        public static Write_blog Select(int _blogId)
        {
            return new MonkeyDBEntities().Write_blog.Where(a => a.Write_blog_Id == _blogId && a.Write_State == 2 && a.Write_Private == 1 && a.Write_IsDel == 2).FirstOrDefault();
        }



        /// <summary>
        /// 添加博客文章
        /// 时间：1-17
        /// 修改：1、时间：1-18,内容：将返回值修改为当前添加的文章的自增id
        /// </summary>
        /// <param name="_wb">文章信息</param>
        /// <returns>当前添加的文章的自增ID</returns>
        public static int AddBlog(Write_blog _wb)
        {
            MonkeyDBEntities _db = new MonkeyDBEntities();

            //判断文章是否公开，公开就将审核状态改为0（待审核）
            //if (_wb.Write_Private == 1)
            _wb.Write_State = 0;

            //将浏览量和点赞量设为0
            _wb.Write_ReadCount = 0;
            _wb.Write_ZanCount = 0;
            _wb.Write_Collection = 0;
            _wb.ULogin_IdIsManager = "否";

            //如果创建时间为空，就将系统当前时间设置为文章创建时间
            if (_wb.Write_Createdate == null)
                _wb.Write_Createdate = DateTime.Now;
            _wb.Blog_Recommend = 2;

            //添加到数据库但未提交
            _db.Write_blog.Add(_wb);

            //提交到数据库，返回提交结果
            _db.SaveChanges();

            return _wb.Write_blog_Id;
        }


        /// <summary>
        /// 将指定文章收入回收站
        /// 时间：1-17
        /// </summary>
        /// <param name="_blogId">文章编号</param>
        /// <returns></returns>
        public static int DeleteBlog(int _blogId)
        {
            MonkeyDBEntities _db = new MonkeyDBEntities();

            //创建要删除的实体对象
            Write_blog _wb = new Write_blog() { Write_blog_Id = _blogId, Write_Deletedate = DateTime.Now, Write_IsDel = 0 };



            var _upd = _db.Entry(_wb);

            //指定数据已更新
            _upd.State = System.Data.EntityState.Unchanged;

            //指定修改的字段
            _upd.Property("Write_Deletedate").IsModified = true;
            _upd.Property("Write_IsDel").IsModified = true;

            //提交数据，并返回结果
            return _db.SaveChanges();
        }


        /// <summary>
        /// 删除指定文章（真删，彻底清除文章）
        /// 时间：1-17
        /// </summary>
        /// <param name="_blogId">文章编号</param>
        /// <returns></returns>
        public static int DeleteOneBlog(int _blogId)
        {
            MonkeyDBEntities _db = new MonkeyDBEntities();

            //创建要删除的实体对象
            Write_blog _wb = new Write_blog() { Write_blog_Id = _blogId };

            //指定需要删除的数据
            _db.Entry(_wb).State = System.Data.EntityState.Deleted;

            _db.Write_blog.Remove(_wb);

            //提交数据库并返回结果
            return _db.SaveChanges();
        }



        /// <summary>
        /// 添加阅读量
        /// 时间：1-17
        /// 修改：1、时间：1-18
        /// </summary>
        /// <param name="_blogId">要添加阅读量得文章编号</param>
        /// <returns></returns>
        public static int AddReadCount(int _blogId)
        {
            MonkeyDBEntities _db = new MonkeyDBEntities();

            string sql = "update Write_blog set Write_ReadCount=Write_ReadCount +1 where Write_blog_Id = " + _blogId;

            //提交数据，并返回结果
            return _db.Database.ExecuteSqlCommand(sql);
        }


        /// <summary>
        /// 给指定文章添加点赞
        /// 时间：1-18
        /// </summary>
        /// <param name="_blogId">被点赞得文章编号</param>
        /// <returns></returns>
        public static int AddZanCount(int _blogId)
        {
            return ZanCount(_blogId, 1);
        }


        /// <summary>
        /// 给指定文章取消点赞
        /// 时间：1-18
        /// </summary>
        /// <param name="_blogId">被取消点赞的文章编号</param>
        /// <returns></returns>
        public static int MinusZanCount(int _blogId)
        {
            return ZanCount(_blogId, -1);
        }


        /// <summary>
        /// 操作文章点赞量
        /// 时间：1-18
        /// </summary>
        /// <param name="_blogId">被点赞的文章编号</param>
        /// <param name="_val">增加点赞量或减少点赞量的值</param>
        /// <returns></returns>
        public static int ZanCount(int _blogId, int _val)
        {
            MonkeyDBEntities _db = new MonkeyDBEntities();

            string sql = "update Write_blog set Write_ZanCount=Write_ZanCount +" + _val + " where Write_blog_Id = " + _blogId;

            //提交数据，并返回结果
            return _db.Database.ExecuteSqlCommand(sql);
        }




        /// <summary>
        /// 给指定文章添加收藏量
        /// 时间：1-18
        /// </summary>
        /// <param name="_blogId">被收藏得文章编号</param>
        /// <returns></returns>
        public static int AddCollection(int _blogId)
        {
            return Collection(_blogId, 1);
        }


        /// <summary>
        /// 给指定文章取消收藏
        /// 时间：1-18
        /// </summary>
        /// <param name="_blogId">被取消点赞收藏的文章编号</param>
        /// <returns></returns>
        public static int MinusCollection(int _blogId)
        {
            return Collection(_blogId, -1);
        }


        /// <summary>
        /// 操作文章收藏量
        /// 时间：1-18
        /// </summary>
        /// <param name="_blogId">被收藏的文章编号</param>
        /// <param name="_val">增加收藏量或减少收藏量的值</param>
        /// <returns></returns>
        public static int Collection(int _blogId, int _val)
        {
            MonkeyDBEntities _db = new MonkeyDBEntities();

            string sql = "update Write_blog set Write_ZanCount=Write_ZanCount +" + _val + " where Write_blog_Id = " + _blogId;

            //提交数据，并返回结果
            return _db.Database.ExecuteSqlCommand(sql);
        }


        /// <summary>
        ///  查询文章
        /// </summary>
        /// <param name="_id">用户登录编号</param>
        /// <param name="_is">是否是查询自己的文章（>0为查询自己，=0查询指定用户可见文章，<0查询所有用户可见文章）</param>
        /// <param name="_selval">查询关键字内容</param>
        /// <returns>文章集合</returns>
        public static List<Write_blog> Select(int _id, int _is, string _selval)
        {
            //创建数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();


            var _all = _db.Write_blog.AsNoTracking().ToList();
            List<Write_blog> _lis = new List<Write_blog>();


            if (_is >= 0)
            {
                _lis.AddRange(_all.Where(a => a.ULogin_Id == _id && a.Write_Title.Contains(_selval) && (_is > 0 || a.Write_IsDel == 2 && a.Write_State == 2 && a.Write_Private == 1)).ToList());
                _lis.AddRange(_all.Where(a => a.ULogin_Id == _id && a.Write_Content.Contains(_selval) && (_is > 0 || a.Write_IsDel == 2 && a.Write_State == 2 && a.Write_Private == 1)).ToList());
            }
            else
            {
                _lis.AddRange(_all.Where(a => a.Write_Title.Contains(_selval) && a.Write_IsDel == 2 && a.Write_State == 2 && a.Write_Private == 1).ToList());
                _lis.AddRange(_all.Where(a => a.Write_Content.Contains(_selval) && a.Write_IsDel == 2 && a.Write_State == 2 && a.Write_Private == 1).ToList());
            }

            return _lis;
        }


    }
}