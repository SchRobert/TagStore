using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TagStore.Service.Models.Items;

namespace TagStore.Service.Data.Items
{
    public interface IItemsRepository
    {
        //ItemsContext Context { get; } // for development only

        /// <summary>
        /// Returns an IQueryable of all available Items
        /// </summary>
        /// <param name="includeTags">If true return also all available Tags for each item.</param>
        /// <returns></returns>
        IQueryable<Item> FindItems(bool includeTags = false);

        IQueryable<Item> FindItems(ItemsQuery query, bool includeTags = false);

        /// <summary>
        /// Returns an IQueryable of all available TagTypes
        /// </summary>
        ///// <param name="includeNames">If true return also all available Names for each TagType.</param>
        /// <returns></returns>
        IQueryable<TagType> FindTagTypes(bool includeNames = false);

        /// <summary>
        /// Returns an IQueryable of all available TagTypes for the Tags of the given Item
        /// </summary>
        ///// <param name="includeNames">If true return also all available Names for each TagType.</param>
        /// <returns></returns>
        IQueryable<TagType> FindTagTypes(Guid itemId, bool includeNames = false);
    }

    public class ItemsRepository : IItemsRepository
    {
        //public ItemsContext Context => _context;
        readonly ItemsContext _context;

        public ItemsRepository(ItemsContext context)
        {
            _context = context;
        }

        public IQueryable<Item> FindItems(bool includeTags = false)
        {
            return includeTags
                ? _context.Items.Include("Tags")
                : _context.Items;
        }

        public IQueryable<Item> FindItems(ItemsQuery query, bool includeTags = false)
        {
            var items = FindItems(includeTags);

            if (query != null)
                items = applyQuery(query, items);
            //if (query is AndQuery and)
            //    foreach (var a in and.And)
            //        items = items.Where(i => i.Tags.Any(t => t.TagId == tagEquals.TagId && t.Value == tagEquals.Value));
            return items;
        }

        IQueryable<Item> applyQuery(ItemsQuery query, IQueryable<Item> items)
        {
#if true
            switch (query.Op)
            {
                case ItemsQuery.Operator.EXISTS:
                case ItemsQuery.Operator.EQUALS:
                    return items.Where(i => i.Tags.Any(hasTag(query)));

                case ItemsQuery.Operator.AND:
                    // TODO: Find a generic way to AND n-Items
                    switch (query.Items?.Length ?? 0)
                    {
                        case 0:
                            return items;
                        case 1:
                            return items.Where(i => i.Tags.Any(hasTag(query.Items[0])));
                        case 2:
                            return items.Where(i => i.Tags.Any(hasTag(query.Items[0])) && i.Tags.Any(hasTag(query.Items[1])));
                        case 3:
                            return items.Where(i => i.Tags.Any(hasTag(query.Items[0])) && i.Tags.Any(hasTag(query.Items[1])) && i.Tags.Any(hasTag(query.Items[2])));
                    }
                    throw new NotImplementedException($"Currently AND-queries only with [0..3] items allowed but got: {query.Items?.Length}");

                case ItemsQuery.Operator.OR:
                    // TODO: Find a generic way to OR n-Items
                    switch (query.Items?.Length ?? 0)
                    {
                        case 0:
                            return items;
                        case 1:
                        return items.Where(i => i.Tags.Any(hasTag(query.Items[0])));
                        case 2:
                        return items.Where(i => i.Tags.Any(hasTag(query.Items[0])) || i.Tags.Any(hasTag(query.Items[1])));
                        case 3:
                        return items.Where(i => i.Tags.Any(hasTag(query.Items[0])) || i.Tags.Any(hasTag(query.Items[1])) || i.Tags.Any(hasTag(query.Items[2])));
                    }
                    throw new NotImplementedException($"Currently AND-queries only with [0..3] items allowed but got: {query.Items?.Length}");

                default:
                    throw new InvalidOperationException($"Unknown operator '{query.Op}'");
            }
#else
            if (query is ExistsQuery exists)
                return items.Where(i => i.Tags.Any(t => t.TagId == exists.TagId));

            if (query is EqualsQuery equals)
            {
                if (equals.ValueLong.HasValue)
                    return items.Where(i => i.Tags.Any(t => t.TagId == equals.TagId && t.ValueLong == equals.ValueLong.Value));
                if (equals.ValueDecimal.HasValue)
                    return items.Where(i => i.Tags.Any(t => t.TagId == equals.TagId && t.ValueDecimal == equals.ValueDecimal.Value));
                if (equals.ValueDate.HasValue)
                    return items.Where(i => i.Tags.Any(t => t.TagId == equals.TagId && t.ValueDate == equals.ValueDate.Value));

                return items.Where(i => i.Tags.Any(t => t.TagId == equals.TagId && t.Value == equals.Value));
            }

#endif
        }

        private static Func<Tag, bool> hasTag(ItemsQuery query)
        {
            switch (query.Op)
            {
                case ItemsQuery.Operator.EXISTS:
                    return t => t.TagId == query.TagId;

                case ItemsQuery.Operator.EQUALS:
                    if (query.ValueLong.HasValue)
                        return t => t.TagId == query.TagId && t.ValueLong == query.ValueLong.Value;

                    if (query.ValueDecimal.HasValue)
                        return t => t.TagId == query.TagId && t.ValueDecimal == query.ValueDecimal.Value;

                    if (query.ValueDate.HasValue)
                        return t => t.TagId == query.TagId && t.ValueDate == query.ValueDate.Value;

                    return t => t.TagId == query.TagId && t.Value == query.Value;

                default: // should never happended: otehr types are handled by the caller
                    throw new InvalidOperationException();
            }
        }

        public IQueryable<TagType> FindTagTypes(bool includeNames = false)
        {
            return includeNames
                ? _context.TagTypes.Include("Names")
                : _context.TagTypes;
        }

        public IQueryable<TagType> FindTagTypes(Guid itemId, bool includeNames = false)
        {
            var tagIds = _context.Tags
                .Where(_ => _.ItemId == itemId)
                .Select(_ => _.TagId);

            return FindTagTypes(includeNames)
                .Where(_ => tagIds.Contains(_.TagId));
        }
    }
}
