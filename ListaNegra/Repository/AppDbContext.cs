using ListaNegra.DALModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ListaNegra.Repository
{
    public interface IRepositoryContext
    {

        /// <summary>
        /// Commit data.
        /// </summary>
        void Save();
    }

    public class AppDbContext : DbContext, IRepositoryContext
    {
        public AppDbContext()
        {
           
        }

        public DbContext DbContext { get { return this; } }
        public DbSet<AspNetUser> AspNetUser { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<ExternalImages> ExternalImages { get; set; }
        public DbSet<PostTags> PostTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectString = ConfigurationManager.ConnectionStrings[2].ConnectionString;
            optionsBuilder
                .UseSqlServer(connectString, providerOptions => providerOptions.CommandTimeout(60))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public void Save()
        {
            this.SaveChanges();
        }
    }
}