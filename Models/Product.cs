using System.ComponentModel.DataAnnotations;
namespace nike_shoes_shop_backend.Models;
public class Product
{
    [Key]
    public string id { get; set; }
    public string title { get; set; }
    public string text { get; set; }
    public string rating { get; set; }
    public string btn { get; set; }
    public string img { get; set; }
    public string price { get; set; }
}