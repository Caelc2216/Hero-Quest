using System.Runtime;

public class CustomBinaryTree
{
    public Challenge? RootNode;

    public CustomBinaryTree()
    {
        RootNode = null;
    }

    public void Insert(Challenge data)
    {
        RootNode = InsertNode(RootNode, data);

    }

    public Challenge InsertNode(Challenge? node, Challenge data)
    {
        if (node == null)
        {
            return new Challenge();
        }

        if (data.Difficulty < node.Difficulty)
        {
            node.Left = InsertNode(node.Left, data);
        }
        else if (data.Difficulty > node.Difficulty)
        {
            node.Right = InsertNode(node.Right, data);
        }
        return node;
    }

    public void Display()
    {
        InOrderTraversal(RootNode);
    }

    public void InOrderTraversal(Challenge? node)
    {
        if (node == null)
        {
            return;
        }

        InOrderTraversal(node.Left);
        Console.WriteLine(node.Difficulty);
        InOrderTraversal(node.Right);
    }

    public void DecsendingOrderTraversal()
    {
        DecsendingOrderTraversal(RootNode);
    }

    public void DecsendingOrderTraversal(Challenge? node)
    {
        if (node == null)
        {
            return;
        }

        DecsendingOrderTraversal(node.Right);
        Console.WriteLine(node.Difficulty);
        DecsendingOrderTraversal(node.Left);
    }

    public void PreOrderTraversal()
    {
        PreOrderTraversal(RootNode);
    }

    public void PreOrderTraversal(Challenge? node)
    {
        if (node == null)
        {
            return;
        }

        Console.WriteLine(node.Difficulty);
        PreOrderTraversal(node.Left);
        PreOrderTraversal(node.Right);
    }

    public void LevelOrderTraversal()
    {
        if (RootNode == null)
        {
            return;
        }

        Queue<Challenge> queue = new();

        queue.Enqueue(RootNode);

        while (queue.Count > 0)
        {
            Challenge current = queue.Dequeue();
            Console.WriteLine(current.Difficulty);

            if (current.Left != null)
            {
                queue.Enqueue(current.Left);
            }

            if (current.Right != null)
            {
                queue.Enqueue(current.Right);
            }
        }
    }

    public Challenge Search(int target)
    {
        return SearchRecursive(RootNode, target);
    }

    /*Delete
    1. Leaf
    2. 1 Kid - Parent -> Child
    3. 2 kid
        smallest right or largest left
    */
    public void DeleteNode(Challenge? target)
    {
        if (target == null)
        {
            return;
        }
        DeleteNode(target.Difficulty);
    }
    public void DeleteNode(int target)
    {
        RootNode = DeleteNode(RootNode, target);
    }
    public Challenge? DeleteNode(Challenge? currentNode, int target)
    {
        if (currentNode == null)
        {
            return currentNode;
        }

        if (target < currentNode.Difficulty)
        {
            currentNode.Left = DeleteNode(currentNode.Left, target); //search left
        }
        else if (target > currentNode.Difficulty)
        {
            currentNode.Right = DeleteNode(currentNode.Left, target); //search right

        }
        else
        {
            //found the node

            //leaf
            if (currentNode.Left == null && currentNode.Right == null)
            {
                return null;
            }
            //1 kid
            if (currentNode.Left == null || currentNode.Right == null)
            {
                Challenge? result = currentNode.Left == null ? currentNode.Right : currentNode.Left;
                return result;
            }
            //2 kid
            currentNode.Difficulty = GetMinValue(currentNode.Right);
            currentNode.Right = DeleteNode(currentNode.Right, currentNode.Difficulty);

        }
        return currentNode;
    }

    //     Node right = node;
    //     Node left = node;
    //     int rightSide = -1;
    //     int leftSide = -1;
    //     while (right != null)
    //     {
    //         right = right.Right;
    //         rightSide++;
    //     }

    //     while (left != null)
    //     {
    //         left = left.Left;
    //         leftSide++;
    //     }
    //     return leftSide > rightSide ? leftSide : rightSide;
    public void GetHeight()
    {
        GetHeight(RootNode);
    }

    public static int GetHeight(Challenge? node)
    {
        if (node == null)
        {
            return 0;
        }

        int leftHeight = GetHeight(node.Left);
        if (leftHeight == -1)
        {
            return -1;
        }
        
        int rightHeight = GetHeight(node.Right);
        if (rightHeight == -1)
        {
            return -1;
        }

        if (Math.Abs(leftHeight - rightHeight) > 1)
        {
            return -1;
        }

        return leftHeight > rightHeight ? leftHeight + 1 : rightHeight + 1;
    }

    public bool IsBalanced()
    {
        return GetHeight(RootNode) != -1;
    }
    public int GetDepth(Challenge? target)
    {
        if (target == null)
        {
            return -1;    
        }
        Challenge? node = RootNode;
        int i = 0;
        while (node != target)
        {
            if (target.Difficulty > node?.Difficulty)
            {
                node=node.Right;
            }
            else node=node?.Left;
            i++;
        }
        
        return i;
    }



    public Challenge SearchRecursive(Challenge? node, int target)
    {

        if (node == null)
        {
            return null;
        }

        if (target == node.Difficulty)
        {
            return node;
        }

        if (target < node.Difficulty)
        {
            return SearchRecursive(node.Left, target);
        }
        else
        {
            return SearchRecursive(node.Right, target);
        }
    }

    public int GetMinValue(Challenge? currentNode = null)
    {
        Challenge? result = currentNode ?? RootNode;

        if (result == null)
        {
            throw new InvalidOperationException("Tree is empty");
        }

        while (result.Left != null)
        {
            result = result.Left;
        }

        return result.Difficulty;
    }


    public int GetMaxValue(Challenge? currentNode = null)
    {
        Challenge? result = RootNode;
        if (currentNode == null)
        {
            result = RootNode;
        }

        if (result != null)
        {
            while (result.Right != null)
            {
                result = result.Right;
            }
            return result.Difficulty;
        }
        return -1;
    }

    public void InitializeTree(int numberOfChallenges)
    {
        for (int i = 0; i < numberOfChallenges; i++)
        {
            Challenge newChallenge = new Challenge();
            Insert(newChallenge);
        }
    }

    public void Rebalance()
    {

    }

    public Challenge ClosestNode(int target)
    {
        return ClosestNodeRecursive(RootNode, target, RootNode);
    }

    
    private Challenge ClosestNodeRecursive(Challenge node, double target, Challenge closest)
    {
        if (node == null)
            return closest;

        // Update closest if current node is closer to target
        if (Math.Abs(node.Difficulty - target) < Math.Abs(closest.Difficulty - target))
        {
            closest = node;
        }

        // Recurse left or right depending on target value
        if (target < node.Difficulty)
        {
            return ClosestNodeRecursive(node.Left, target, closest);
        }
        else
        {
            return ClosestNodeRecursive(node.Right, target, closest);
        }
    }
}