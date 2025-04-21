Map m = new Map();
CustomBinaryTree challenges = new CustomBinaryTree();
Inventory inventory = new Inventory();
Hero hero = new Hero(1, 1, 1, 20, inventory);
List<Room> pathOut = m.InitializeMap(20, 0.002);
Room previousRoom = null;
List<Room> deadEnds = new List<Room>();
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
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Current Room: " + currentRoom?.Name);
    Console.ResetColor();
    hero.DisplayStats();
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Current Room: " + currentRoom?.Name);
            Console.ResetColor();
            hero.DisplayStats();
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
                        previousRoom = currentRoom;
                        currentRoom = room;
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("You didn't move to this room. Press any key to continue...");
                Console.ReadKey(true);
            }
            break;
        case ConsoleKey.D2:
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Current Room: " + currentRoom?.Name);
            Console.ResetColor();
            hero.DisplayStats();
            inventory.ViewInventory();
            inventory.InventorySelection(hero);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            break;
        case ConsoleKey.D3:
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Current Room: " + currentRoom?.Name);
            Console.ResetColor();
            hero.DisplayStats();
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
    challenges.InitializeTree(numberOfChallenges: 30);
}

void DisplayRooms(Dictionary<Room, List<Edge>> adjacencyList, Room currentRoom)
{
    Console.WriteLine("Available rooms to move to:");
    List<Room> availableRooms = new List<Room>();
    foreach (var room in adjacencyList[currentRoom])
    {
        if (deadEnds.Contains(room.To))
        {
            continue;
        }
        availableRooms.Add(room.To);
    }
    if (availableRooms.Count == 1 && availableRooms[0] == previousRoom)
    {
        deadEnds.Add(currentRoom);
    }
    foreach (var room in availableRooms)
    {
        if (visitedRooms.Contains(room))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Room {room.Number}");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine($"Room {room.Number}");
        }
    }
}

bool MoveToRoom(Dictionary<Room, List<Edge>> adjacencyList, Room currentRoom, string input)
{
    if (int.TryParse(input, out int roomNumber))
    {
        foreach (var room in adjacencyList[currentRoom])
        {
            if (room.To.Number == roomNumber)
            {
                bool requirementsPassed = CheckRequirements(room.To);
                if (!requirementsPassed)
                {
                    return false;
                }
                if (visitedRooms.Contains(room.To))
                {
                    Console.WriteLine("You have already visited this room. Do you want to move there again? (y/n)");
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key != ConsoleKey.Y)
                    {
                        return false;
                    }
                }
                currentRoom = room.To;
                Console.WriteLine($"Moved to {room.To.Name}.");
                if (visitedRooms.Contains(room.To) && room.To.ChallengeFinished)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You have already completed the challenge in this room.");
                    Console.ResetColor();
                    return true;
                }
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
    if (challenges == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("You have ran out of challenges. Game over.");
        Console.ResetColor();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
        Console.WriteLine("Correct Path: ");
        foreach (var r in pathOut)
        {
            Console.WriteLine(r.Name);
        }
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
        Environment.Exit(0);
    }
    if (room.IsExit)
    {
        Console.WriteLine("You have found the exit but first you need to complete the challenge!");
        Console.WriteLine("Press any key to start the challenge...");
        Console.ReadKey(true);
        if (room.challenge == null)
        {
            room.challenge = challenges.ClosestNode(room.Number);
            StartChallenge(room.challenge);
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("You have reached the exit! Congratulations!");
        Console.ResetColor();
        Environment.Exit(0);
    }
    else
    {
        Console.WriteLine($"You have entered {room.Name}. Prepare for a challenge!");
        Console.WriteLine("Press any key to start the challenge...");
        Console.ReadKey(true);
        if (room.challenge == null)
        {
            room.challenge = challenges.ClosestNode(room.Number);
        }
        StartChallenge(room.challenge);
        if (room.challenge != null && (room.challenge.Type == ChallengeType.Puzzle || room.challenge.Type == ChallengeType.Combat))
        {
            room.ChallengeFinished = true;
        }
    }
}

void StartChallenge(Challenge challenge)
{
    Console.Clear();
    if (challenge == null)
    {
        Console.WriteLine("Challenge is null");
        return;
    }
    Console.WriteLine($"Challenge: {challenge.Type} - Difficulty: {challenge.Difficulty}");
    if (challenge.Type == ChallengeType.Puzzle)
    {
        Console.WriteLine($"You have encountered a puzzle!");
        Console.WriteLine($"Requires intelligence greater than or equal to {challenge.Difficulty}");
        if (hero.Intelligence >= challenge.Difficulty)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You solved the puzzle!");
            Console.ResetColor();
            hero.UpdateHeroStats();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You failed to solve the puzzle. You lost {challenge.Difficulty - hero.Intelligence} health");
            Console.ResetColor();
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You defeated the enemy!");
            Console.ResetColor();
            hero.UpdateHeroStats();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You failed to defeat the enemy. You lost {challenge.Difficulty - hero.Strength} health");
            Console.ResetColor();
            hero.Health -= challenge.Difficulty - hero.Strength;
            hero.UpdateHeroStats();
        }
    }

    else if (challenge.Type == ChallengeType.Trap)
    {
        Console.WriteLine($"You have come across a trap!");
        Console.WriteLine($"Requires agility greater than or equal to {challenge.Difficulty}");
        if (hero.Agility >= challenge.Difficulty)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You avoided the trap!");
            Console.ResetColor();
            hero.UpdateHeroStats();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You failed to avoid the trap. You lost {challenge.Difficulty - hero.Agility} health");
            Console.ResetColor();
            hero.Health -= challenge.Difficulty - hero.Agility;
            hero.UpdateHeroStats();
        }
    }
    if (hero.Health <= 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("You have died. Game over.");
        Console.ResetColor();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
        Console.WriteLine("Correct Path: ");
        foreach (var room in pathOut)
        {
            Console.WriteLine(room.Name);
        }
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
        Environment.Exit(0);
    }
    challenges.DeleteNode(challenge);
    challenges.Rebalance();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
}

