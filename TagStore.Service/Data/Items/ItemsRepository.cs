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
