public static class Shop
{
    private static readonly List<Item> ShopItems = new List<Item>
    {
        new Item { Name = "Sword of Strength", Type = ItemType.Weapon, Value = 10, Price = 100 },
        new Item { Name = "Shield of Defense", Type = ItemType.Armor, Value = 5, Price = 80 },
        new Item { Name = "Health Potion", Type = ItemType.Consumable, Value = 20, Price = 30 },
        new Item { Name = "Ring of Luck", Type = ItemType.Accessory, Value = 15, Price = 120 }
    };

    private static void DisplayShopItems()
    {
        Console.WriteLine("Welcome to the shop! Here are the items available for purchase:");
        for (int i = 0; i < ShopItems.Count; i++)
        {
            var item = ShopItems[i];
            Console.WriteLine($"{i + 1}. {item.Name} - Type: {item.Type}, Value: {item.Value}, Price: {item.Price} gold");
        }
    }

    private static void BuyItem(Player player, int itemIndex)
    {
        if (itemIndex < 1 || itemIndex > ShopItems.Count)
        {
            Console.WriteLine("Invalid item selection.");
            return;
        }

        var selectedItem = ShopItems[itemIndex - 1];
        if (player.Gold < selectedItem.Price)
        {
            Console.WriteLine("You don't have enough gold to buy this item.");
            return;
        }

        player.Gold -= selectedItem.Price;
        player.Inventory.Add(selectedItem);
        Console.WriteLine($"You have purchased {selectedItem.Name} for {selectedItem.Price} gold.");
    }

    private static void SellItem(Player player, int itemIndex)
    {
        if (itemIndex < 1 || itemIndex > player.Inventory.Count)
        {
            Console.WriteLine("Invalid item selection.");
            return;
        }

        var selectedItem = player.Inventory[itemIndex - 1];
        int sellPrice = selectedItem.Price / 2; // Predajna cena je polovica nakupnej ceny
        player.Gold += sellPrice;
        player.Inventory.RemoveAt(itemIndex - 1);
        Console.WriteLine($"You have sold {selectedItem.Name} for {sellPrice} gold.");
    }

    public static void OpenShop(Player player)
    {
        while (true)
        {
            DisplayShopItems();
            Console.WriteLine("Enter the number of the item you want to buy, or 0 to exit:");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice == 0)
            {
                break;
            }
            else if (int.TryParse(Console.ReadLine(), out choice))
            {
                BuyItem(player, choice);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }
}



