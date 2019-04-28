using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TagStore.Service.Data.Items;
using TagStore.Service.Models.Items;

namespace TagStore.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagTypesController : ControllerBase
    {
        readonly IItemsRepository _repository;

        public TagTypesController(IItemsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/TagTypes
        // GET: api/TagTypes?includeNames=true
        /// <summary>
        /// Returns all defined TagTypes.
        /// </summary>
        /// <param name="includeNames">If true return also all localized names for each tag.</param>
        /// <param name="cancellationToken">Optional token to cancel the operation</param>
        /// <returns></returns>
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TagType[]))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<TagType[]>> GetTagTypes([FromQuery] bool includeNames = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tagTypes = await _repository.FindTagTypes(includeNames).ToArrayAsync(cancellationToken);

            if (true == tagTypes?.Any())
                return Ok(tagTypes);

            return NoContent();
        }

        // GET: api/TagTypes
        // GET: api/TagTypes?includeNames=true
        /// <summary>
        /// Returns all defined TagTypes for a specified ItemId.
        /// 
        /// IMPROVE: Return 404 if the Item was not found and 204 if the item was found bus has no tags or the tags have no types.
        /// </summary>
        /// <param name="includeNames">If true return also all localized names for each tag.</param>
        /// <param name="cancellationToken">Optional token to cancel the operation</param>
        /// <returns></returns>
        [HttpGet("{itemId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TagType[]))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<TagType[]>> GetTagTypes(Guid itemId, [FromQuery] bool includeNames = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tagTypes = await _repository.FindTagTypes(itemId, includeNames).ToArrayAsync(cancellationToken);

            if (true == tagTypes?.Any())
                return Ok(tagTypes);

            return NoContent();
        }

        // PUT: api/Items/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateItem(Guid id, Item item)
        //{
        //    if (id != item.ItemId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(item).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ItemExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Items
        //[HttpPost]
        //public async Task<ActionResult<Item>> AddItem(Item item)
        //{
        //    _context.Items.Add(item);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetItem", new { id = item.ItemId }, item);
        //}

        // DELETE: api/Items/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Item>> DeleteItem(Guid id)
        //{
        //    var item = await _context.Items.FindAsync(id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Items.Remove(item);
        //    await _context.SaveChangesAsync();

        //    return item;
        //}

        //private bool ItemExists(Guid id)
        //{
        //    return _context.Items.Any(e => e.ItemId == id);
        //}
    }
}
