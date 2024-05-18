using MinimalWebAPI.Models.Enums;

namespace MinimalWebAPI.Models.APIModels;

public class ItemAPIModel {

    public long Id { get; set; }
    public string? Name { get; set; }
    public ItemSize Size { get; set; }
}