Map m = new Map();
CustomBinaryTree challenges = new CustomBinaryTree();
Inventory inventory = new Inventory();
Hero hero = new Hero(1, 1, 1, 20, inventory);
List<Room> pathOut = m.InitializeMap(20, 0.002);
Stack<Room> visitedRooms = new Stack<Room>();


StartGame();
Console.WriteLine("Press any key to start the game...");
Console.ReadKey(true);

Room currentRoom = null;
foreach (var room in m.AdjacencyList.Keys)
{
    if (room.IsStart)
    {
        currentRoom = room;
        break;
    }
}

while (true)
{
    Console.Clear();
    visitedRooms.Push(currentRoom);
    Console.WriteLine("Current Room: " + currentRoom?.Name);
    Console.WriteLine(@"What do you want to do?
1. Move to another room
2. View inventory
3. Loot
4. Exit game");

    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

    switch (keyInfo.Key)
    {
        case ConsoleKey.D1:
        Console.Clear();
            Console.WriteLine("Current Room: " + currentRoom?.Name);
            DisplayRooms(m.AdjacencyList, currentRoom);
            Console.WriteLine("Enter the room number you want to move to:");
            string input = Console.ReadLine();
            bool CanMove = MoveToRoom(m.AdjacencyList, currentRoom, input);
            if (CanMove)
            {
                foreach (var room in m.AdjacencyList.Keys)
                {
                    if (room.Number.ToString() == input)
                    {
                        currentRoom = room;
                        break;
                    }
                }
            }
            break;
        case ConsoleKey.D2:
            Console.Clear();
            Console.WriteLine("Current Room: " + currentRoom?.Name);
            inventory.ViewInventory();
            inventory.InventorySelection(hero);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            break;
        case ConsoleKey.D3:
            Console.Clear();
            Console.WriteLine("Current Room: " + currentRoom?.Name);
            Loot();
            Console.WriteLine("Looting completed. Press any key to continue...");
            Console.ReadKey(true);
            break;
        case ConsoleKey.D4:
            Console.Clear();
            Console.WriteLine("Exiting game...");
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}

void StartGame()
{
    Console.WriteLine("Welcome to Hero's Quest!");
    inventory.InitializeInventory();
    hero.UpdateHeroStats();
    challenges.InitializeTree(numberOfChallenges: 20);
}

void DisplayRooms(Dictionary<Room, List<Room>> adjacencyList, Room currentRoom)
{
    Console.WriteLine("Available rooms to move to:");
    foreach (var room in adjacencyList[currentRoom])
    {
        Console.WriteLine($"Room {room.Number}");
    }
}

bool MoveToRoom(Dictionary<Room, List<Room>> adjacencyList, Room currentRoom, string input)
{
    if (int.TryParse(input, out int roomNumber))
    {
        foreach (var room in adjacencyList[currentRoom])
        {
            if (room.Number == roomNumber)
            {
                if (visitedRooms.Contains(room))
                {
                    Console.WriteLine("You have already visited this room. Do you want to move there again? (y/n)");
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key != ConsoleKey.Y)
                    {
                        return false;
                    }
                }
                currentRoom = room;
                Console.WriteLine($"Moved to {room.Name}.");
                GetChallenge(currentRoom);
                return true;
            }
        }
    }
    Console.WriteLine("Invalid room number. Please try again.");
    return false;
}

void GetChallenge(Room room)
{
    if (room.IsExit)
    {
        Console.WriteLine("You have reached the exit! Congratulations!");
        Environment.Exit(0);
    }
    else
    {
        Console.WriteLine($"You have entered {room.Name}. Prepare for a challenge!");
        Console.WriteLine("Press any key to start the challenge...");
        Console.ReadKey(true);
        Challenge c = challenges.ClosestNode(room.Number);
        StartChallenge(c);
    }
}

void StartChallenge(Challenge challenge)
{
    Console.Clear();
    Console.WriteLine($"Challenge: {challenge.Type} - Difficulty: {challenge.Difficulty}");
    if (challenge.Type == ChallengeType.Puzzle)
    {
        Console.WriteLine($"You have encountered a puzzle!");
        Console.WriteLine($"Requires intelligence greater than or equal to {challenge.Difficulty}");
        if (hero.Intelligence >= challenge.Difficulty)
        {
            Console.WriteLine("You solved the puzzle!");
            hero.UpdateHeroStats();
        }
        else
        {
            Console.WriteLine($"You failed to solve the puzzle. You lost {challenge.Difficulty - hero.Intelligence} health");
            hero.Health -= challenge.Difficulty - hero.Intelligence;
            hero.UpdateHeroStats();
        }
    }
    if (challenge.Type == ChallengeType.Combat)
    {
        Console.WriteLine($"You have encountered an enemy!");
        Console.WriteLine($"Requires strength greater than or equal to {challenge.Difficulty}");
        if (hero.Strength >= challenge.Difficulty)
        {
            Console.WriteLine("You defeated the enemy!");
            hero.UpdateHeroStats();
        }
        else
        {
            Console.WriteLine($"You failed to defeat the enemy. You lost {challenge.Difficulty - hero.Strength} health");
            hero.UpdateHeroStats();
        }
    }

    // Trap
    else
    {
        Console.WriteLine($"You have come across a trap!");
        Console.WriteLine($"Requires agility greater than or equal to {challenge.Difficulty}");
        if (hero.Agility >= challenge.Difficulty)
        {
            Console.WriteLine("You avoided the trap!");
            hero.UpdateHeroStats();
        }
        else
        {
            Console.WriteLine($"You failed to avoid the trap. You lost {challenge.Difficulty - hero.Agility} health");
            hero.UpdateHeroStats();
        }
    }
    challenges.DeleteNode(challenge);
    challenges.Rebalance();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
}

void Loot()
{
    List<Item> lootFound = currentRoom.LootRoom();
    Console.WriteLine("You found the following items:");
    for (int i = 0; i < lootFound.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {lootFound[i].Name}");
    }
    Console.WriteLine("How many items do you want to take? (1-3)");
    string input = Console.ReadLine();
    if (int.TryParse(input, out int numberOfItems) && numberOfItems <= lootFound.Count)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            Console.WriteLine($"Enter the number of the item you want to take (1-{lootFound.Count}):");
            string itemInput = Console.ReadLine();
            if (int.TryParse(itemInput, out int itemIndex) && itemIndex >= 1 && itemIndex <= lootFound.Count)
            {
                Item selectedItem = lootFound[itemIndex - 1];
                inventory.AddItem(selectedItem);
                Console.WriteLine($"You took {selectedItem.Name}.");
                lootFound.RemoveAt(itemIndex - 1); // Remove the item from the loot list
            }
            else
            {
                Console.WriteLine("Invalid item number. Please try again.");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid number of items. Please try again.");
    }
}

