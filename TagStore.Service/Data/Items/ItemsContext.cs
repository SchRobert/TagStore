using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using TagStore.Service.Models.Items;

namespace TagStore.Service.Data.Items
{
    public class ItemsContext : DbContext
    {
        public ItemsContext(DbContextOptions<ItemsContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<Item> Items { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagType> TagTypes { get; set; }
        public DbSet<TagTypeName> TagTypeNames { get; set; }

        const int itemNameLength = 256;
        const int tagIdLength = 256;
        const int tagValueLength = 2048;
        const int tagTypeLength = 64;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().Property(_ => _.ItemId).HasColumnName("id"  ).IsRequired();
            modelBuilder.Entity<Item>().Property(_ => _.Type  ).HasColumnName("type").IsRequired();
            modelBuilder.Entity<Item>().Property(_ => _.Name  ).HasColumnName("name").HasMaxLength(itemNameLength).IsUnicode().IsRequired();

            modelBuilder.Entity<Item>().HasKey (_ => _.ItemId  ); // PK
            modelBuilder.Entity<Item>().HasMany(x => x.Tags    ); // Childs
            modelBuilder.Entity<Item>().Ignore(_ => _.TagTypes);// unbound Child!
          //modelBuilder.Entity<Item>().HasMany(x => x.TagTypes); 

            modelBuilder.Entity<Tag>().Property(_ => _.ItemId      ).HasColumnName("itemId"   ).IsRequired();
            modelBuilder.Entity<Tag>().Property(_ => _.TagId       ).HasColumnName("id"       ).IsTagId(tagIdLength);
            modelBuilder.Entity<Tag>().Property(_ => _.Order       ).HasColumnName("order"    ).IsRequired();
            modelBuilder.Entity<Tag>().Property(_ => _.Value       ).HasColumnName("value"    ).HasMaxLength(tagValueLength);
            modelBuilder.Entity<Tag>().Property(_ => _.ValueLong   ).HasColumnName("valueLong");
            modelBuilder.Entity<Tag>().Property(_ => _.ValueDecimal).HasColumnName("valueDec" );
            modelBuilder.Entity<Tag>().Property(_ => _.ValueDate   ).HasColumnName("valueDate");
            modelBuilder.Entity<Tag>().Property(_ => _.Virtual     ).HasColumnName("virtual"  ).IsRequired();

            modelBuilder.Entity<Tag>().HasKey(_ => new { _.ItemId, _.TagId, _.Order}); // PK
            modelBuilder.Entity<Tag>().HasOne(_ => _.Item ); // FK to Item

            modelBuilder.Entity<TagType>().Property(_ => _.TagId).HasColumnName("tagId").IsTagId(tagIdLength);
            modelBuilder.Entity<TagType>().Property(_ => _.Type ).HasColumnName("type" ).HasMaxLength(tagTypeLength).IsRequired();

            modelBuilder.Entity<TagType>().HasKey (_ => _.TagId); // PK
            modelBuilder.Entity<TagType>().HasMany(_ => _.Names); // Childs

            var tagTypeNameEntity = modelBuilder.Entity<TagTypeName>();
            tagTypeNameEntity.Property(_ => _.TagId      ).HasColumnName("tagId" ).IsTagId(tagIdLength);            
            tagTypeNameEntity.Property(_ => _.Locale     ).HasColumnName("locale").HasMaxLength(10).IsUnicode(false); // https://en.wikipedia.org/wiki/Language_code
            tagTypeNameEntity.Property(_ => _.Name       ).HasColumnName("name"  ).HasMaxLength(256).IsUnicode().IsRequired();
            tagTypeNameEntity.Property(_ => _.Description).HasColumnName("desc"  ).HasMaxLength(2048).IsUnicode();

            tagTypeNameEntity.HasKey(_ => new { _.TagId, _.Locale }); // PK
            tagTypeNameEntity.HasOne(_ => _.TagType                ); // FK to TagType
        }
    }

    public static class ItemsContextExtensions
    {
        public static PropertyBuilder<TProperty> IsTagId<TProperty>(this PropertyBuilder<TProperty> b, int tagNameLength)
            => b.HasMaxLength(tagNameLength).IsUnicode(true).IsRequired();
    }
}
