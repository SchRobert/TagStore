using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagStore.Service.Models.Items
{
#if true
    public class ItemsQuery
    {
        // TODO: Add and implement more operators: BETWEEN, CONTAINS, STARTSWITH, ENDSWITH, ...
        public enum Operator { AND, OR, EXISTS, EQUALS }
        public Operator Op { get; set; }
        public ItemsQuery[] Items { get; set; }
        public string TagId { get; set; }
        public string Value { get; set; }
        public long? ValueLong { get; set; }
        public decimal? ValueDecimal { get; set; }
        public DateTimeOffset? ValueDate { get; set; }

        public static ItemsQuery AND(params ItemsQuery[] items) => new ItemsQuery { Op = Operator.AND, Items = items };
        public static ItemsQuery OR(params ItemsQuery[] items) => new ItemsQuery { Op = Operator.OR, Items = items };
        public static ItemsQuery EXISTS(string tagId) => new ItemsQuery { Op = Operator.EXISTS, TagId = tagId };
        public static ItemsQuery EQUALS(string tagId, string value) => new ItemsQuery { Op = Operator.EQUALS, TagId = tagId, Value = value };
        public static ItemsQuery EQUALS(string tagId, long value) => new ItemsQuery { Op = Operator.EQUALS, TagId = tagId, ValueLong = value };
        public static ItemsQuery EQUALS(string tagId, decimal value) => new ItemsQuery { Op = Operator.EQUALS, TagId = tagId, ValueDecimal = value };
        public static ItemsQuery EQUALS(string tagId, DateTimeOffset value) => new ItemsQuery { Op = Operator.EQUALS, TagId = tagId, ValueDate = value };
    }
#else

    // Not deserailizable by json
    public abstract class ItemsQuery
    {
    }
    public class ExistsQuery : ItemsQuery
    {
        public string TagId { get; set; }
    }
    public class EqualsQuery : ItemsQuery
    {
        public string TagId { get; set; }
        public string Value { get; set; }
        public long? ValueLong { get; set; }
        public decimal? ValueDecimal { get; set; }
        public DateTimeOffset? ValueDate { get; set; }
    }
    public class AndQuery : ItemsQuery
    {
        public ItemsQuery[] And { get; set; }
    }
    public class OrQuery : ItemsQuery
    {
        public ItemsQuery[] Or { get; set; }
    }
#endif
}
