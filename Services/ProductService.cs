using Microsoft.EntityFrameworkCore;
using FashionManagement.Data;
using FashionManagement.Models;
namespace FashionManagement.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;


        public ProductService(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<List<Product>> GetProductsAsync() 
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
            
        }
        public async Task AddProductAsync(Product product)
        {
           Console.WriteLine("Name: " + product.Name);
            Console.WriteLine("Price: " + product.Price);
            Console.WriteLine("Stock Quantity: " + product.StockQuantity);
            Console.WriteLine("Description: " + product.Description);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        

        public async Task UpdateProductAsync(Product product)
        {
            Console.WriteLine("UPDATING PRODUCT");
            Console.WriteLine("Name: " + product.Name);
            Console.WriteLine("Price: " + product.Price);
            Console.WriteLine("Stock Quantity: " + product.StockQuantity);
            Console.WriteLine("Description: " + product.Description);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
