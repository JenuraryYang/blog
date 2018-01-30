using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonkeyBlog.DTO
{
    /// <summary>
    /// 请求返回的结果集
    /// </summary>
    public class DTO_Result
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string Result_Code { get; set; }

        /// <summary>
        /// 返回结果标题
        /// </summary>
        public string Result_Title { get; set; }


        public string value { get; set; }
    }
}