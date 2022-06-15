using UnityEngine;
using System.Collections;
using System;
public class Node
{
    public bool isObstacle;
    public int xCoor;
    public int yCoor;
    public int hCost;
    public int gCost;
    public int index;
    public Node parent;
    public GameObject entity { get; set; }
    private int type;

    public int Type {
        get {
            return type;
        }
        set {
            type = value;
            isObstacle = (type != 1 && type != 2 && type != 4 && type != 5);
        }
    }

    public Node(int x, int y, int type)
    {
        isObstacle = (type != 1 && type != 2 && type != 4 && type != 5);
        xCoor = x;
        yCoor = y;
        this.type = type;
        entity = null;
    }

    public int FCost()
    {
        return hCost + gCost;
    }

    public int CompareTo(Node node)
    {
        if (this.FCost() > node.FCost())
        {
            return 1;
        }
        else if (this.FCost() < node.FCost())
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public bool Equals(Node a, Node b)
    {
        return a.FCost() == b.FCost();
    }
}