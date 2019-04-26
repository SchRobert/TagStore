using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagStore.Service.Data
{
    public class Tree
    {
        public Guid TreeId { get; set; }
        public string Name { get; set; }
        public TreeItem Root { get; set; }
        public string Location{ get; set; }
    }

    public class TreeItem
    {
        public virtual Tree Tree { get; set; }
        public virtual TreeItem Parent { get; set; }
        public virtual ICollection<TreeItem> Childs { get; set; }
        public Guid ItemId { get; set; }
    }

}
