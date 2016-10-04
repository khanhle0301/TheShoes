﻿using Microsoft.AspNet.Identity.EntityFramework;
using MyShop.Model.Models;
using System.Data.Entity;

namespace MyShop.Data
{
    public class MyShopDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyShopDbContext() : base("MyShopConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Footer> Footers { set; get; }
        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderDetail> OrderDetails { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Post> Posts { set; get; }
        public DbSet<PostCategory> PostCategories { set; get; }
        public DbSet<PostTag> PostTags { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }
        public DbSet<Slide> Slides { set; get; }
        public DbSet<Tag> Tags { set; get; }
        public DbSet<Error> Errors { set; get; }
        public DbSet<ContactDetail> ContactDetails { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }
        public DbSet<Banner> Banners { set; get; }
        public DbSet<Size> Sizes { set; get; }
        public DbSet<ProductSize> ProductSizes { set; get; }
        public DbSet<Vendor> Vendors { set; get; }
        public DbSet<Color> Colors { set; get; }
        public DbSet<ProductColor> ProductColors { set; get; }

        public static MyShopDbContext Create()
        {
            return new MyShopDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId });
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId);
        }
    }
}