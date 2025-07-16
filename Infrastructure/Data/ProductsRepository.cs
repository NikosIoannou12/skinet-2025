using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class ProductsRepository(StoreContext context) : IProductRepository
{
    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await context.Products.Select(x => x.Brand).Distinct().ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await context.Products.Select(x => x.Type).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var query = context.Products.AsQueryable(); //All products

        if (!string.IsNullOrWhiteSpace(brand))                 //If brand is given, search for those products with that brand
        {
            query = query.Where(x => x.Brand == brand);
        }

        if (!string.IsNullOrWhiteSpace(type))                  //If type is given, search for those products with that type
        {
            query = query.Where(x => x.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            if (sort == "priceAsc")
            {
                // Sort by price ascending
                query = query.OrderBy(x => x.Price);
            } else if (sort == "priceDesc")
            {
                // Sort by price descending
                query = query.OrderByDescending(x => x.Price);
            }
        }

        query = sort switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            _ => query.OrderBy(x => x.Name) // Default sort by Name
        };

        return await query.ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return context.Products.Any(p => p.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
    }
}
