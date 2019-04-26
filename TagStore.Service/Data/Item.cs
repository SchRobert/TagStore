using System;
using System.Collections.Generic;

namespace TagStore.Service.Data
{
    public enum ItemType : byte
    {
        /// <summary>
        /// Specifies this item is a single file (with a single content)
        /// </summary>
        File,
        /// <summary>
        /// Specifies this item is a package (archive, ...)
        /// </summary>
        Package,
        /// <summary>
        /// Specifies this item is a folder (without any content)
        /// </summary>
        Folder
    }

    public class Item
    {
        public Guid ItemId { get; set; }
        public ItemType Type { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// The Tags assiciated to this item
        /// </summary>
        public virtual ICollection<Tag> Tags { get; set; }
        /// <summary>
        /// The TagTypes for the assiciated Tags
        /// </summary>
        public virtual ICollection<TagType> TagTypes { get; set; }
    }

    public class Tag
    {
        public virtual Item Item { get; set; }
        public string TagId { get; set; }
        /// <summary>
        /// Current value of this Tag (can be string, int, float, ...)
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// True if this tag is calculated by a parent object
        /// </summary>
        public bool Virtual { get; set; }
    }

    public class TagType
    {
        public string TagId { get; set; }
        /// <summary>
        /// Data type for tags of this id
        /// </summary>
        public string Type { get; set; }
        public virtual ICollection<TagTypeName> Names { get; set; }
    }

    public class TagTypeName
    {
        public virtual TagType TagType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Locale { get; set; }
    }
}
