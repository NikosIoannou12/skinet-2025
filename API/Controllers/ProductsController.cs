using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext productsDBContext;
    public ProductsController(StoreContext productsDBContext)
    {
        this.productsDBContext = productsDBContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await productsDBContext.Products.ToListAsync();
    }

    [HttpGet("{id:int}")]   // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productsDBContext.Products.FindAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productsDBContext.Products.Add(product);

        await productsDBContext.SaveChangesAsync();

        return Ok(product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
        {
            return BadRequest("Cannot update this product");
        }

        //We must inform the DB that the product we sent is modified and has to be updated when we save changes.
        productsDBContext.Entry(product).State = EntityState.Modified;

        await productsDBContext.SaveChangesAsync(); 

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productsDBContext.Products.FindAsync(id);

        if (product == null) return NotFound();

        productsDBContext.Remove(product);

        await productsDBContext.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return productsDBContext.Products.Any(x => x.Id == id);
    }
}
