public class InventoryItem
{
    public ItemType Type { get; set; }
    public string Name { get; set; }

    public int Quantity { get; set; } = 0;

    public InventoryItem(ItemType type, string name, int quantity)
    {
        Type = type;
        Name = name;
        Quantity = quantity;
    }
}