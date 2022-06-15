using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding:MonoBehaviour
{
    MapManager mapManager;
    GameObject gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        mapManager = gameManager.GetComponent<MapManager>();
    }


    public List<Node> FindPath(Node a, Node b)
    {
        Node startNode = a;
        Node targetNode = b;
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);



        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost() < currentNode.FCost() || (openSet[i].FCost() == currentNode.FCost() && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                //Debug.Log("Path Found");
                return RetracePath(startNode, targetNode);
            }
            foreach (Node neighbours in mapManager.GetNeighbours(currentNode))
            {

                if ((neighbours.isObstacle && neighbours != targetNode) || closedSet.Contains(neighbours))
                {
                    continue;
                }
                int newMoveCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbours);
                if (newMoveCostToNeighbour < neighbours.gCost || !openSet.Contains(neighbours))
                {
                    neighbours.gCost = newMoveCostToNeighbour;
                    neighbours.hCost = GetDistance(neighbours, targetNode);
                    neighbours.parent = currentNode;
                    if (!openSet.Contains(neighbours))
                    {
                        openSet.Add(neighbours);
                    }
                }
            }
        }
       // Debug.Log("can't find path");
        return null;
    }

    public List<Node> FindPathDebug(Node a, Node b,GameObject path)
    {
        Node startNode = a;
        Node targetNode = b;
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);



        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost() < currentNode.FCost() || (openSet[i].FCost() == currentNode.FCost() && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            Debug.Log("Current Node x: " + mapManager.GridmapCoordinateXToWorldCoordinateX(currentNode.xCoor) + "Current Node y: " + mapManager.GridmapCoordinateYToWorldCoordinateY(currentNode.yCoor));
            Instantiate(path, new Vector3(mapManager.GridmapCoordinateXToWorldCoordinateX(currentNode.xCoor),mapManager.GridmapCoordinateYToWorldCoordinateY(currentNode.yCoor), 0), Quaternion.identity);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                Debug.Log("Path Found");
                return RetracePath(startNode, targetNode);
            }
            foreach (Node neighbours in mapManager.GetNeighbours(currentNode))
            {

                if ((neighbours.isObstacle && neighbours != targetNode) || closedSet.Contains(neighbours))
                {
                    Debug.Log("Unqualified Neighbour of Current Node x: " + mapManager.GridmapCoordinateXToWorldCoordinateX(neighbours.xCoor) + " y: " + mapManager.GridmapCoordinateYToWorldCoordinateY(neighbours.yCoor));
                    continue;
                }
                int newMoveCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbours);
                if (newMoveCostToNeighbour < neighbours.gCost || !openSet.Contains(neighbours))
                {
                    neighbours.gCost = newMoveCostToNeighbour;
                    neighbours.hCost = GetDistance(neighbours, targetNode);
                    neighbours.parent = currentNode;
                    if (!openSet.Contains(neighbours))
                    {
                        Debug.Log("Qualified Neighbour of Current Node x: " + mapManager.GridmapCoordinateXToWorldCoordinateX(neighbours.xCoor) + " y: " + mapManager.GridmapCoordinateYToWorldCoordinateY(neighbours.yCoor));
                        openSet.Add(neighbours);
                    }
                }
            }
        }
        Debug.Log("can't find path");
        return null;
    }

    public List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            if (!currentNode.isObstacle)
            {
                path.Add(currentNode);
            }
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }



    public int GetDistance(Node a, Node b)
    {
        int distanceX = Mathf.Abs(a.xCoor - b.xCoor);
        int distanceY = Mathf.Abs(a.yCoor - b.yCoor);
        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}