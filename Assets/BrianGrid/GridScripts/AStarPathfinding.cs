using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    private GridManager grid;

    private void Awake()
    {
        grid = FindObjectOfType<GridManager>();
    }

    private class Node
    {
        public int x;
        public int y;
        public int gCost;
        public int hCost;
        public int fCost => gCost + hCost;
        public Node parent;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;

            // IMPORTANT FIX
            gCost = int.MaxValue;
        }
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        Node[,] nodes = new Node[grid.width, grid.height];

        for (int x = 0; x < grid.width; x++)
            for (int y = 0; y < grid.height; y++)
                nodes[x, y] = new Node(x, y);

        List<Node> openList = new();
        HashSet<Node> closedList = new();

        Node startNode = nodes[start.x, start.y];
        Node targetNode = nodes[target.x, target.y];

        startNode.gCost = 0;
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node current = openList[0];

            for (int i = 1; i < openList.Count; i++)
                if (openList[i].fCost < current.fCost)
                    current = openList[i];

            openList.Remove(current);
            closedList.Add(current);

            if (current == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbour in GetNeighbours(nodes, current))
            {
                if (closedList.Contains(neighbour)) continue;

                if (!grid.IsValidTile(neighbour.x, neighbour.y)) continue;

                int costToNeighbour =
                    current.gCost +
                    grid.tiles[neighbour.x, neighbour.y].moveCost;

                if (costToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = costToNeighbour;
                    neighbour.hCost = Mathf.Abs(neighbour.x - target.x) + Mathf.Abs(neighbour.y - target.y);
                    neighbour.parent = current;

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }
            }
        }

        return null;
    }

    private List<Node> GetNeighbours(Node[,] nodes, Node node)
    {
        List<Node> neighbours = new();

        int x = node.x;
        int y = node.y;

        TryAdd(x + 1, y);
        TryAdd(x - 1, y);
        TryAdd(x, y + 1);
        TryAdd(x, y - 1);

        return neighbours;

        void TryAdd(int nx, int ny)
        {
            if (nx >= 0 && ny >= 0 && nx < grid.width && ny < grid.height)
                neighbours.Add(nodes[nx, ny]);
        }
    }

    private List<Vector2Int> RetracePath(Node start, Node end)
    {
        List<Vector2Int> path = new();
        Node current = end;

        while (current != start)
        {
            path.Add(new Vector2Int(current.x, current.y));
            current = current.parent;
        }

        path.Reverse();
        return path;
    }
}
