const int HealthPotionCost = 20;
const int HealthPotionHealAmount = 100;
const int MaxHealth = 100;

Console.WriteLine("Hello, dear traveler!");
Console.WriteLine("What is your name?");
string name = Console.ReadLine() ?? "Traveler";
Console.WriteLine($"Welcome, {name}! Your adventure begins now.");

Player player = new Player { Name = name };
Random random = new Random();

Console.WriteLine("Do you wish to look at your stats? (yes/no)");
if (IsYes(Console.ReadLine()))
{
    PrintPlayerStats(player);
}
else
{
    Console.WriteLine("Very well, let the adventure continue!");
}

RunAdventure(player, random);

// Vyhodnoti odpoved ano/yes pre jednoduche rozhodnutia. Dokoncene.
bool IsYes(string? input)
{
    string normalized = (input ?? string.Empty).Trim().ToLowerInvariant();
    return normalized is "yes" or "ano";
}

// Nahodne vyberie typ nepriatela pre dalsi suboj. Dokoncene.
Enemy CreateRandomEnemy(Random rng)
{
    return rng.Next(0, 4) switch
    {
        0 => new Goblin(),
        1 => new Skeleton(),
        2 => new Zombie(),
        _ => new HumanBandit(),
    };
}

// Vypise aktualne statistiky hraca na konzolu. Dokoncene.
void PrintPlayerStats(Player currentPlayer)
{
    Console.WriteLine($"Name: {currentPlayer.Name}");
    Console.WriteLine($"Health: {currentPlayer.Health}");
    Console.WriteLine($"Level: {currentPlayer.Level}");
    Console.WriteLine($"Experience: {currentPlayer.Experience}");
    Console.WriteLine($"Attack Power: {currentPlayer.AttackPower}");
    Console.WriteLine($"Defense: {currentPlayer.Defense}");
    Console.WriteLine($"Gold: {currentPlayer.Gold}");
}

// Riadi hlavny cyklus dobrodruzstva (stretnutie, boj, pokracovanie). Dokoncene.
void RunAdventure(Player currentPlayer, Random rng)
{
    while (currentPlayer.Health > 0)
    {
        Console.WriteLine("You encounter a wild enemy!");
        Enemy currentEnemy = CreateRandomEnemy(rng);

        Console.WriteLine("Do you wish to fight the enemy? (yes/no)");
        if (!IsYes(Console.ReadLine()))
        {
            Console.WriteLine("You choose to end your adventure. Farewell, traveler!");
            break;
        }

        Console.WriteLine("You engage in battle!");
        BattleResult result = RunBattle(currentPlayer, currentEnemy, rng);

        if (result is BattleResult.PlayerDefeated or BattleResult.PlayerRanAway)
        {
            break;
        }

        if (!HandleAdventureChoice(currentPlayer))
        {
            Console.WriteLine("You decide to end your adventure here. Farewell, traveler!");
            break;
        }
    }
}

// Spracuje cely boj medzi hracom a jednym nepriatelom. Dokoncene.
BattleResult RunBattle(Player currentPlayer, Enemy currentEnemy, Random rng)
{
    while (currentPlayer.Health > 0 && currentEnemy.Health > 0)
    {
        if (!TryPlayerAttack(currentPlayer, currentEnemy, rng))
        {
            Console.WriteLine("You ran away from the fight!");
            return BattleResult.PlayerRanAway;
        }

        if (currentEnemy.Health <= 0)
        {
            HandleEnemyDefeat(currentPlayer, currentEnemy);
            return BattleResult.EnemyDefeated;
        }

        if (!ResolveEnemyAttack(currentPlayer, currentEnemy, rng))
        {
            return BattleResult.PlayerDefeated;
        }
    }

    return currentPlayer.Health <= 0 ? BattleResult.PlayerDefeated : BattleResult.EnemyDefeated;
}

