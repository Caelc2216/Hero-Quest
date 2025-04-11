using System.Runtime.CompilerServices;

public class Room
{
    public string Name { get; set; }
    public int Number { get; set; }
    public bool IsStart { get; set; }
    public bool IsExit { get; set; }
    public bool Looted = false;
    public bool ChallengeFinished = false;
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
    ];

    public List<Item> LootRoom()
    {
        if (Looted)
        {
            Console.WriteLine("This room has already been looted.");
            return new List<Item>();
        }
        Random rand = new Random();
        int lootCount = rand.Next(1, 4); 
        List<Item> loot = new List<Item>();
        for (int i = 0; i < lootCount; i++)
        {
            int randomIndex = rand.Next(possibleLoot.Count);
            Item randomItem = possibleLoot[randomIndex];
            loot.Add(randomItem);
            possibleLoot.RemoveAt(randomIndex); 
        }
        Looted = true;
        return loot;
    }
}