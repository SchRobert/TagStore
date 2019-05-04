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
    public class ItemsController : ControllerBase
    {
        readonly IItemsRepository _repository;

        public ItemsController(IItemsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Items
        // GET: api/Items?includeTags=true
        /// <summary>
        /// Returns the found items.
        /// </summary>
        /// <param name="includeTags">If true return also all available Tags for each item.</param>
        /// <param name="cancellationToken">Optional token to cancel the operation</param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(Item[]))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Item[]>> GetItems([FromQuery] bool includeTags = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await _repository.FindItems(includeTags).ToArrayAsync(cancellationToken);

            if (true == items?.Any())
                return Ok(items);

            return NoContent();
        }

        // GET: api/Items/00000000-0000-0000-0001-000000000001
        // GET: api/Items/00000000-0000-0000-0001-000000000001?includeTags=true
        /// <summary>
        /// Returns a specific item.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="includeTags">If true return also all available Tags for each item.</param>
        /// <param name="cancellationToken">Optional token to cancel the operation</param>
        /// <returns></returns>
        [HttpGet("{itemId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(Item))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Item>> GetItem(Guid itemId, [FromQuery] bool includeTags = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var item = await _repository.FindItems(includeTags).SingleOrDefaultAsync(_ => _.ItemId == itemId, cancellationToken);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        /// <summary>
        /// Returns items matching the tags query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includeTags">If true return also all available Tags for each item.</param>
        /// <param name="cancellationToken">Optional token to cancel the operation</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Item))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost()]
        public async Task<ActionResult<Item[]>> Find([FromBody] ItemsQuery query, [FromQuery] bool includeTags = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await _repository.FindItems(query, includeTags).ToArrayAsync(cancellationToken);

            if (true == items?.Any())
                return Ok(items);

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
