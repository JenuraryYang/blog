﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MonkeyDBEntities : DbContext
    {
        public MonkeyDBEntities()
            : base("name=MonkeyDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AdminUser> AdminUser { get; set; }
        public DbSet<Auditing_Blog> Auditing_Blog { get; set; }
        public DbSet<Backstage_Category> Backstage_Category { get; set; }
        public DbSet<Carousel_figure> Carousel_figure { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<FollowUser> FollowUser { get; set; }
        public DbSet<KeyWord> KeyWord { get; set; }
        public DbSet<Special> Special { get; set; }
        public DbSet<ULogin> ULogin { get; set; }
        public DbSet<Users_Details> Users_Details { get; set; }
        public DbSet<Write_blog> Write_blog { get; set; }
        public DbSet<Blog_Comment> Blog_Comment { get; set; }
    }
}