void Loot()
{
    List<Item> lootFound = currentRoom.LootRoom();
    Random treasureRandom = new Random();
    int treasureChance = treasureRandom.Next(0, 100);
    if (treasureChance < 10)
    {
        Treasure treasureFound = (Treasure)treasureRandom.Next(0, 3);
        if (treasureFound != Treasure.None)
        {
            Console.WriteLine($"You found a treasure: {treasureFound}!");
            hero.inventory.treasures.Push(treasureFound);
        }
    }
    if (lootFound.Count == 0)
    {
        Console.WriteLine("No loot found in this room.");
        return;
    }
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
            Console.Clear();
            for (int j = 0; j < lootFound.Count; j++)
            {
                Console.WriteLine($"{j + 1}. {lootFound[j].Name}");
            }
            Console.WriteLine($"Enter the number of the item you want to take (1-{lootFound.Count}):");
            string itemInput = Console.ReadLine();
            if (int.TryParse(itemInput, out int itemIndex) && itemIndex >= 1 && itemIndex <= lootFound.Count)
            {
                Item selectedItem = lootFound[itemIndex - 1];
                inventory.AddItem(selectedItem);
                if (inventory.items.Contains(selectedItem))
                {

                    Console.WriteLine($"You took {selectedItem.Name}.");
                    hero.UpdateHeroStats();
                    lootFound.RemoveAt(itemIndex - 1); // Remove the item from the loot list
                }
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
bool CheckRequirements(Room moveTo)
{
    foreach (var Edge in m.AdjacencyList[currentRoom])
    {
        if (Edge.To.Name == moveTo.Name)
        {
            if (Edge.RequirementStat == Edge.Stat.Strength && hero.Strength < Edge.Requirement)
            {
                Console.WriteLine($"You need at least {Edge.Requirement} strength to move to {moveTo.Name}.");

                return UseLockPick();
            }
            else if (Edge.RequirementStat == Edge.Stat.Agility && hero.Agility < Edge.Requirement)
            {
                Console.WriteLine($"You need at least {Edge.Requirement} agility to move to {moveTo.Name}.");
                return UseLockPick();
            }
            else if (Edge.RequirementStat == Edge.Stat.Intelligence && hero.Intelligence < Edge.Requirement)
            {
                Console.WriteLine($"You need at least {Edge.Requirement} intelligence to move to {moveTo.Name}.");
                return UseLockPick();
            }
        }
    }
    return true;
}

bool UseLockPick()
{
    foreach (Item i in inventory.items)
    {
        if (i.Name == "Lockpick")
        {
            Console.WriteLine("You have a lockpick. Do you want to use it? (y/n)");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Y)
            {
                inventory.RemoveItem(i, hero);
                Console.WriteLine("You used the lockpick. You can now move to this room.");
                return true;
            }
            else if (keyInfo.Key == ConsoleKey.N)
            {
                return false;
            }
        }
    }
    Console.WriteLine("You don't have a lockpick or you chose not to use it. You cannot move to this room.");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
    return false;
}

