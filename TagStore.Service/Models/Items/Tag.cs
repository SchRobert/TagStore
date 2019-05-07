using Newtonsoft.Json;
using System;
using System.Globalization;

namespace TagStore.Service.Models.Items
{
    /// <summary>
    /// Every Item has it's own Tags.
    /// If you need multiple Tags with the same TagId for one Item you have to specify different order values for each Tag
    /// 
    /// For all Tags of the same order there may exist one TagType.
    /// </summary>
    public class Tag : TagValue
    {
        // PK ItemId / TagId / Order
        [JsonIgnore]
        public Guid ItemId { get; set; }
        public string TagId { get; set; }
        public int Order { get; set; }

        /// <summary>
        /// If set, this tag is calculated by a parent object
        /// </summary>
        public Guid? Source { get; set; }

        // navigation property

        /// <summary>
        /// Reference back to the associated Item.
        /// 
        /// NOTE: This field is not serialized to avoid a JsonSerializationException(Self referencing loop detected) 
        ///       when serializing Items with Tags
        /// </summary>
        [JsonIgnore]
        public virtual Item Item { get; set; }
    }
}
