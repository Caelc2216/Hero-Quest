public class Hero
{
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Intelligence { get; set; }
    public int Health { get; set; }
    public Inventory inventory { get; set; }

    public Hero(int strength, int agility, int intelligence, int health, Inventory i)
    {
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
        Health = health;
        inventory = i;
    }

    public void UpdateHeroStats()
    {
        foreach (var item in inventory.newItems)
        {
            Strength += item.Seffect;
            Agility += item.Aeffect;
            Intelligence += item.Ieffect;
            Health += item.Heffect;
        }
            inventory.newItems.Clear();
        Console.WriteLine($"Hero stats updated: Strength: {Strength}, Agility: {Agility}, Intelligence: {Intelligence}, Health: {Health}");
    }
}