using Microsoft.EntityFrameworkCore;

namespace nike_shoes_shop_backend.Models;

public class Cart
{
    public string productID { get; set; }
    public string userId { get; set; }
}