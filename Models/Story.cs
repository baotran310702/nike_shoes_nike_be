using System.ComponentModel.DataAnnotations;

namespace nike_shoes_shop_backend.Models;
public class Story
{
    [Key]
    public string id { get; set; }
    public string heading { get; set; }
    public string text { get; set; }
    public string btn { get; set; }
    public string url { get; set; }
    public string img { get; set; }
    public string time { get; set; }
    public string like { get; set; }
}