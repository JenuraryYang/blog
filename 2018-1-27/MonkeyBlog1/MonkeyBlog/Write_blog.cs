//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonkeyBlog
{
    using System;
    using System.Collections.Generic;
    
    public partial class Write_blog
    {
        public int Write_blog_Id { get; set; }
        public string Write_Title { get; set; }
        public Nullable<int> Write_ArticleType { get; set; }
        public string Write_Content { get; set; }
        public Nullable<int> Backstage_Category_Id { get; set; }
        public string Write_Label { get; set; }
        public string Category_Id { get; set; }
        public Nullable<int> Write_Private { get; set; }
        public Nullable<int> ULogin_Id { get; set; }
        public Nullable<System.DateTime> Write_Createdate { get; set; }
        public Nullable<System.DateTime> Write_Deletedate { get; set; }
        public Nullable<int> Write_State { get; set; }
        public Nullable<int> Write_IsDel { get; set; }
        public Nullable<int> Write_ReadCount { get; set; }
        public Nullable<int> Write_ZanCount { get; set; }
        public Nullable<int> Write_Collection { get; set; }
        public Nullable<int> Blog_Recommend { get; set; }
        public string ULogin_IdIsManager { get; set; }
    }
}
