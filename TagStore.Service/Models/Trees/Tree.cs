using System;

namespace TagStore.Service.Models.Trees
{
    public class Tree
    {
        /// <summary>
        /// Identifier for this tree.
        /// </summary>
        public Guid TreeId { get; set; }

        /// <summary>
        /// Name of this tree.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Physical location (Root-Path)
        /// </summary>
        public string Location { get; set; }

        public virtual TreeItem Root { get; set; }
    }
}
