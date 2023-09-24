using System.ComponentModel.DataAnnotations;

namespace nike_shoes_shop_backend.Models;

public class Users
{
    [Key]
    public string userId { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}