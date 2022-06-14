using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _context.Products
            .Find(p => true)
            .ToListAsync();
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _context.Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
        return await _context.Products
            .Find(filter)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
    {
        return await _context.Products
            .Find(p => p.Category == category)
            .ToListAsync();
    }

    public async Task Create(Product product)
    {
        await _context.Products.InsertOneAsync(product);
    }

    public async Task<bool> Update(Product product)
    {
        var updatedResult = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
        return updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0;
    }

    public async Task<bool> Delete(string id)
    {
        var deletedResult = await _context.Products.DeleteOneAsync(p => p.Id == id);
        return deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;
    }
}