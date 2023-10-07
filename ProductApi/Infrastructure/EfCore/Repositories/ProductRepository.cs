using Microsoft.EntityFrameworkCore;
using ProductApi.Core.Models;
using SharedModels;

namespace ProductApi.Infrastructure.EfCore.Repositories;

public class ProductRepository : IRepository<Product>
{
    private readonly ProductApiContext db;

    public ProductRepository(ProductApiContext db)
    {
        this.db = db;
    }

    public Product Add(Product entity)
    {
        var product = db.Products.Add(entity);
        db.SaveChanges();

        return product.Entity;
    }

    public IEnumerable<Product> GetAll()
    {
        return db.Products.ToList();
    }

    public Product Get(int id)
    {
        return db.Products.FirstOrDefault(p => p.Id == id);
    }

    public Product Edit(Product entity)
    {
        db.Entry(entity).State = EntityState.Modified;
        db.SaveChanges();
        return entity;
    }

    public Product Remove(Product entity)
    {
        var product = db.Products.FirstOrDefault(entity);
        db.Products.Remove(product);
        db.SaveChanges();
        return product;
    }
}