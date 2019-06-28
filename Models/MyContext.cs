    using Microsoft.EntityFrameworkCore; 
    namespace Proj.Models
    {
        public class MyContext : DbContext
        {
            public MyContext(DbContextOptions options) : base(options) { }
            
            public DbSet<Item> Items {get;set;}
        }
    }
    