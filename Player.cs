using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;    

public class Player : Character
{
    public int Experience { get; set; } = 0;
    public int Gold { get; set; } = 0;

    // Nastavi zakladne staty hraca na zaciatku hry. Dokoncene.
    public Player()
    {
        AttackPower = 25;
        Defense = 5;
        CriticalChance = 10;
    }

    public List<Item> Inventory { get; set; } = new List<Item>();

    // Prida item do inventara hraca. Dokoncene.
    public void AddItemToInventory(Item item)
    {
        Inventory.Add(item);
        Console.WriteLine($"You have obtained: {item.Name} (Type: {item.Type}, Value: {item.Value}, Price: {item.Price} gold)");
    }

    // Vypise obsah inventara hraca. Dokoncene.
    public void PrintInventory()
    {
        if (Inventory.Count == 0)
        {
            Console.WriteLine("Your inventory is empty.");
            return;
        }

        Console.WriteLine("Your Inventory:");
        foreach (var item in Inventory)
        {
            Console.WriteLine($"- {item.Name} (Type: {item.Type}, Value: {item.Value}, Price: {item.Price} gold)");
        }
    }
}


public class PlayerLevelUp
{
    // Zvysi level hraca a vylepsi jeho staty. Dokoncene.
    public bool LevelUpPlayer(Player player)
    {
        int requiredExperience = player.Level * 50;
        if (player.Experience < requiredExperience)
        {
            return false;
        }

        player.Experience -= requiredExperience;
        player.Level++;
        player.AttackPower += 5;
        player.Defense += 2;
        player.CriticalChance += 2;
        player.Health = 100 + (player.Level - 1) * 20;
        Console.WriteLine($"Congratulations! You've reached level {player.Level}!");
        return true;
    }
}
