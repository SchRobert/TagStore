using Newtonsoft.Json;
using System.Collections.Generic;

namespace TagStore.Service.Models.Items
{
    public enum TagDataType
    {
        /// <summary>Specifies that the value can be stored in any property</summary>
        Undefined,
        /// <summary>Specifies that the value is stored in the Value property</summary>
        String,
        /// <summary>Specifies that the value is stored in the ValueLong property</summary>
        Long,
        /// <summary>Specifies that the value is stored in the ValueDecimal property</summary>
        Decimal,
        /// <summary>Specifies that the value is stored in the ValueDate property but only the date-part is used</summary>
        Date,
        /// <summary>Specifies that the value is stored in the ValueDate property but only the time part is used</summary>
        Time,
        /// <summary>Specifies that the value is stored in the ValueDate property</summary>
        DateTime
    }

    /// <summary>
    /// One TagType can be specified for a TagId
    ///  - every TagType has none or more TagTypeNames
    /// </summary>
    public class TagType
    {
        // TagId is the primary key
        public string TagId { get; set; }

        /// <summary>
        /// Data type for tags of this id
        /// </summary>
        public TagDataType Type { get; set; }

        // navigation properties
        
        // Available localized names for tags with this TagId
        public virtual ICollection<TagTypeName> Names { get; set; }

        // Available localized names for values of tags with this TagId
        public virtual ICollection<TagTypeValue> Values { get; set; }
    }

    public class TagTypeName
    {
        // TagId + Locate is the primary key
        public string TagId { get; set; }
        public string Locale { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }


        // navigation properties

        /// <summary>
        /// Reference back to the associated TagType.
        /// 
        /// NOTE: This field is not serialized to avoid a JsonSerializationException(Self referencing loop detected) 
        ///       when serializing Items with Tags
        /// </summary>
        [JsonIgnore]
        public virtual TagType TagType { get; set; }
    }

    public class TagTypeValue : TagValue
    {
        // TagId + Locate is the primary key
        public string TagId { get; set; }
        public string Locale { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }


        // navigation properties

        /// <summary>
        /// Reference back to the associated TagType.
        /// 
        /// NOTE: This field is not serialized to avoid a JsonSerializationException(Self referencing loop detected) 
        ///       when serializing Items with Tags
        /// </summary>
        [JsonIgnore]
        public virtual TagType TagType { get; set; }
    }
}
