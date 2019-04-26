using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TagStore.Service.Models.Trees;

namespace TagStore.Service.Data.Trees
{
    public class TreesContext : DbContext
    {
        public TreesContext(DbContextOptions<TreesContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<Tree> Trees { get; set; }
        public DbSet<TreeItem> TreeItems { get; set; }
    }
}
