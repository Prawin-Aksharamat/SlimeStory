using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public Node[,] map; //actual 2Darray
    private int[] boundaryBox;
    private int width;
    private int height;

    /*
     public int getSizeY()
        {
            return Mathf.Abs(partitionBoundaryBox[1] - partitionBoundaryBox[3]) + 1;
        }

        public int getSizeX()
        {
            return Mathf.Abs(partitionBoundaryBox[0] - partitionBoundaryBox[2]) + 1;
        }
         */


    public void SetMap(int[,] gridMap,int[] boundaryBox)
    {
        height = gridMap.GetLength(0);
        width = gridMap.GetLength(1);
        map = new Node[height, width];
        for(int i=0; i<height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int currentTileValue = gridMap[i, j];
                map[i, j] = new Node(i, j, currentTileValue);
            }
        }
        this.boundaryBox = boundaryBox;
    }

    public Node[,] GetMap()
    {
        return map;
    }

    public Node UpdateMap(Vector3 currentPos,Vector3 nextPos,int value,GameObject entity)
    {
        int x = WorldCoordinateXToGridmapCoordinateX((int)currentPos.x);
        int y = WorldCoordinateYToGridmapCoordinateY((int)currentPos.y);

        int xNext = WorldCoordinateXToGridmapCoordinateX((int)nextPos.x);
        int yNext = WorldCoordinateYToGridmapCoordinateY((int)nextPos.y);

        map[x,y].Type = 1;
        map[x,y].entity = null;
        map[xNext,yNext].Type = value; //will likely change to string later
        map[xNext, yNext].entity = entity;

        return map[xNext, yNext];
    }
    public Node UpdateMap(Vector3 currentPos, int value, GameObject entity)
    {
        int x = WorldCoordinateXToGridmapCoordinateX((int)currentPos.x);
        int y = WorldCoordinateYToGridmapCoordinateY((int)currentPos.y);
        map[x, y].Type = value;
        map[x, y].entity = entity;

        return map[x, y];
    }

    public void UpdateMap(Node node, int value, GameObject entity)
    {

            node.Type = value;
            node.entity = entity;

    }

    public int WorldCoordinateXToGridmapCoordinateX(int x)
    {
        return x + Math.Abs(boundaryBox[2]);
    }

    public int WorldCoordinateYToGridmapCoordinateY(int y)
    {
        return y + Math.Abs(boundaryBox[3]);
    }

    public int GridmapCoordinateXToWorldCoordinateX(int x)
    {
        return x - Math.Abs(boundaryBox[2]);
    }

    public int GridmapCoordinateYToWorldCoordinateY(int y)
    {
        return y - Math.Abs(boundaryBox[3]);
    }

    public bool CantWalk(int x, int y)
    {
        /*Debug.Log(WorldCoordinateXToGridmapCoordinateX(x) + " " + WorldCoordinateYToGridmapCoordinateY(y));
        Debug.Log(map[WorldCoordinateXToGridmapCoordinateX(x), WorldCoordinateYToGridmapCoordinateY(y)]);*/
        //Debug.Log(map[WorldCoordinateXToGridmapCoordinateX(x), WorldCoordinateYToGridmapCoordinateY(y)].Type);
        return map[WorldCoordinateXToGridmapCoordinateX(x), WorldCoordinateYToGridmapCoordinateY(y)].Type != 1;
    }

    public List<Node> GetNeighbours(Node node) //incomplete
    {
        List<Node> list = new List<Node>();

        int x = node.xCoor;
        int y = node.yCoor;
        if (x - 1 >= 0) { list.Add(map[x - 1, y]); };
        if (y - 1 >= 0) { list.Add(map[x, y - 1]); };
        if (x + 1 < width) { list.Add(map[x + 1, y]); };
        if (y + 1 < height) { list.Add(map[x, y + 1]); };

        /*
        for (int i = node.xCoor - 1; i<node.xCoor+2 ; i++)
        {
            for (int j = node.yCoor - 1; j<node.yCoor+2 ; j++)
            {
                if (i<0 || j<0 || i>=width || j>=height || i==node.xCoor && j==node.yCoor)
                {
                    continue;
                }
                list.Add(map[i, j]);
            }
        }*/
        
        return list;
    }
}