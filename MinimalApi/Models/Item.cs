using MinimalApi.Models.Enums;

namespace MinimalApi.Models;

public class Item {
    public int Id { get; set;}
    public string? Name { get; set;}
    public ItemSize Size { get; set;}
}