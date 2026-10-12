using Microsoft.EntityFrameworkCore;
using FashionManagement.Models;
namespace FashionManagement.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext > options) : base(options)
    {

    }
    public DbSet<Product> Products{ get; set; }
    
}
