using Microsoft.EntityFrameworkCore;

 
namespace LogReg.Models
{
    public class LRContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public LRContext(DbContextOptions options) : base(options) { }
        //"users" table is represeted by the DbSet "Users"
        public DbSet<User> Users {get;set;}

    }
}
