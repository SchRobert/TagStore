using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TagStore.Service.Data
{
    public class ItemDbContext : DbContext
    {
        //public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
        //{
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

        public ICollection<Item> Items { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<TagType> TagTypes { get; set; }
        public ICollection<TagTypeName> TagTypeNames { get; set; }
    }
}
