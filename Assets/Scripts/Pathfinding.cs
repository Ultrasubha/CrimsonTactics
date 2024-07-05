using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static List<Vector3> FindPath(Vector3 startPosition, Vector3 targetPosition, GridManagerScript gridManager)
    {
        Vector2Int startGridPos = new Vector2Int(Mathf.RoundToInt(startPosition.x), Mathf.RoundToInt(startPosition.z));
        Vector2Int targetGridPos = new Vector2Int(Mathf.RoundToInt(targetPosition.x), Mathf.RoundToInt(targetPosition.z));

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        Node startNode = new Node(startGridPos);
        Node targetNode = new Node(targetGridPos);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.position == targetNode.position)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (Node neighbour in GetNeighbours(currentNode, gridManager))
            {
                if (closedList.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    private static List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(new Vector3(currentNode.position.x, 0.5f, currentNode.position.y));
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        int dstY = Mathf.Abs(nodeA.position.y - nodeB.position.y);
        return dstX + dstY;
    }

    private static List<Node> GetNeighbours(Node node, GridManagerScript gridManager)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                Vector2Int neighbourPos = new Vector2Int(node.position.x + x, node.position.y + y);

                if (neighbourPos.x >= 0 && neighbourPos.x < 10 && neighbourPos.y >= 0 && neighbourPos.y < 10 && !gridManager.IsTileBlocked(neighbourPos.x, neighbourPos.y))
                {
                    neighbours.Add(new Node(neighbourPos));
                }
            }
        }

        return neighbours;
    }

    private class Node
    {
        public Vector2Int position;
        public int gCost;
        public int hCost;
        public Node parent;

        public Node(Vector2Int position)
        {
            this.position = position;
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }
    }
}
