using System.ComponentModel.DataAnnotations;

namespace nike_shoes_shop_backend.Models;

public class Users
{
    [Key]
    public string userId { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string fullName { get; set; }
    public string age { get; set; }
    public string dob { get; set; }
    public string email { get; set; }
    public string role { get; set; }
    public string phone { get; set; }
}