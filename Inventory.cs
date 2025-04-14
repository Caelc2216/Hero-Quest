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
            string oldestItem = items.Peek().Name;
            Console.WriteLine($"Inventory is full. {oldestItem} will be removed. Do you want to proceed? (y/n)");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key != ConsoleKey.Y)
            {
                Console.WriteLine("Item not added to inventory.");
                return;
            }
            items.Dequeue();
            items.Enqueue(item);
            Console.WriteLine($"{item.Name} has been added to the inventory. {oldestItem} removed.");
        }
    }

    public void ViewInventory()
    {
        Console.WriteLine("Inventory:");
        Console.WriteLine($"{"Name",-20}{"Agility",-10}{"Intelligence",-15}{"Strength",-10}{"Health",-10}{"Type",-20}{"Usable/Passive Effect",-25}");
        Console.WriteLine("--------------------------------------------------------------------------------------------------");
        foreach (var item in items)
        {
            string use = item.EffectOnUse ? "Usable" : "Passive Effect";
            Console.WriteLine($"{item.Name,-20} {item.Aeffect,-10} {item.Ieffect,-15} {item.Seffect,-10} {item.Heffect,-10} {item.Type,-20}{use,-25}");
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
        2. Remove item
        3. Exit inventory");
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        switch (keyInfo.Key)
        {
            case ConsoleKey.D1:
                UseItem(h);
                break;
            case ConsoleKey.D2:
                Item item = SelectItem(h);
                RemoveItem(item, h);
                h.UpdateHeroStats();
                break;
            case ConsoleKey.D3:
                Console.WriteLine("Exiting inventory...");
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
    public void UseItem(Hero h)
    {
        Item itemToUse = SelectItem(h);
        if (itemToUse != null)
        {
            if ((itemToUse.Type == Item.ItemType.Potion || itemToUse.Type == Item.ItemType.Miscellaneous) && itemToUse.EffectOnUse == true)
            {
                Queue<Item> tempQueue = new Queue<Item>();
                Console.WriteLine($"Using {itemToUse.Name}...");
                h.Strength += itemToUse.Seffect;
                h.Agility += itemToUse.Aeffect;
                h.Intelligence += itemToUse.Ieffect;
                h.Health += itemToUse.Heffect;
                h.UpdateHeroStats();
                foreach (var item in items)
                {
                    if (item.Name != itemToUse.Name)
                    {
                        tempQueue.Enqueue(item);
                    }
                }
                items.Clear();
                foreach (var item in tempQueue)
                {
                    items.Enqueue(item);
                }
                tempQueue.Clear();
                Console.WriteLine($"{itemToUse.Name} has been used and removed from the inventory.");
            }
        }
        else
        {
            Console.WriteLine("Invalid item type. Cannot use this item.");
        }
    }

    public void RemoveItem(Item item, Hero h)
    {
        if (items.Contains(item))
        {
            Queue<Item> tempQueue = new Queue<Item>();
            foreach (var i in items)
            {
                if (i.Name != item.Name)
                {
                    tempQueue.Enqueue(i);
                }
            }
            items.Clear();
            foreach (var i in tempQueue)
            {
                items.Enqueue(i);
            }

            if (item.EffectOnUse == false)
            {
                if(item.Aeffect != 0)
                {
                    h.Agility -= item.Aeffect;
                }
                if(item.Ieffect != 0)
                {
                    h.Intelligence -= item.Ieffect;
                }
                if(item.Seffect != 0)
                {
                    h.Strength -= item.Seffect;
                }
                if(item.Heffect != 0)
                {
                    h.Health -= item.Heffect;
                }
            }
            Console.WriteLine($"{item.Name} has been removed from the inventory.");
        }
        else
        {
            Console.WriteLine($"{item.Name} is not in the inventory.");
        }

    }

    public Item SelectItem(Hero h)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("No items in inventory.");
            return null;
        }
        Console.WriteLine("Enter the name of the item you want to select:");
        string itemName = Console.ReadLine();
        Item itemToUse = null;
        if (itemName == null || itemName == "")
        {
            Console.WriteLine("Invalid item name. Please try again.");
            return null;
        }
        foreach (var item in items)
        {
            if (item.Name == itemName)
            {
                itemToUse = item;
                return itemToUse;
            }
        }
        return null;
    }
}