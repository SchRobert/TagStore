using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TagStore.Service.Models.Items
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
        // PK
        public Guid ItemId { get; set; }

        public ItemType Type { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The Tags assiciated to this item
        /// </summary>
        public virtual ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// The TagTypes for the assoiciated Tags
        ///  => This list is not handled by EF!
        /// </summary>
        public virtual ICollection<TagType> TagTypes { get; set; }
    }
}
