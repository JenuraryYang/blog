using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonkeyBlog.DTO
{
   
    /// <summary>
    /// 名称：后台类别管理
    /// 时间：1-18
    /// 作者：
    /// </summary>
    public class DTO_Backstage_Category
    {

        /// <summary>
        /// 名称：查询后台管理员所有分类
        /// 时间：1-18
        /// 作者：
        /// </summary>
        /// <returns>查询管理员所有分类</returns>
        public static List<Backstage_Category> SelectBackstage_Category()
        {
            //创建一个数据库上下文对象
            MonkeyDBEntities _db = new MonkeyDBEntities();

            return _db.Backstage_Category.ToList();
        }
    }
}