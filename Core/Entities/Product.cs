using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;

public class Product : BaseEntity
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public decimal Price { get; set; }
    [Required]
    public string PictureUrl { get; set; }
    public string Type {  get; set; }
    [Required]
    public string Brand { get; set; }
    public int QuantityInStock { get; set; }
}
