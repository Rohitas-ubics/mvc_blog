﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Security
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class mvc_blog_dbEntities : DbContext
    {
        public mvc_blog_dbEntities()
            : base("name=mvc_blog_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Blog_Tag> Blog_Tag { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tag_master> Tag_master { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
