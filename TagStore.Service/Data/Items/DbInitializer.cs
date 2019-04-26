using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagStore.Service.Data.Items
{
    public static class DbInitializer
    {
        public static void Initialize(ItemsContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
