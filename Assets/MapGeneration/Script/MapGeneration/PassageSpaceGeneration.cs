using System;
using Rogue.Data;
using System.Collections;
using System.Collections.Generic;

namespace Rogue.Map {
    public class PassageSpaceGeneration
    {
        public void StartPassageSpaceGeneration(SpaceNode root)
        {
            createPassageSpace(root);
            expand1TileDiagonalBlock(root);
            changeIncompletewallToPebble(root);
        }

        private static void changeIncompletewallToPebble(SpaceNode root)
        {
            int[,] fullGridMap = root.getGridMap();
            //มันเปลี่ยนเป็น 2 รอบนึงรอบหน้าก็ต้อง check อีก
            int change = -1;
            while (change != 0)
            {
                change = 0;
                for (int j = 0; j < fullGridMap.GetLength(0); j++)
                {
                    for (int k = 0; k < fullGridMap.GetLength(1); k++)
                    {
                        int x1 = j + 1 >= fullGridMap.GetLength(0) ? j : j + 1;
                        int x2 = j - 1 < 0 ? j : j - 1;
                        int y1 = k + 1 >= fullGridMap.GetLength(1) ? k : k + 1;
                        int y2 = k - 1 < 0 ? k : k - 1;

                        if (fullGridMap[j, k] == 0)
                        {
                            if (
                                (fullGridMap[j, y1] != 0
                                && fullGridMap[j, y2] != 0)
                                ||
                                (fullGridMap[x1, k] != 0 &&
                                fullGridMap[x2, k] != 0)
                                )
                            {
                                fullGridMap[j, k] = 2;
                                change++;
                            }

                            else if ((fullGridMap[x1, y1] != 0 &&
                                fullGridMap[x2, y2] != 0) && (
                                (fullGridMap[x2, y1] == 0 ||
                                fullGridMap[x1, y2] == 0))
                                ||
                                (fullGridMap[x2, y1] != 0 &&
                                fullGridMap[x1, y2] != 0
                                && (fullGridMap[x1, y1] == 0 ||
                                fullGridMap[x2, y2] == 0)))
                            {
                                fullGridMap[j, k] = 1;
                                change++;
                            }
                        }
                    }
                }
            }
            root.setGridMap(fullGridMap);
        }

        private static void expand1TileDiagonalBlock(SpaceNode root)
        {
            int[,] fullGridMap = root.getGridMap();

            for (int j = 0; j < fullGridMap.GetLength(0); j++)
            {
                for (int k = 0; k < fullGridMap.GetLength(1); k++)
                {
                    Dictionary<string, int> tiles=root.getSurroundingTiles(j, k,root);
                    if(
                        (
                        ((tiles["t"] == 0 && tiles["l"] == 0)|| (tiles["b"] == 0 && tiles["r"] == 0))
                        && (tiles["tl"] == 1 && tiles["br"] == 1)
                        )||
                        (
                        ((tiles["t"] == 0 && tiles["r"] == 0) || (tiles["b"] == 0 && tiles["l"] == 0))
                        && (tiles["tr"] == 1 && tiles["bl"] == 1)
                        )
                        )
                   
                    {
                        int x1 = j + 1 >= fullGridMap.GetLength(0) ? j : j + 1;
                        int x2 = j - 1 < 0 ? j : j - 1;
                        int y1 = k + 1 >= fullGridMap.GetLength(1) ? k : k + 1;
                        int y2 = k - 1 < 0 ? k : k - 1;

                        fullGridMap[x1,k] = 1;
                        fullGridMap[x2,k] = 1;
                        fullGridMap[j,y1] = 1;
                        fullGridMap[j, y2] = 1;
                    }              
                }
            }
            root.setGridMap(fullGridMap);
        }

        private static void createPassageSpace(SpaceNode root)
        {
            SpaceNode[] allNode = root.getAllChildsFormCurrentNode();
            Array.Reverse(allNode);
            foreach (SpaceNode node in allNode)
            {
                if (node.isLeafNode()) continue;
                Room room = node.getRoom();
                SpaceNode leftChild = node.getLeft();
                SpaceNode rightChild = node.getRight();

                int leftChildX = leftChild.getRandomCoordinateX();
                int leftChildY = leftChild.getRandomCoordinateY();

                while (leftChild.getGridMap()[leftChildX, leftChildY] != 1)
                {
                    leftChildX = leftChild.getRandomCoordinateX(); ;
                    leftChildY = leftChild.getRandomCoordinateY(); ;
                }

                int rightChildX = rightChild.getRandomCoordinateX();
                int rightChildY = rightChild.getRandomCoordinateY();
                while (rightChild.getGridMap()[rightChildX, rightChildY] != 1)
                {
                    rightChildX = rightChild.getRandomCoordinateX();
                    rightChildY = rightChild.getRandomCoordinateY();
                }

                //เปลี่ยน coor ของ right child ให้ตรงกับ parent
                rightChildX = rightChild.getParentCoordinateX(rightChildX);
                rightChildY = rightChild.getParentCoordinateY(rightChildY);

                int[] currentPoint = new int[2];
                currentPoint[0] = leftChildX;
                currentPoint[1] = leftChildY;

                int[] targetPoint = new int[2];
                targetPoint[0] = rightChildX;
                targetPoint[1] = rightChildY;

                int[] currentVelocity = new int[2];
                currentVelocity[0] = 0;
                currentVelocity[1] = 0;

                while (currentPoint[0] != targetPoint[0] || currentPoint[1] != targetPoint[1])
                {
                    int[] againstVelocity = new int[2];
                    againstVelocity[0] = -currentVelocity[0];
                    againstVelocity[1] = -currentVelocity[1];
                    if (currentPoint[0] > targetPoint[0]) currentVelocity[0] = -1;
                    if (currentPoint[0] < targetPoint[0]) currentVelocity[0] = 1;
                    if (currentPoint[1] > targetPoint[1]) currentVelocity[1] = -1;
                    if (currentPoint[1] < targetPoint[1]) currentVelocity[1] = 1;
                    if (currentPoint[0] == targetPoint[0]) currentVelocity[0] = 0;
                    if (currentPoint[1] == targetPoint[1]) currentVelocity[1] = 0;
                    while (currentVelocity == againstVelocity || currentPoint[0] + currentVelocity[0] > node.getSizeX() ||
                                currentPoint[0] + currentVelocity[0] < 0 ||
                                currentPoint[1] + currentVelocity[1] > node.getSizeY() ||
                                currentPoint[1] + currentVelocity[1] < 0)
                    {
                        currentVelocity = room.getRandomDirection();
                    }
                    currentPoint[0] += currentVelocity[0];
                    currentPoint[1] += currentVelocity[1];

                    node.setGridMapAtPoint(currentPoint[0], currentPoint[1], 1);
                }
            }
        }
    }
}