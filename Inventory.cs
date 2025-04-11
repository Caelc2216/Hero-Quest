public class Inventory
{
    public Queue<Item> items;
    public Queue<Item> newItems;
    public int Max = 5;

    public Inventory()
    {
        items = new Queue<Item>();
        newItems = new Queue<Item>();
    }

    public void AddItem(Item item)
    {
        if (items.Count < Max)
        {
            items.Enqueue(item);
            newItems.Enqueue(item);
            Console.WriteLine($"{item.Name} has been added to the inventory.");
        }
        else
        {
            Console.WriteLine("Inventory is full. Oldest item will be removed. Do you want to proceed? (y/n)");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key != ConsoleKey.Y)
            {
                Console.WriteLine("Item not added to inventory.");
                return;
            }
            items.Dequeue();
            items.Enqueue(item);
            Console.WriteLine($"{item.Name} has been added to the inventory. Oldest item removed.");
        }
    }

    public void ViewInventory()
    {
        Console.WriteLine("Inventory:");
        Console.WriteLine($"{"Name", -20}{"Agility",-10}{"Intelligence",-15}{"Strength",-10}{"Health", -10}{"Type", -20}");
        Console.WriteLine("--------------------------------------------------------------------------------------------------");
        foreach (var item in items)
        {
            Console.WriteLine($"{item.Name,-20} {item.Aeffect, -10} {item.Ieffect,-15} {item.Seffect,-10} {item.Heffect,-10} {item.Type,-20}");
        }
    }

    public void InitializeInventory()
    {
        AddItem(new Item("Sword", seffect: 5, type: Item.ItemType.Weapon, effectOnUse: false));
        AddItem(new Item("Shield", heffect: 10, type: Item.ItemType.Armor, effectOnUse: false));
    }

    public void InventorySelection(Hero h)
    {
        Console.WriteLine(@"What do you want to do?
        1. Use item
        2. Exit inventory");
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        switch (keyInfo.Key)
        {
            case ConsoleKey.D1:
                UseItem(h);
                break;
            case ConsoleKey.D2:
                Console.WriteLine("Exiting inventory...");
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
    public void UseItem(Hero h)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("No items in inventory to use.");
            return;
        }
        Console.WriteLine("Enter the name of the item you want to use:");
        string itemName = Console.ReadLine();
        Item itemToUse = null;
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                itemToUse = item;
                break;
            }
        }

        if ((itemToUse.Type == Item.ItemType.Potion || itemToUse.Type == Item.ItemType.Miscellaneous) && itemToUse.EffectOnUse == true)
        {
            Console.WriteLine($"Using {itemToUse.Name}...");
            h.Strength += itemToUse.Seffect;
            h.Agility += itemToUse.Aeffect;
            h.Intelligence += itemToUse.Ieffect;
            h.Health += itemToUse.Heffect;
            h.UpdateHeroStats();
            items.Dequeue();
            Console.WriteLine($"{itemToUse.Name} has been used and removed from the inventory.");
        }
        else
        {
            Console.WriteLine("Invalid item type. Cannot use this item.");
        }
    }
}