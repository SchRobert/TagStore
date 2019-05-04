using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TagStore.Service.Data.Items;
using TagStore.Service.Models.Items;

using static TagStore.Service.Models.Items.ItemsQuery;
namespace TagStore.Service.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var repository = new ItemsRepository(new ItemsContext(null));

            // find items with tag1
            var items1 = repository.FindItems(EXISTS("tag1"));
            // find items with tag1=value1
            var items2 = repository.FindItems(EQUALS("tag1", "value1"));
            // find items with tag1 and tag2=42
            var items3 = repository.FindItems(AND(EXISTS("tag1"), EQUALS("tag2", 42)));
            // find items with tag1 or tag2=10,5
            var items4 = repository.FindItems(OR(EXISTS("tag1"), EQUALS("tag2", 10.5m)));
            // find items with tag1 or (tag2 and tag3)
            var items5 = repository.FindItems(OR(EXISTS("tag1"), AND(EXISTS("tag2"), EXISTS("tag3"))));
        }
    }
}
