using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    public class OrderingContext : DbContext
    {
        public OrderingContext() : base("OrderDataBase")
        {
            Database.SetInitializer(
              new DropCreateDatabaseIfModelChanges<OrderingContext>());
        }

        public DbSet<OrderforDB> OrderforDBs { get; set; }
        public DbSet<ItemforDB> ItemforDBs { get; set; }
    }
}