// Vykona utok hraca, alebo vrati false ak sa hrac rozhodol utiect. Dokoncene.
bool TryPlayerAttack(Player currentPlayer, Enemy currentEnemy, Random rng)
{
    Console.WriteLine("Do you wish to attack? (yes/no)");
    if (!IsYes(Console.ReadLine()))
    {
        return false;
    }

    Console.WriteLine("You choose to attack the enemy!");
    int playerDamage = currentPlayer.CalculateDamageAgainst(currentEnemy, rng, out bool criticalHit);
    if (criticalHit)
    {
        Console.WriteLine("Critical hit! You deal extra damage!");
    }

    currentEnemy.Health -= playerDamage;
    Console.WriteLine($"You deal {playerDamage} damage to the {currentEnemy.Name}. Enemy health is now {currentEnemy.Health}.");
    return true;
}

// Vykona utok nepriatela na hraca a skontroluje porazku. Dokoncene.
bool ResolveEnemyAttack(Player currentPlayer, Enemy currentEnemy, Random rng)
{
    int enemyDamage = currentEnemy.CalculateDamageAgainst(currentPlayer, rng, out bool enemyCriticalHit);
    if (enemyCriticalHit)
    {
        Console.WriteLine($"The {currentEnemy.Name} lands a critical hit! It deals extra damage!");
    }

    currentPlayer.Health -= enemyDamage;
    Console.WriteLine($"The {currentEnemy.Name} deals {enemyDamage} damage to you. Your health is now {currentPlayer.Health}.");

    if (currentPlayer.Health <= 0)
    {
        currentPlayer.Health = 0;
        Console.WriteLine("You have been defeated! Game over.");
        return false;
    }

    return true;
}

// Prideli odmeny za porazenie nepriatela (XP + zlato). Dokoncene.
void HandleEnemyDefeat(Player currentPlayer, Enemy currentEnemy)
{
    currentEnemy.Health = 0;
    Console.WriteLine($"You have defeated the {currentEnemy.Name}!");
    currentPlayer.Experience += currentEnemy.XpReward;
    currentPlayer.Gold += currentEnemy.GoldReward;
    Console.WriteLine($"You gain {currentEnemy.XpReward} XP and {currentEnemy.GoldReward} gold.");
    Console.WriteLine($"Your current XP: {currentPlayer.Experience}, Gold: {currentPlayer.Gold}");
    if (currentPlayer.Experience >= currentPlayer.Level * 50)
    {
        PlayerLevelUp levelUpHandler = new PlayerLevelUp();
        levelUpHandler.LevelUpPlayer(currentPlayer);
    }
}

// Po boji vyriesi dalsiu volbu hraca (pokracovat, obchod, koniec). Dokoncene.
bool HandleAdventureChoice(Player currentPlayer)
{
    Console.WriteLine("Do you wish to continue your adventure? (yes/no)");
    if (!IsYes(Console.ReadLine()))
    {
        return false;
    }

    Console.WriteLine("You chose to continue your adventure! Nice choice, traveler! You can choose from this options :");
    Console.WriteLine($"1. Health Potion for {HealthPotionCost} gold (restores {HealthPotionHealAmount} health)");
    Console.WriteLine("2. View your stats");
    Console.WriteLine("3. Check your inventory");
    Console.WriteLine("4. Quit the adventure");

    string response = (Console.ReadLine() ?? "3").Trim();
    if (response == "1")
    {
        BuyHealthPotion(currentPlayer);
        return true;
    }

    if (response == "2")
    {
        PrintPlayerStats(currentPlayer);
        return true;
    }

    if (response == "3")
    {
        currentPlayer.PrintInventory();
        return true;
    }

    if (response == "4")
    {
        Console.WriteLine("You chose to end your adventure. Farewell, traveler!");
        return false;
    }

    Console.WriteLine("Invalid choice. Please try again.");
    return true;
}

// Pokusi sa kupit lektvar zdravia a obnovi HP do maxima. Dokoncene.
void BuyHealthPotion(Player currentPlayer)
{
    if (currentPlayer.Gold < HealthPotionCost)
    {
        Console.WriteLine("You don't have enough gold to buy a Health Potion.");
        return;
    }

    currentPlayer.Gold -= HealthPotionCost;
    currentPlayer.Health = Math.Min(currentPlayer.Health + HealthPotionHealAmount, MaxHealth);
    Console.WriteLine("You bought a Health Potion and restored 100 health!");
    Console.WriteLine($"Your current health: {currentPlayer.Health}, Gold: {currentPlayer.Gold}");
}

enum BattleResult
{
    EnemyDefeated,
    PlayerDefeated,
    PlayerRanAway,
}


