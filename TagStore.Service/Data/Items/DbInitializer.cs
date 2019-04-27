using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TagStore.Service.Models.Items;

namespace TagStore.Service.Data.Items
{
    public static class DbInitializer
    {
        public static void Initialize(ItemsContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static void SeedTestingData(ItemsContext context)
        {
            var item1 = new Item {
                ItemId = new Guid("00000000-0000-0000-0001-000000000001"),
                Name = "item1"                
            };
            var item2 = new Item
            {
                ItemId = new Guid("00000000-0000-0000-0001-000000000002"),
                Name = "item2"
            };
            var tag1 = new Tag { Item = item1, TagId = "tag1", Order = 0, Value = "value1" };
            var tag2 = new Tag { Item = item1, TagId = "tag1", Order = 1, Value = "value2" };
            var tag3 = new Tag { Item = item1, TagId = "tag3", ValueLong = 3 };
            var tag4 = new Tag { Item = item1, TagId = "tag4", ValueDecimal = 4.5m };
            var tagType1 = new TagType { TagId = "tag1", Type = "string" };
            var tagType3 = new TagType { TagId = "tag3", Type = "long" };
            var tagTypeName1 = new TagTypeName { TagType = tagType1, Locale = "de", Name = "Day1", Description = "Tag des Monats" };
            var tagTypeName2 = new TagTypeName { TagType = tagType1, Locale = "en", Name = "Tag1", Description = "Day of the Month" };

            if (null == context.Items.Find(item1.ItemId)) context.Items.Add(item1);
            if (null == context.Items.Find(item2.ItemId)) context.Items.Add(item2);
            context.SaveChanges();
            if (null == context.Tags.Find(item1.ItemId, tag1.TagId, 0)) context.Tags.Add(tag1);
            if (null == context.Tags.Find(item1.ItemId, tag2.TagId, 1)) context.Tags.Add(tag2);
            if (null == context.Tags.Find(item1.ItemId, tag3.TagId, 0)) context.Tags.Add(tag3);
            if (null == context.Tags.Find(item1.ItemId, tag4.TagId, 0)) context.Tags.Add(tag4);
            context.SaveChanges();
            if (null == context.TagTypes.Find(tagType1.TagId)) context.TagTypes.Add(tagType1);
            if (null == context.TagTypes.Find(tagType3.TagId)) context.TagTypes.Add(tagType3);
            context.SaveChanges();
            if (null == context.TagTypeNames.Find(tagType1.TagId, tagTypeName1.Locale)) context.TagTypeNames.Add(tagTypeName1);
            if (null == context.TagTypeNames.Find(tagType1.TagId, tagTypeName2.Locale)) context.TagTypeNames.Add(tagTypeName2);
            context.SaveChanges();
        }
    }
}
