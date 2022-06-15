using Rogue.Data;

namespace Rogue.Map
{
    public class RoomSpaceGeneration
    {
        public void StartRoomSpaceGeneration(SpaceNode root,Floor currentFloor)
        {
            SpaceNode[] allNode = root.getAllChildsFormCurrentNode();

            foreach (SpaceNode node in allNode)
            {
                if(node.isLeafNode())
                {
                    GenerateRoomSpaceFor(node, currentFloor);
                    FillRoomSpaceFor(node);
                }
            }
        }

        private void GenerateRoomSpaceFor(SpaceNode node, Floor currentFloor)
        {
            Room roomTemplate = currentFloor.getRandomRoom();
            node.setRoom(roomTemplate);

            int[,] roomSpace = new int[node.getSizeX(), node.getSizeY()];
            int[] startPoint = node.getRandomCoordinate();
            int numberOfTurn = roomTemplate.getRandomTurnNumber();
            
            int[] currentPoint = new int[2];
            currentPoint[0] = startPoint[0];
            currentPoint[1] = startPoint[1];

            int[] currentVelocity = roomTemplate.getRandomDirection();
            int velocityChangeProbability = 0;

            int nodeSizeX = node.getSizeX();
            int nodeSizeY = node.getSizeY();

            int probabilityIncrement = roomTemplate.getProbabilityIncrement();

            while (numberOfTurn > 0)
            {
                int[] againstVelocity = new int[2];
                againstVelocity[0] = -currentVelocity[0];
                againstVelocity[1] = -currentVelocity[1];

                roomSpace[currentPoint[0], currentPoint[1]] = 1;

                if (UnityEngine.Random.Range(1, 100) <= velocityChangeProbability)
                {
                    velocityChangeProbability = 0;
                    currentVelocity = roomTemplate.getRandomDirection();
                    while (currentVelocity[0] == againstVelocity[0]&& currentVelocity[1] == againstVelocity[1])
                    {
                        currentVelocity = roomTemplate.getRandomDirection();
                    }
                    numberOfTurn--;
                }
                else
                {
                    velocityChangeProbability += probabilityIncrement;
                }

                //ดักมันเดินเกินนั่นละ
                if (currentVelocity == againstVelocity || currentPoint[0] + currentVelocity[0] >= nodeSizeX ||
                            currentPoint[0] + currentVelocity[0] < 0 ||
                            currentPoint[1] + currentVelocity[1] >= nodeSizeY ||
                            currentPoint[1] + currentVelocity[1] < 0)
                {
                    while (currentVelocity == againstVelocity || currentPoint[0] + currentVelocity[0] >= nodeSizeX ||
                            currentPoint[0] + currentVelocity[0] < 0 ||
                            currentPoint[1] + currentVelocity[1] >= nodeSizeY ||
                            currentPoint[1] + currentVelocity[1] < 0)
                    {
                        velocityChangeProbability = 0;
                        int[] possibleX;
                        int[] possibleY;
                        if(currentPoint[0] + 1 >= nodeSizeX && currentPoint[0] -1 < 0)
                        {
                            possibleX = new int[] { 0 };
                        }
                        else if (currentPoint[0] + 1 >= nodeSizeX)
                        {
                            possibleX = new int[] { 0, -1 };
                        }
                        else if(currentPoint[0] -1 < 0)
                        {
                            possibleX = new int[] {0, 1};
                        }
                        else
                        {
                            possibleX = new int[] { 0, 1,-1 };
                        }

                        if (currentPoint[1] + 1 >= nodeSizeY && currentPoint[1] -1 < 0)
                        {
                            possibleY = new int[] { 0 };
                        }
                        else if (currentPoint[1] + 1 >= nodeSizeY)
                        {
                            possibleY = new int[] { 0, -1 };
                        }
                        else if (currentPoint[1] -1 < 0)
                        {
                            possibleY = new int[] { 0, 1 };
                        }
                        else
                        {
                            possibleY = new int[] { 0, 1, -1 };
                        }

                        foreach(int m in possibleX)
                        {
                            foreach(int n in possibleY)
                            {
                                if((m != againstVelocity[0] || n != againstVelocity[1]) && (m != 0 || n != 0))
                                {
                                    currentVelocity = new int[] { m, n };
                                }
                            }
                        }
                    }
                    numberOfTurn--;
                }
                currentPoint[0] += currentVelocity[0];
                currentPoint[1] += currentVelocity[1];
            }
            roomSpace[currentPoint[0], currentPoint[1]] = 1;

            //จาก current point ที่เป็น point สุดท้ายต้องมาเชื่อมกับ start point
            while (currentPoint[0] != startPoint[0] || currentPoint[1] != startPoint[1])
            {
                int[] againstVelocity = new int[2];
                againstVelocity[0] = -currentVelocity[0];
                againstVelocity[1] = -currentVelocity[1];

                if (currentPoint[0] > startPoint[0]) currentVelocity[0] = -1;
                if (currentPoint[0] < startPoint[0]) currentVelocity[0] = 1;
                if (currentPoint[1] > startPoint[1]) currentVelocity[1] = -1;
                if (currentPoint[1] < startPoint[1]) currentVelocity[1] = 1;
                if (currentPoint[0] == startPoint[0]) currentVelocity[0] = 0;
                if (currentPoint[1] == startPoint[1]) currentVelocity[1] = 0;

                    while (currentVelocity == againstVelocity || currentPoint[0] + currentVelocity[0] >= nodeSizeX ||
                            currentPoint[0] + currentVelocity[0] < 0 ||
                            currentPoint[1] + currentVelocity[1] >= nodeSizeY ||
                            currentPoint[1] + currentVelocity[1] < 0)
                    {
                    int[] possibleX;
                    int[] possibleY;
                    if (currentPoint[0] + 1 >= nodeSizeX && currentPoint[0] - 1 < 0)
                    {
                        possibleX = new int[] { 0 };
                    }
                    else if (currentPoint[0] + 1 >= nodeSizeX)
                    {
                        possibleX = new int[] { 0, -1 };
                    }
                    else if (currentPoint[0] - 1 < 0)
                    {
                        possibleX = new int[] { 0, 1 };
                    }
                    else
                    {
                        possibleX = new int[] { 0, 1, -1 };
                    }

                    if (currentPoint[1] + 1 >= nodeSizeY && currentPoint[1] - 1 < 0)
                    {
                        possibleY = new int[] { 0 };
                    }
                    else if (currentPoint[1] + 1 >= nodeSizeY)
                    {
                        possibleY = new int[] { 0, -1 };
                    }
                    else if (currentPoint[1] - 1 < 0)
                    {
                        possibleY = new int[] { 0, 1 };
                    }
                    else
                    {
                        possibleY = new int[] { 0, 1, -1 };
                    }

                    foreach (int m in possibleX)
                    {
                        foreach (int n in possibleY)
                        {
                            if ((m != againstVelocity[0] || n != againstVelocity[1]) && (m != 0 || n != 0))
                            {
                                currentVelocity = new int[] { m, n };
                            }
                        }
                    }

                }
            
                currentPoint[0] += currentVelocity[0];
                currentPoint[1] += currentVelocity[1];
                roomSpace[currentPoint[0], currentPoint[1]] = 1;
            }
            node.setGridMap(roomSpace);
        }
        
