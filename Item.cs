public class Item
{
    public string? Name {get; set; }
    public ItemType Type {get; set; }
    public int Value {get; set; }
    public int Price {get; set; }
}

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Accessory
}
