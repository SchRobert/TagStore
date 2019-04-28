﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace TagStore.Service.Models.Items
{
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
        public string Type { get; set; }

        // navigation properties
        
        public virtual ICollection<TagTypeName> Names { get; set; }
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
}
