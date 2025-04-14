public class Map
{
    public Dictionary<Room, List<Edge>> AdjacencyList { get; set; }

    public Map()
    {
        AdjacencyList = new Dictionary<Room, List<Edge>>();
    }

    public void AddRoom(Room room)
    {
        if (!AdjacencyList.ContainsKey(room))
        {
            AdjacencyList[room] = new List<Edge>();
        }
    }

    public void AddPath(Room room1, Edge path)
    {
        if (AdjacencyList.ContainsKey(room1)) /* if directional delete && AdjacencyList.ContainsKey(vertex2) */
        {
            if (!AdjacencyList[room1].Contains(path))
            {
                AdjacencyList[room1].Add(path);
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
                if (r.Value[i].To == room)
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
        foreach (var edge in AdjacencyList[startVertex])
        {
            if (edge.To == endVertex)
            {
                return true;
            }
        }
        return false;
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
                if (!visited.Contains(vertex.To))
                {
                    queue.Enqueue(vertex.To);
                    visited.Add(vertex.To);
                    parent[vertex.To] = currentVertex;
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
            if (!visited.Contains(vertex.To))
            {
                if (DepthFirstSearchRecursive(vertex.To, target, visited, path))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void RemovePath(Room startVertex, Room endVertex)
    {
        foreach (var edge in AdjacencyList[startVertex])
        {
            if (edge.To == endVertex)
            {
                AdjacencyList[startVertex].Remove(edge);
                break;
            }
        }
    }

    public void DisplayMap()
    {
        foreach (var vertex in AdjacencyList)
        {
            Console.Write($"{vertex.Key.Name}: ");
            foreach (var edge in vertex.Value)
            {
                Console.Write($"{edge.To.Name} ");
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

        Random rand2 = new Random();
        var path = new List<Room>();
        //starts with start
        path.Add(start);

        //Add random rooms in between
        for (int i = 0; i < rand2.Next(10, 16); i++)
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
            int connections = rand.Next(0, 8);
            int connectionsCount = 0;
            foreach (var to in rooms)
            {
                if (room.Name != to.Name && !HasPath(room, to))
                {
                    AddPath(room, new Edge(to));
                    connectionsCount++;
                }
                if (HasPath(room, room))
                {
                    RemovePath(room, room);
                }
                if (connectionsCount >= connections)
                {
                    break;
                }
            }
        }

        CanReachExit(rooms, start, end);
        return path;

    }

    public void CanReachExit(List<Room> rooms, Room start, Room end)
    {
        List<Room> canReachExit = new List<Room>();
        List<Room> cannotReach = new List<Room>();
        foreach (var room in rooms)
        {
            if (BFS(room, end))
            {
                canReachExit.Add(room);
            }
            else
            {
                cannotReach.Add(room);
            }
        }

        for (int i = 0; i < cannotReach.Count; i++)
        {
            Random rand = new ();
            AddPath(cannotReach[i], new Edge(canReachExit[rand.Next(canReachExit.Count)]));
        }
    }
}


