using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        public DbSet<TagTypeValue> TagTypeValues { get; set; }

        const int itemNameLength = 256;
        const int tagIdLength = 256;
        const int tagValueLength = 2048;
        const int tagTypeLength = 64;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var itemEntity = modelBuilder.Entity<Item>();
            itemEntity.Property(_ => _.ItemId).HasColumnName("id"  ).IsRequired();
            itemEntity.Property(_ => _.Type  ).HasColumnName("type").IsRequired();
            itemEntity.Property(_ => _.Name  ).HasColumnName("name").HasMaxLength(itemNameLength).IsUnicode().IsRequired();
            itemEntity.HasKey  (_ => _.ItemId  ); // PK
            itemEntity.HasMany (x => x.Tags    ); // Childs
            itemEntity.Ignore  (_ => _.TagTypes);  // unbound Child!
            //itemEntity.HasMany(x => x.TagTypes); 

            var tagEntity = modelBuilder.Entity<Tag>();
            tagEntity.Property(_ => _.ItemId      ).HasColumnName("itemId").IsRequired();
            tagEntity.Property(_ => _.TagId       ).HasColumnName("id"    ).IsTagId(tagIdLength);
            tagEntity.Property(_ => _.Order       ).HasColumnName("order" ).IsRequired();
            tagEntity.Property(_ => _.Source      ).HasColumnName("src"   );
            tagEntity.initializeTagValue(tagValueLength);
            tagEntity.HasKey(_ => new { _.ItemId, _.TagId, _.Order}); // PK
            tagEntity.HasOne(_ => _.Item ); // FK to Item

            var tagTypeEntity = modelBuilder.Entity<TagType>();
            tagTypeEntity.Property(_ => _.TagId).HasColumnName("tagId").IsTagId(tagIdLength);
            tagTypeEntity.Property(_ => _.Type ).HasColumnName("type" ).HasMaxLength(tagTypeLength).IsRequired();
            tagTypeEntity.HasKey  (_ => _.TagId); // PK
            tagTypeEntity.HasMany (_ => _.Names); // Childs

            var tagTypeNameEntity = modelBuilder.Entity<TagTypeName>();
            tagTypeNameEntity.Property(_ => _.TagId      ).HasColumnName("tagId" ).IsTagId(tagIdLength);            
            tagTypeNameEntity.Property(_ => _.Locale     ).HasColumnName("locale").HasMaxLength(10).IsUnicode(false); // https://en.wikipedia.org/wiki/Language_code
            tagTypeNameEntity.Property(_ => _.Name       ).HasColumnName("name"  ).HasMaxLength(256).IsUnicode().IsRequired();
            tagTypeNameEntity.Property(_ => _.Description).HasColumnName("desc"  ).HasMaxLength(2048).IsUnicode();

            tagTypeNameEntity.HasKey(_ => new { _.TagId, _.Locale }); // PK
            tagTypeNameEntity.HasOne(_ => _.TagType                ); // FK to TagType

            var tagTypeValueEntity = modelBuilder.Entity<TagTypeValue>();
            tagTypeValueEntity.Property(_ => _.TagId      ).HasColumnName("tagId" ).IsTagId(tagIdLength);            
            tagTypeValueEntity.Property(_ => _.Locale     ).HasColumnName("locale").HasMaxLength(10).IsUnicode(false); // https://en.wikipedia.org/wiki/Language_code
            tagTypeValueEntity.Property(_ => _.Name       ).HasColumnName("name"  ).HasMaxLength(256).IsUnicode().IsRequired();
            tagTypeValueEntity.Property(_ => _.Description).HasColumnName("desc"  ).HasMaxLength(2048).IsUnicode();
            tagTypeValueEntity.initializeTagValue(tagValueLength);
            tagTypeValueEntity.HasKey(_ => new { _.TagId, _.Locale, _.Id }); // PK
            tagTypeValueEntity.HasOne(_ => _.TagType                ); // FK to TagType
        }
    }

    public static class ItemsContextExtensions
    {
        public static PropertyBuilder<TProperty> IsTagId<TProperty>(this PropertyBuilder<TProperty> b, int tagNameLength)
            => b.HasMaxLength(tagNameLength).IsUnicode(true).IsRequired();

        public static EntityTypeBuilder<T> initializeTagValue<T>(this EntityTypeBuilder<T> entityTypeBuilder, int tagValueLength) where T : TagValue
        {
            entityTypeBuilder.Property(_ => _.ValueString).HasColumnName("value").HasMaxLength(tagValueLength);
            entityTypeBuilder.Property(_ => _.ValueLong).HasColumnName("valueLong");
            entityTypeBuilder.Property(_ => _.ValueDecimal).HasColumnName("valueDec");
            entityTypeBuilder.Property(_ => _.ValueDate).HasColumnName("valueDate");
            return entityTypeBuilder;
        }

    }
}
