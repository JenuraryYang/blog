using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonkeyBlog.DTO
{
    /// <summary>
    /// 名称：自定义类别管理
    /// 时间：1-18
    /// 作者：
    /// </summary>
    public class DTO_Category
    {

        /// <summary>
        /// 名称：查询单个用户的所有自定义分类（分页）
        /// 时间：1-18
        /// 作者：
        /// </summary>
        /// <param name="_loginId">要查询的用户的登陆账号编号</param>
        /// <returns>查询当前用户的所有自定义类别</returns>
        public static List<Category> SelectCategory(int _loginId)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            
            return _db.Category.Where(a => a.ULogin_Id == _loginId).OrderByDescending(a=>a.Category_Id).ToList();
        }


        /// <summary>
        /// 名称：查询单个用户的所有自定义分类
        /// 时间：1-18
        /// 作者：
        /// </summary>
        /// <param name="_loginId">要查询的用户的登陆账号编号</param>
        /// <param name="_val">要查询类别关键字</param>
        /// <returns>查询当前用户的所有自定义类别</returns>
        public static List<Category> SelectCategory(int _loginId,string _val)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            return _db.Category.Where(a => a.ULogin_Id == _loginId&&a.Category_Type.Contains(_val)).ToList();
        }


        /// <summary>
        /// 验证自定义类别是否存在
        /// </summary>
        /// <param name="_loginId">登陆账号id</param>
        /// <param name="_ty">类别名称</param>
        /// <returns>true代表数据库不存在该条数据；FALSE代表数据库存在该条数据</returns>
        public static bool IsContains(int _loginId, string _ty)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            //查询
            bool _bo = _db.Category.Where(a => a.ULogin_Id == _loginId && a.Category_Type == _ty).FirstOrDefault() == null;

            return _bo;
        }

        /// <summary>
        /// 验证自定义类别是否存在
        /// </summary>
        /// <param name="_loginId">登陆账号id</param>
        /// <param name="_ty">类别编号</param>
        /// <returns>true代表数据库不存在该条数据；FALSE代表数据库存在该条数据</returns>
        public static bool IsContains(int _loginId, int _cId)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            //查询
            bool _bo = _db.Category.Where(a => a.ULogin_Id == _loginId && a.Category_Id == _cId).FirstOrDefault() == null;

            return _bo;
        }


        /// <summary>
        /// 验证自定义类别是否存在
        /// </summary>
        /// <param name="_loginId">登陆账号id</param>
        /// <param name="_cId">类别编号</param>
        /// <param name="_ty">类别名称</param>
        /// <returns>true：可修改；FALSE：不可修改</returns>
        public static bool IsContains(int _loginId, int _cId, string _ty)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            //查询
            bool _bo = _db.Category.Where(a => a.ULogin_Id == _loginId && a.Category_Id == _cId).FirstOrDefault() == null;
            if (!_bo)
            {
                _bo = _db.Category.Where(a => a.ULogin_Id == _loginId && a.Category_Type == _ty).FirstOrDefault() == null;
            }

            return _bo;
        }

        /// <summary>
        /// 名称：添加自定义类别
        /// 时间：1-18
        /// 作者：
        /// </summary>
        /// <param name="_ca">要添加的自定义类别数据</param>
        /// <returns>新添加的自定义类别自增id</returns>
        public static int AddCategory(Category _ca)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            _db.Category.Add(_ca);

            //提交
            _db.SaveChanges();

            //返回自增id
            return _ca.Category_Id;
        }



        /// <summary>
        /// 名称：修改自定义类别
        /// 时间：1-18
        /// 作者：
        /// </summary>
        /// <param name="_ca">修改后的对象</param>
        /// <returns>修改后受影响的行id</returns>
        public static int UpdateCategory(Category _ca)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            var _upd = _db.Entry(_ca);

            //指定数据已更新
            _upd.State = System.Data.EntityState.Unchanged;

            //指定修改的字段
            _upd.Property("Category_Reception").IsModified = _ca.Category_Reception != null;
            _upd.Property("Category_Type").IsModified = !string.IsNullOrEmpty(_ca.Category_Type);

            return _db.SaveChanges();
        }



        /// <summary>
        /// 名称：删除自定义类别
        /// 时间：1-18
        /// 作者：
        /// </summary>
        /// <param name="_caId">要删除的自定义类别编号</param>
        /// <returns>删除后受影响的行</returns>
        public static int DeleteCategory(int _caId)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            //编辑要删除的数据
            Category _ca = new Category() { Category_Id = _caId };

            //指定要删除的自定义类别
            _db.Entry(_ca).State = System.Data.EntityState.Deleted;





            _db.Category.Remove(_ca);

            return _db.SaveChanges();
        }






        /// <summary>
        /// 名称：查询指定类别下的文章数量
        /// 时间：18-1-23
        /// 作者：
        /// </summary>
        /// <param name="_caId"></param>
        /// <returns></returns>
        public static int SelectChildCount(string _caId)
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            string[] _ids = _caId.Split(',');

            return _db.Write_blog.Where(a => _ids.Contains(a.Category_Id.ToString())).Count();

        }
    }
}