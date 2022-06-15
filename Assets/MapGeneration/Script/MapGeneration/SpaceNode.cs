using Rogue.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Map
{
    public class SpaceNode
    {
        int[] partitionBoundaryBox;
        int[,] gridMap;


        GameObject gizmozObject=null;

        ArrayList dummyAllChildNode;
        SpaceNode[] allChildNode;

        SpaceNode parent;
        SpaceNode left;
        SpaceNode right;

        Biome biome;
        Room room;


        public SpaceNode(int[] boundaryBox)
        {
            this.partitionBoundaryBox = boundaryBox;
            room = null;
            left = null;
            right = null;
            biome = null;
            parent = null;
            allChildNode = null;
            gizmozObject = null;
            gridMap = new int[getSizeX(), getSizeY()];
        }

        private int[] getSize() => new int[] { getSizeX(), getSizeY() };

        public int getSizeY()
        {
            return Mathf.Abs(partitionBoundaryBox[1] - partitionBoundaryBox[3]) + 1;
        }

        public int getSizeX()
        {
            return Mathf.Abs(partitionBoundaryBox[0] - partitionBoundaryBox[2]) + 1;
        }

        public SpaceNode[] getAllChildsFormCurrentNode()
        {
            if (allChildNode == null)
            {
                dummyAllChildNode = new ArrayList();
                traversePreOrder(this);
                object[] allNode = dummyAllChildNode.ToArray();

                allChildNode = new SpaceNode[allNode.Length];
                for (int i = 0; i < allNode.Length; i++)
                {
                    allChildNode[i] = (SpaceNode)allNode[i];
                }
            }

            return allChildNode;
        }

        private void traversePreOrder(SpaceNode node)
        {
            if (node != null)
            {
                dummyAllChildNode.Add(node);
                traversePreOrder(node.left);
                traversePreOrder(node.right);
            }
        }

        public float getCenterX()
        {
            return (partitionBoundaryBox[0] + partitionBoundaryBox[2]) / 2;
        }

        public float getCenterY()
        {
            return (partitionBoundaryBox[1] + partitionBoundaryBox[3]) / 2;
        }

        public int[] getBoundaryBox() => partitionBoundaryBox;
        public SpaceNode getLeft() => left;
        public SpaceNode getRight() => right;
        public Biome getBiome() => biome;
        public SpaceNode getParent() => parent;
        public int[,] getGridMap() => gridMap;
        public GameObject getGizmozObject() => gizmozObject;

        public void setGridMapAtPoint(int i, int j, int value)
        {
            gridMap[i, j] = value;
            setParentGridMapFromCurrentPoint(i, j, value);
            setChildGridMapFromCurrentPoint(i, j, value);

        }

        private void setChildGridMapFromCurrentPoint(int i, int j, int value)
        {
            if (isLeafNode()) return;
            int[,] leftChildGridMap = this.getLeft().getGridMap();

            if (i < leftChildGridMap.GetLength(0) && j < leftChildGridMap.GetLength(1))
            {
                left.getGridMap()[i, j] = value;
                this.getLeft().setChildGridMapFromCurrentPoint(i, j, value);
            }
            else
            {
                right.getGridMap()[getRightChildCoordinateX(i), getRightChildCoordinateY(j)] = value;
                this.getRight().setChildGridMapFromCurrentPoint(getRightChildCoordinateX(i), getRightChildCoordinateY(j), value);
            }
        }

        private void setParentGridMapFromCurrentPoint(int i, int j, int value)
        {
            if (parent == null) return;
            parent.getGridMap()[getParentCoordinateX(i), getParentCoordinateY(j)] = value;
            parent.setParentGridMapFromCurrentPoint(getParentCoordinateX(i), getParentCoordinateY(j), value);
        }

        public void setGridMap(int[,] gridMap)
        {
            this.gridMap = gridMap;

            setGridMapToParent(gridMap);
            setGridMapToChild(gridMap);

        }

        private void setGridMapToParent(int[,] gridMap)
        {
            if (parent == null)
            {
                return;
            }

            int[,] parentGridMap = parent.getGridMap();

            if (parent.getRight() == this)
            {
                int parentX = partitionBoundaryBox[2] - parent.getBoundaryBox()[2];
                int parentY = partitionBoundaryBox[3] - parent.getBoundaryBox()[3];
                for (int i = parentX; i < parent.getSizeX(); i++)
                {
                    for (int j = parentY; j < parent.getSizeY(); j++)
                    {
                        parentGridMap[i, j] = gridMap[i - parentX, j - parentY];
                    }
                }
            }
            else
            {
                for (int i = 0; i < gridMap.GetLength(0); i++)
                {
                    for (int j = 0; j < gridMap.GetLength(1); j++)
                    {
                        parentGridMap[i, j] = gridMap[i, j];
                    }
                }
            }
            parent.setGridMapToParent(parentGridMap);
        }

        public SpaceNode getNodeFromWorldCoordinate(int x, int y)
        {

            if (this.getLeft() == null && this.getRight() == null) return this;

            if (x <= this.getLeft().getBoundaryBox()[0] && y <= this.getLeft().getBoundaryBox()[1])
            {
                return this.getLeft().getNodeFromWorldCoordinate(x, y);
            }
            else
            {
                return this.getRight().getNodeFromWorldCoordinate(x, y);
            }
        }

        private void setGridMapToChild(int[,] gridMap)
        {
            if (isLeafNode()) return;

            int[,] leftChildGridMap = left.getGridMap();
            int[,] rightChildGridMap = right.getGridMap();


            for (int i = 0; i < gridMap.GetLength(0); i++)
            {
                for (int j = 0; j < gridMap.GetLength(1); j++)
                {
                    if (i < leftChildGridMap.GetLength(0) && j < leftChildGridMap.GetLength(1))
                    {
                        leftChildGridMap[i, j] = gridMap[i, j];
                    }
                    else
                    {
                        rightChildGridMap[getRightChildCoordinateX(i), getRightChildCoordinateY(j)] = gridMap[i, j];
                    }
                }
            }

            right.setGridMapToChild(rightChildGridMap);
            left.setGridMapToChild(leftChildGridMap);

        }

        public void setRoom(Room room) => this.room = room;
        public Room getRoom() => room;

        public void setLeft(SpaceNode left)
        {
            this.left = left;
            left.setParent(this);
        }
        public void setRight(SpaceNode right)
        {
            this.right = right;
            right.setParent(this);
        }
        private void setParent(SpaceNode parent) => this.parent = parent;
        public void setBiome(Biome biome) => this.biome = biome;
        public void setGizmozObject(GameObject gizmozObject) => this.gizmozObject = gizmozObject;

        public bool isLeafNode() => this.getLeft() == null && this.getRight() == null;

        public int[] getRandomCoordinate() =>
             new int[]
                { getRandomCoordinateX(),
                getRandomCoordinateY()
                };

        public int getRandomCoordinateY()
        {
            return Mathf.RoundToInt(UnityEngine.Random.Range(0, this.getSizeY()));
        }

        public int getRandomCoordinateX()
        {
            return Mathf.RoundToInt(UnityEngine.Random.Range(0, this.getSizeX()));
        }

        public int getParentCoordinateX(int x) => this.getParent().getLeft() == this ? x : x + (this.getParent().getSizeX() - this.getSizeX());
        public int getParentCoordinateY(int y) => this.getParent().getLeft() == this ? y : y + (this.getParent().getSizeY() - this.getSizeY());

        public int getWorldCoordinateX(int x)
        {
            int resultX=x;
            SpaceNode node = this;
            while (node.getParent()!= null)
            {
                resultX=node.getParentCoordinateX(resultX);
                node = node.getParent();
            }
            return resultX;
        }

        public int getWorldCoordinateY(int y)
        {
            int resultY = y;
            SpaceNode node = this;
            while (node.getParent() != null)
            {
                resultY = node.getParentCoordinateY(resultY);
                node = node.getParent();
            }
            return resultY;
        }

        public int getRightChildCoordinateX(int x) => x - (this.getSizeX() - this.right.getSizeX());
        public int getRightChildCoordinateY(int y) => y - (this.getSizeY() - this.right.getSizeY());

        public Dictionary<string,int> getSurroundingTiles(int i,int j,SpaceNode root)
        {
            int dungeonPositionX = i + partitionBoundaryBox[2];
            int dungeonPositionY = j + partitionBoundaryBox[3];
            int right = i + 1 >= gridMap.GetLength(0) ? i : i + 1;
            int left = i - 1 < 0 ? i : i - 1;
            int top = j + 1 >= gridMap.GetLength(1) ? j : j + 1;
            int bottom = j - 1 < 0 ? j : j - 1;

            bool isRightInMap = dungeonPositionX + 1 <= root.getBoundaryBox()[0];
            bool isLeftInMap = dungeonPositionX - 1 >= root.getBoundaryBox()[2];
            bool isTopInMap = dungeonPositionY + 1 <= root.getBoundaryBox()[1];
            bool isBottomInMap = dungeonPositionY - 1 >= root.getBoundaryBox()[3];


            int rightTile;
            if (isRightInMap)
            {
                SpaceNode partition = root.getNodeFromWorldCoordinate(dungeonPositionX + 1, dungeonPositionY);
                rightTile = partition.getGridMap()[dungeonPositionX + 1 - partition.getBoundaryBox()[2], dungeonPositionY - partition.getBoundaryBox()[3]];
            }
            else rightTile = 0;

            int leftTile;
            if (isLeftInMap)
            {
                SpaceNode partition = root.getNodeFromWorldCoordinate(dungeonPositionX - 1, dungeonPositionY);
                leftTile = partition.getGridMap()[dungeonPositionX - 1 - partition.getBoundaryBox()[2], dungeonPositionY - partition.getBoundaryBox()[3]];
            }
            else leftTile = 0;

            int topTile;
            if (isTopInMap)
            {
                SpaceNode partition = root.getNodeFromWorldCoordinate(dungeonPositionX, dungeonPositionY + 1);
                topTile = partition.getGridMap()[dungeonPositionX - partition.getBoundaryBox()[2], dungeonPositionY + 1 - partition.getBoundaryBox()[3]];
            }
            else topTile = 0;

            int bottomTile;
            if (isBottomInMap)
            {
                SpaceNode partition = root.getNodeFromWorldCoordinate(dungeonPositionX, dungeonPositionY - 1);
                bottomTile = partition.getGridMap()[dungeonPositionX - partition.getBoundaryBox()[2], dungeonPositionY - 1 - partition.getBoundaryBox()[3]];
            }
            else bottomTile = 0;

            int topRightTile;
            if (isTopInMap && isRightInMap)
            {
                SpaceNode partition = root.getNodeFromWorldCoordinate(dungeonPositionX + 1, dungeonPositionY + 1);
                topRightTile = partition.getGridMap()[dungeonPositionX + 1 - partition.getBoundaryBox()[2], dungeonPositionY + 1 - partition.getBoundaryBox()[3]];
            }
            else topRightTile = 0;

            int topLeftTile;
            if (isTopInMap && isLeftInMap)
            {
                SpaceNode partition = root.getNodeFromWorldCoordinate(dungeonPositionX - 1, dungeonPositionY + 1);
                topLeftTile = partition.getGridMap()[dungeonPositionX - 1 - partition.getBoundaryBox()[2], dungeonPositionY + 1 - partition.getBoundaryBox()[3]];
            }
            else topLeftTile = 0;

            int bottomLeftTile;
            if (isBottomInMap && isLeftInMap)
            {
                SpaceNode partition = root.getNodeFromWorldCoordinate(dungeonPositionX - 1, dungeonPositionY - 1);
                bottomLeftTile = partition.getGridMap()[dungeonPositionX - 1 - partition.getBoundaryBox()[2], dungeonPositionY - 1 - partition.getBoundaryBox()[3]];
            }
            else bottomLeftTile = 0;

            int bottomRightTile;
            if (isBottomInMap && isRightInMap)
            {
                SpaceNode partition = root.getNodeFromWorldCoordinate(dungeonPositionX + 1, dungeonPositionY - 1);
                bottomRightTile = partition.getGridMap()[dungeonPositionX + 1 - partition.getBoundaryBox()[2], dungeonPositionY - 1 - partition.getBoundaryBox()[3]];
            }
            else bottomRightTile = 0;

            Dictionary<string,int> EightTile = new Dictionary<string,int>();
            EightTile.Add("r", rightTile);
            EightTile.Add("l", leftTile);
            EightTile.Add("t", topTile);
            EightTile.Add("b", bottomTile);
            EightTile.Add("tr", topRightTile);
            EightTile.Add("tl", topLeftTile);
            EightTile.Add("br", bottomRightTile);
            EightTile.Add("bl", bottomLeftTile);
            return EightTile;
        }
    }
}
