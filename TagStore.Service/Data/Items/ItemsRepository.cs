using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TagStore.Service.Models.Items;

namespace TagStore.Service.Data.Items
{
    public interface IItemsRepository
    {
        /// <summary>
        /// Returns the specified Item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task<Item> GetItemAsync(Guid itemId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns all Tags attaches to the specified Item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        IQueryable<Tag> GetTags(Guid itemId);

        /// <summary>
        /// Returns the TagTypes for all Tags attaches to the specified Item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        IQueryable<TagType> GetTagTypes(Guid itemId);

        /// <summary>
        /// Returns all TagTypeNames for all Tags of the specified Item
        /// 
        //  NOTE: Always returns the values for all available locales!
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        IQueryable<TagTypeName> GetTagTypeName(Guid itemId);
    }

    public class ItemsRepository : IItemsRepository
    {
        readonly ItemsContext _context;

        public ItemsRepository(ItemsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the specified Item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Task<Item> GetItemAsync(Guid itemId, CancellationToken cancellationToken = default(CancellationToken))
            => _context.Items.FindAsync(itemId, cancellationToken);

        /// <summary>
        /// Returns all Tags attaches to the specified Item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public IQueryable<Tag> GetTags(Guid itemId) 
            => _context.Tags.Where(_ => _.ItemId == itemId);

        IQueryable<string> GetTagsIds(Guid itemId) => GetTags(itemId).Select(_ => _.TagId);

        /// <summary>
        /// Returns the TagTypes for all Tags attaches to the specified Item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public IQueryable<TagType> GetTagTypes(Guid itemId) 
            => _context.TagTypes.Where(_ => GetTagsIds(itemId).Contains(_.TagId));

        /// <summary>
        /// Returns all TagTypeNames for all Tags of the specified Item
        /// 
        //  NOTE: Always returns the values for all available locales!
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public IQueryable<TagTypeName> GetTagTypeName(Guid itemId) 
            => _context.TagTypeNames.Where(_ => GetTagsIds(itemId).Contains(_.TagId));
    }
}
