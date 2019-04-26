using System;
using System.Collections.Generic;

namespace TagStore.Service.Models.Trees
{
    public class TreeItem
    {
        public virtual Tree Tree { get; set; }

        public virtual TreeItem Parent { get; set; }

        public virtual ICollection<TreeItem> Childs { get; set; }

        public Guid ItemId { get; set; }
    }

}
