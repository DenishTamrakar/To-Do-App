using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace ToDo.Data{
    public class AppDbContext : DbContext{
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
        public DbSet<TODO> ToDos { get; set;}
        public DbSet<User> Users { get; set; }
    }
}