        private void FillRoomSpaceFor(SpaceNode i)
        {
            //เราจะ scan line บน-ล่าง-ซ้าย-ขวา
            int[,] lineCount = new int[i.getGridMap().GetLength(0), i.getGridMap().GetLength(1)];
            int[,] roomSpace = i.getGridMap();
            //scan จากซ้าย + setup line count sก่อน
            for(int j = 0; j < lineCount.GetLength(0); j++)
            {
                int point = 0;
                for (int k = 0; k < lineCount.GetLength(1); k++)
                {
                    if (roomSpace[j, k] == 1) point = 1;
                    lineCount[j, k] = point;
                }
            }

            //scan จากขวา
            for (int j = lineCount.GetLength(0)-1; j >=0; j--)
            {
                int point = 0;
                for (int k = lineCount.GetLength(1)-1; k >=0; k--)
                {
                    if (roomSpace[j, k] == 1) point = 1;
                    lineCount[j, k] += point;
                }
            }

            //scan จากบน
            for (int j = 0; j < lineCount.GetLength(1); j++)
            {
                int point = 0;
                for (int k = 0; k < lineCount.GetLength(0); k++)
                {
                    if (roomSpace[k, j] == 1) point = 1;
                    lineCount[k, j] += point;
                }
            }

            //scan จากล่าง
            for (int j = lineCount.GetLength(1)-1; j >=0; j--)
            {
                int point = 0;
                for (int k = lineCount.GetLength(0)-1; k >=0; k--)
                {
                    if (roomSpace[k, j] == 1) point = 1;
                    lineCount[k, j] += point;
                }
            }

            //fill roomspace          
           for (int j = 0; j < lineCount.GetLength(0); j++)
                {
                    for (int k = 0; k < lineCount.GetLength(1); k++)
                    {
                        if (lineCount[j, k] == 4)
                        {
                            roomSpace[j, k] = 1;
                        }
                    }
                }
             i.setGridMap(roomSpace);
        }     
    }
}
