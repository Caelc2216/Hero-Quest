public class Map
{
    public Dictionary<Room, List<Room>> AdjacencyList { get; set; }

    public Map()
    {
        AdjacencyList = new Dictionary<Room, List<Room>>();
    }

    public void AddRoom(Room room)
    {
        if (!AdjacencyList.ContainsKey(room))
        {
            AdjacencyList[room] = new List<Room>();
        }
    }

    public void AddPath(Room room1, Edge path)
    {
        if (AdjacencyList.ContainsKey(room1)) /* if directional delete && AdjacencyList.ContainsKey(vertex2) */
        {
            if (!AdjacencyList[room1].Contains(path.To))
            {
                AdjacencyList[room1].Add(path.To);
            }
        }
    }

    public void RemoveRoom(Room room)
    {
        if (!HasRoom(room))
        {
            Console.WriteLine($"{room} does not exist.");
            return;
        }
        // Remove all edges that contain this vertex
        foreach (var r in AdjacencyList)
        {
            for (int i = 0; i < r.Value.Count; i++)
            {
                if (r.Value[i] == room)
                {
                    r.Value.RemoveAt(i);
                }
            }
        }
        // Remove vertex
        AdjacencyList.Remove(room);
        Console.WriteLine($"{room} has been removed from the map.");
    }

    public bool HasPath(Room startVertex, Room endVertex)
    {
        return AdjacencyList[startVertex].Contains(endVertex);
    }

    public bool HasRoom(Room vertex)
    {
        return AdjacencyList.ContainsKey(vertex);
    }

    // Breadth-First Search
    public bool BFS(Room start, Room target)
    {
        Queue<Room> queue = new Queue<Room>();
        HashSet<Room> visited = new HashSet<Room>();
        Dictionary<Room, Room> parent = new Dictionary<Room, Room>();

        queue.Enqueue(start);
        visited.Add(start);
        parent[start] = null;

        while (queue.Count > 0)
        {
            Room currentVertex = queue.Dequeue();

            if (currentVertex == target)
            {
                List<Room> path = new List<Room>();
                while (currentVertex != null)
                {
                    path.Add(currentVertex);
                    currentVertex = parent[currentVertex];
                }

                path.Reverse();
                Console.Write("Path: ");
                foreach (var item in path)
                {
                    Console.Write($"{item} ");
                }
                return true;
            }

            foreach (var vertex in AdjacencyList[currentVertex])
            {
                if (!visited.Contains(vertex))
                {
                    queue.Enqueue(vertex);
                    visited.Add(vertex);
                    parent[vertex] = currentVertex;
                }
            }
        }
        return false;
    }

    // Depth-First Search
    public bool DepthFirstSearch(Room start, Room target)
    {
        HashSet<Room> visited = new HashSet<Room>();
        List<Room> path = new List<Room>();
        return DepthFirstSearchRecursive(start, target, visited, path);
    }

    public bool DepthFirstSearchRecursive(Room currentVertex, Room target, HashSet<Room> visited, List<Room> path)
    {
        path.Add(currentVertex);
        visited.Add(currentVertex);

        if (currentVertex == target)
        {
            Console.Write("Path: ");
            foreach (var item in path)
            {
                Console.Write($"{item} ");
            }
            return true;
        }

        foreach (var vertex in AdjacencyList[currentVertex])
        {
            if (!visited.Contains(vertex))
            {
                if (DepthFirstSearchRecursive(vertex, target, visited, path))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void RemovePath(Room startVertex, Room endVertex)
    {
        AdjacencyList[startVertex].Remove(endVertex);
    }

    public void DisplayMap()
    {
        foreach (var vertex in AdjacencyList)
        {
            Console.Write($"{vertex.Key.Name}: ");
            foreach (var edge in vertex.Value)
            {
                Console.Write($"{edge.Name} ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    public List<Room> InitializeMap(int numOfRooms, double edgeProbability)
    {
        List<Room> rooms = new List<Room>();

        for (int i = 0; i < numOfRooms; i++)
        {
            Room newRoom = new Room { Name = $"Room {i + 1}", Number = i + 1, IsExit = false, IsStart = false };
            AddRoom(newRoom);
            rooms.Add(newRoom);
        }

        Random rand = new Random();

        int startIndex = rand.Next(numOfRooms);
        int endIndex;

        //Selects a random endIndex different from startIndex
        do
        {
            endIndex = rand.Next(numOfRooms);
        } while (endIndex == startIndex);

        Room start = rooms[startIndex];
        Room end = rooms[endIndex];

        start.IsStart = true;
        end.IsExit = true;

        var pathRooms = new List<Room>();
        foreach (var room in rooms)
        {
            if (room != start && room != end)
            {
                pathRooms.Add(room);
            }
        }

        pathRooms.Remove(start);
        pathRooms.Remove(end);

        Random rand2 = new Random(4);
        var path = new List<Room>();
        //starts with start
        path.Add(start);

        //Add random rooms in between
        for (int i = 0; i < rand2.Next(); i++)
        {
            path.Add(pathRooms[rand2.Next(pathRooms.Count)]);
        }

        //Add end
        path.Add(end);

        //Set Up guarented path
        for (int i = 0; i < path.Count - 1; i++)
        {
            AddPath(path[i], new Edge(path[i + 1]));
        }

        //Add random paths between rooms
        foreach (var room in rooms)
        {
            foreach (var to in rooms)
            {
                if (room.Name != to.Name && !HasPath(room, to) && rand.NextDouble() < edgeProbability)
                {
                    AddPath(room, new Edge(to));
                }
                if (HasPath(room, room))
                {
                    RemovePath(room, room);
                }
            }
        }
        return path;

    }
}


