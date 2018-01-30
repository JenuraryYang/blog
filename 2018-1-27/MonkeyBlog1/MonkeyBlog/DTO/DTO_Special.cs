using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonkeyBlog.DTO
{
    /// <summary>
    /// 名称：博客专栏
    /// 时间：1-18
    /// 作者：
    /// </summary>
    /// <returns></returns>
    public class DTO_Special
    {
        /// <summary>
        /// 名称：添加专栏信息
        /// 时间：1-18
        /// 作者：
        /// </summary>
        /// <param name="_sp">要添加的专栏数据</param>
        /// <returns>新添加的自定义类别自增id</returns>
        public static int AddSpecial(Special _sp)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            _db.Special.Add(_sp);

            //提交
            _db.SaveChanges();

            //返回自增id
            return _sp.Special_Id;
        }



        /// <summary>
        /// 名称：修改专栏信息
        /// 时间：1-18
        /// 作者：
        /// </summary>
        /// <param name="_sp">修改的数据</param>
        /// <returns>操作受影响的行数</returns>
        public static int UpdateSpecial(Special _sp)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            var _upd = _db.Entry(_sp);

            //指定数据已更新
            _upd.State = System.Data.EntityState.Unchanged;

            //指定修改的字段
            _upd.Property("Special_Name").IsModified = !string.IsNullOrEmpty(_sp.Special_Name);
            _upd.Property("Special_Introduce").IsModified = !string.IsNullOrEmpty(_sp.Special_Introduce);

               
            return _db.SaveChanges();
        }



        /// <summary>
        /// 名称：删除专栏信息
        /// 时间：1-18
        /// 作者：
        /// </summary>
        /// <param name="_spId">要删除的专栏编号</param>
        /// <returns>新添加的自定义类别自增id</returns>
        public static int DeleteSpecial(int _spId)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            //编辑要删除的数据
            Special _sp = new Special() { Special_Id = _spId };

            //指定要删除的自定义类别
            _db.Entry(_sp).State = System.Data.EntityState.Deleted;

            _db.Special.Remove(_sp);

            return _db.SaveChanges();
        }

    }
}