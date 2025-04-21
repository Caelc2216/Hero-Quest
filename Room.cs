using System.Runtime.CompilerServices;

public class Room
{
    public string Name { get; set; }
    public int Number { get; set; }
    public bool IsStart { get; set; }
    public bool IsExit { get; set; }
    public bool Looted = false;
    public bool ChallengeFinished = false;
    public Challenge challenge = null;
    public List<Item> loot = new List<Item>();
    public List<Item> possibleLoot { get; set; } = [
        new Item("Medium Health Potion", true, heffect: 10, type: Item.ItemType.Potion),
        new Item("Large Health Potion", true, heffect: 15, type: Item.ItemType.Potion),
        new Item("Small Health Potion", true, heffect: 5, type: Item.ItemType.Potion),
        new Item("Intelligence Potion", true, ieffect: 5, type: Item.ItemType.Potion),
        new Item("Agility Potion", true, aeffect: 5, type: Item.ItemType.Potion),
        new Item("Helmet", false, heffect: 5, type: Item.ItemType.Armor),
        new Item("Chestplate", false, heffect: 10, type: Item.ItemType.Armor),
        new Item("Ancient Book", false, ieffect: 5, type: Item.ItemType.Miscellaneous),
        new Item("Ring of Strength", false, seffect: 5, type: Item.ItemType.Miscellaneous),
        new Item("Ring of Agility", false, aeffect: 5, type: Item.ItemType.Miscellaneous),
        new Item("Ring of Intelligence", false, ieffect: 5, type: Item.ItemType.Miscellaneous),
        new Item("Ring of Health", false, heffect: 5, type: Item.ItemType.Miscellaneous),
        new Item("Lockpick", true, type: Item.ItemType.Miscellaneous),
    ];

    public List<Item> LootRoom()
    {
        bool alreadyHasHealthPotion = false;
        if (Looted)
        {
            Console.WriteLine("This room has already been looted.");
            return loot;
        }
        Random rand = new Random();
        int lootCount = rand.Next(0, 4);
        for (int i = 0; i < lootCount; i++)
        {
            int randomIndex = rand.Next(possibleLoot.Count);
            Item randomItem = possibleLoot[randomIndex];
            if (randomItem.Name.Contains("Health Potion") && alreadyHasHealthPotion)
            {
                continue;
            }
            else if (randomItem.Name.Contains("Health Potion") && !alreadyHasHealthPotion)
            {
                alreadyHasHealthPotion = true;
                loot.Add(randomItem);
                possibleLoot.RemoveAt(randomIndex);
            }
            else
            {
               loot.Add(randomItem);
                possibleLoot.RemoveAt(randomIndex); 
            }
        }
        Looted = true;
        return loot;
    }
}