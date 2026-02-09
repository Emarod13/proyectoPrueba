using Microsoft.EntityFrameworkCore;
using ProyectoPrueba.DTOs;
using ProyectoPrueba.Entity;

namespace ProyectoPrueba
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

       
    }
}
