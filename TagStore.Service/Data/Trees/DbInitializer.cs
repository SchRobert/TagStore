using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagStore.Service.Data.Trees
{
    public static class DbInitializer
    {
        public static void Initialize(TreesContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
