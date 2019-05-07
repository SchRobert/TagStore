using System;
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

        public static void SeedTestingData(ItemsContext ctx)
        {
            // Meta tag for preselection
            var mediaType = createTagType(ctx, new TagType { TagId = "MediaType", Type = TagDataType.String });
            createTagTypeName(ctx, new TagTypeName { TagType = mediaType, Locale = "en", Name = "Media Type" });
            createTagTypeName(ctx, new TagTypeName { TagType = mediaType, Locale = "de", Name = "Medien-Typ" });
            createTagTypeValue(ctx, new TagTypeValue { TagType = mediaType, Locale = "en", ValueString = "Music", Name = "Music" });
            createTagTypeValue(ctx, new TagTypeValue { TagType = mediaType, Locale = "de", ValueString = "Music", Name = "Musik" });
            createTagTypeValue(ctx, new TagTypeValue { TagType = mediaType, Locale = "en", ValueString = "Movie", Name = "Movie" });
            createTagTypeValue(ctx, new TagTypeValue { TagType = mediaType, Locale = "de", ValueString = "Movie", Name = "Film" });
            createTagTypeValue(ctx, new TagTypeValue { TagType = mediaType, Locale = "en", ValueString = "Series", Name = "TV Series" });
            createTagTypeValue(ctx, new TagTypeValue { TagType = mediaType, Locale = "de", ValueString = "Series", Name = "TV Serie" });
            createTagTypeValue(ctx, new TagTypeValue { TagType = mediaType, Locale = "en", ValueString = "Docu", Name = "Documentation" });
            createTagTypeValue(ctx, new TagTypeValue { TagType = mediaType, Locale = "de", ValueString = "Docu", Name = "Dokumentation" });

            // calculated tags:
            //  encodingAudio: wav, mp3, ...
            //  encodingVideo: h264, ...
            //  encodingContainer: avi, mkv, mpg, ...
            //  bpm: beats per minute
            //  lastVisited, totalVisited: (?)
            //  contentType: for files and packages

            // TODO: Identify trailers in a movie package?
            // TODO: Identify covers in a movie package or audio album (which is also just a package)

            // genre: Horror, Comedy, Musical, SciFi, Action, Adventure, SlowWaltz, VienneseWaltz, Tango, ...
            // rating: 0..5

            // Usually multiple tags for a single episode, movie or documentation
            var actorType = createTagType(ctx, new TagType { TagId = "Actor", Type = TagDataType.String });
            createTagTypeName(ctx, new TagTypeName { TagType = actorType, Locale = "en", Name = "Actor"        });
            createTagTypeName(ctx, new TagTypeName { TagType = actorType, Locale = "de", Name = "Schauspieler" });

            // TODO: for TV-Series the total length of all episodes or the length of one file
            var lengthType = createTagType(ctx, new TagType { TagId = "Length", Type = TagDataType.Time });
            createTagTypeName(ctx, new TagTypeName { TagType = lengthType, Locale = "en", Name = "Length" });
            createTagTypeName(ctx, new TagTypeName { TagType = lengthType, Locale = "de", Name = "Länge" });

            // TODO: can be the size of the stream or the size of the package
            var sizeType = createTagType(ctx, new TagType { TagId = "Size", Type = TagDataType.Long });
            createTagTypeName(ctx, new TagTypeName { TagType = sizeType, Locale = "en", Name = "Size in Bytes" });
            createTagTypeName(ctx, new TagTypeName { TagType = sizeType, Locale = "de", Name = "Größe in Bytes" });

            // TODO: can be the checksum of the stream or the checksum of the package
            var checksumMd5Type = createTagType(ctx, new TagType { TagId = "Checksum.MD5", Type = TagDataType.Long });
            createTagTypeName(ctx, new TagTypeName { TagType = checksumMd5Type, Locale = "en", Name = "MD5 Checksum" });
            createTagTypeName(ctx, new TagTypeName { TagType = checksumMd5Type, Locale = "de", Name = "MD5 Prüfsumme" });

            // TODO: can be the checksum of the stream or the checksum of the package
            var checksumSha1Type = createTagType(ctx, new TagType { TagId = "Checksum.SHA1", Type = TagDataType.Long });
            createTagTypeName(ctx, new TagTypeName { TagType = checksumMd5Type, Locale = "en", Name = "SHA1 Checksum" });
            createTagTypeName(ctx, new TagTypeName { TagType = checksumMd5Type, Locale = "de", Name = "SHA1 Prüfsumme" });

            var item1 = createItem(ctx, new Item
            {
                ItemId = new Guid("00000000-0000-0000-0001-000000000001"),
                Name = "item1"
            });
            var tag1 = createTag(ctx, new Tag { Item = item1, TagId = "tag1", Order = 0, ValueString = "value1" });
            var tag2 = createTag(ctx, new Tag { Item = item1, TagId = "tag1", Order = 1, ValueString = "value2" });
            var tag3 = createTag(ctx, new Tag { Item = item1, TagId = "tag3", ValueLong = 3 });
            var tag4 = createTag(ctx, new Tag { Item = item1, TagId = "tag4", ValueDecimal = 4.5m });
            var tagType1 = createTagType(ctx, new TagType { TagId = "tag1", Type = TagDataType.String });
            var tagType4 = createTagType(ctx, new TagType { TagId = "tag4", Type = TagDataType.Decimal });
            var tagTypeName1 = createTagTypeName(ctx, new TagTypeName { TagType = tagType1, Locale = "de", Name = "Meldung", Description = "Kundenmeldung" });
            var tagTypeName2 = createTagTypeName(ctx, new TagTypeName { TagType = tagType1, Locale = "en", Name = "Message", Description = "Custom Message" });
            ctx.SaveChanges();

            var item2 = createItem(ctx, new Item
            {
                ItemId = new Guid("00000000-0000-0000-0001-000000000002"),
                Name = "item2"
            });
            var tag7 = createTag(ctx, new Tag { Item = item2, TagId = "tag7", ValueDate = new DateTimeOffset(new DateTime(2019, 12, 31, 23, 59, 58), TimeSpan.FromHours(3.5)) });
            var tag8 = createTag(ctx, new Tag { Item = item2, TagId = "tag8", ValueDate = DateTimeOffset.Now });
            var tag9 = createTag(ctx, new Tag { Item = item2, TagId = "tag4", ValueDecimal = 44.55m });
            var tagType7 = createTagType(ctx, new TagType { TagId = "tag7", Type = TagDataType.DateTime });
            var tagTypeName7 = createTagTypeName(ctx, new TagTypeName { TagType = tagType7, Locale = "de", Name = "Tag7", Description = "Tag Nummer 7" });
            ctx.SaveChanges();
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
        static TagTypeValue createTagTypeValue(ItemsContext context, TagTypeValue newTagTypeValue)
        {
            if (null == context.TagTypeValues.Find(newTagTypeValue.TagType.TagId, newTagTypeValue.Locale, 
                newTagTypeValue.ValueString,
                newTagTypeValue.ValueLong,
                newTagTypeValue.ValueDecimal,
                newTagTypeValue.ValueDate)) context.TagTypeValues.Add(newTagTypeValue);
            return newTagTypeValue;
        }
    }
}
