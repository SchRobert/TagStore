using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TagStore.Service.Data
{
    public class TreeDbContext : DbContext
    {
        //public TagsDbContext(DbContextOptions<TagsDbContext> options) : base(options)
        //{
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

        public ICollection<Tree> Trees { get; set; }
        public ICollection<TreeItem> TreeItems { get; set; }
    }
}
