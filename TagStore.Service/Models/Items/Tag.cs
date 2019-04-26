using System;

namespace TagStore.Service.Models.Items
{
    /// <summary>
    /// Every Item has it's own Tags.
    /// If you need multiple Tags with the same TagId for one Item you have to specify different order values for each Tag
    /// 
    /// For all Tags of the same order there may exist one TagType.
    /// </summary>
    public class Tag
    {
        // PK ItemId / TagId / Order
        public Guid ItemId { get; set; }
        public string TagId { get; set; }
        public int Order { get; set; }

        /// <summary>
        /// Current value of this Tag (can be string, int, float, ...)
        /// </summary>
        public string Value { get; set; }
        public long ValueLong { get; set; }
        public decimal ValueDecimal { get; set; }
        public DateTimeOffset ValueDate { get; set; }

        /// <summary>
        /// True if this tag is calculated by a parent object
        /// </summary>
        public bool Virtual { get; set; }

        // navigation property
        public virtual Item Item { get; set; }
    }
}
