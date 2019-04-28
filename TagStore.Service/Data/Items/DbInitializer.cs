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
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static void SeedTestingData(ItemsContext context)
        {
            var item1 = createItem(context, new Item
            {
                ItemId = new Guid("00000000-0000-0000-0001-000000000001"),
                Name = "item1"
            });
            var tag1 = createTag(context, new Tag { Item = item1, TagId = "tag1", Order = 0, Value = "value1" });
            var tag2 = createTag(context, new Tag { Item = item1, TagId = "tag1", Order = 1, Value = "value2" });
            var tag3 = createTag(context, new Tag { Item = item1, TagId = "tag3", ValueLong = 3 });
            var tag4 = createTag(context, new Tag { Item = item1, TagId = "tag4", ValueDecimal = 4.5m });
            var tagType1 = createTagType(context, new TagType { TagId = "tag1", Type = "string" });
            var tagType4 = createTagType(context, new TagType { TagId = "tag4", Type = "decimal" });
            var tagTypeName1 = createTagTypeName(context, new TagTypeName { TagType = tagType1, Locale = "de", Name = "Meldung", Description = "Kundenmeldung" });
            var tagTypeName2 = createTagTypeName(context, new TagTypeName { TagType = tagType1, Locale = "en", Name = "Message", Description = "Custom Message" });
            context.SaveChanges();

            var item2 = createItem(context, new Item
            {
                ItemId = new Guid("00000000-0000-0000-0001-000000000002"),
                Name = "item2"
            });
            var tag7 = createTag(context, new Tag { Item = item2, TagId = "tag7", ValueDate = new DateTimeOffset(new DateTime(2019, 12, 31, 23, 59, 58), TimeSpan.FromHours(3.5)) });
            var tag8 = createTag(context, new Tag { Item = item2, TagId = "tag8", ValueDate = DateTimeOffset.Now });
            var tag9 = createTag(context, new Tag { Item = item2, TagId = "tag4", ValueDecimal = 44.55m });
            var tagType7 = createTagType(context, new TagType { TagId = "tag7", Type = "date" });
            var tagTypeName7 = createTagTypeName(context, new TagTypeName { TagType = tagType7, Locale = "de", Name = "Tag7", Description = "Tag Nummer 7" });
            context.SaveChanges();
        }

        static Item createItem(ItemsContext context, Item newItem)
        {
            if (null == context.Items.Find(newItem.ItemId)) context.Items.Add(newItem);
            return newItem;
        }
        static Tag createTag(ItemsContext context, Tag newTag)
        {
            if (null == context.Tags.Find(newTag.Item.ItemId, newTag.TagId, newTag.Order)) context.Tags.Add(newTag);
            return newTag;
        }
        static TagType createTagType(ItemsContext context, TagType newTagType)
        {
            if (null == context.TagTypes.Find(newTagType.TagId)) context.TagTypes.Add(newTagType);
            return newTagType;
        }
        static TagTypeName createTagTypeName(ItemsContext context, TagTypeName newTagTypeName)
        {
            if (null == context.TagTypeNames.Find(newTagTypeName.TagType.TagId, newTagTypeName.Locale)) context.TagTypeNames.Add(newTagTypeName);
            return newTagTypeName;
        }
    }
}
