public class Item
{
    public string Name { get; set; }
    public enum ItemType { Weapon, Armor, Potion, Miscellaneous }
    public ItemType Type { get; set; } // Type of the item (Weapon, Armor, Potion, etc.)
    public bool EffectOnUse { get; set; } // Indicates if the item is used or not
    public int Aeffect { get; set; } // Effect of the item on the hero's stats
    public int Ieffect { get; set; } // Effect of the item on the hero's stats
    public int Seffect { get; set; } // Effect of the item on the hero's stats
    public int Heffect { get; set; } // Effect of the item on the hero's stats

    public Item(string name, bool effectOnUse, int aeffect = 0, int ieffect = 0, int seffect = 0, int heffect = 0, ItemType type = ItemType.Miscellaneous)
    {
        Name = name;
        Aeffect = aeffect;
        Ieffect = ieffect;
        Seffect = seffect;
        Heffect = heffect;
        Type = type;
        EffectOnUse = effectOnUse;
    }
}