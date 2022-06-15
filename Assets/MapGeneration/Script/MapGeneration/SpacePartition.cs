using UnityEngine;
using Rogue.Data;

namespace Rogue.Map
{
    public class SpacePartition
    {
        private Floor floor;
        private GameObject spaceSprite;

        private SpaceNode root;
        private NodeSizeCap nodeSizeCap;
        int[] boundaryBox;
        private int dungeonLevel;

        public void StartSpacePartitionFor(Floor floor)
        {
            bool stopXPartition = false;
            bool stopYPartition = false;

            this.floor = floor;

            boundaryBox = floor.getBoundaryBox();
            nodeSizeCap = floor.getPartitionSizeCap();
            
            root = new SpaceNode(boundaryBox);
            Partition(root, stopXPartition, stopYPartition);
        }

        private void Partition(SpaceNode node, bool stopXPartition, bool stopYPartition)
        {
            if (stopYPartition && stopXPartition) return;

            bool isHorizontalPartition = getRandomBoolean();
            if (stopXPartition) isHorizontalPartition = true;
            if (stopYPartition) isHorizontalPartition = false;

            int[] leftBound, rightBound;
            setUpLRBound(node, out leftBound, out rightBound);

            if (isHorizontalPartition)
            {
                partitionByAxisX(false, node, leftBound, rightBound, stopXPartition, stopYPartition);
            }

            if (!isHorizontalPartition)
            {
                partitionByAxisX(true, node, leftBound, rightBound, stopXPartition, stopYPartition);
            }
        }

        public bool getRandomBoolean()
        {
            return UnityEngine.Random.value > 0.5f;
        }

        public void setUpLRBound(SpaceNode node, out int[] leftBound, out int[] rightBound)
        {
            leftBound = new int[4];
            rightBound = new int[4];
            int[] boundary = node.getBoundaryBox();
            for (int i = 0; i < boundary.Length; i++)
            {
                leftBound[i] = boundary[i];
                rightBound[i] = boundary[i];
            }
        }

        public void partitionByAxisX(bool isXAxis,SpaceNode node, int[] leftBound, int[] rightBound, bool stopXPartition, bool stopYPartition)
        {
            int lowerBBoxCoordinateIndex;
            int higherBBoxCoordinateIndex;

            if (isXAxis)
            {
                lowerBBoxCoordinateIndex = 2;
                higherBBoxCoordinateIndex = 0;
            }
            else
            {
                lowerBBoxCoordinateIndex = 3;
                higherBBoxCoordinateIndex = 1;
            }

            int lowerCoordinate = node.getBoundaryBox()[lowerBBoxCoordinateIndex];
            int higherCoordinate = node.getBoundaryBox()[higherBBoxCoordinateIndex];

            //partitionCoor เป็นของ Higher Coor เลยต้อง +1
            int partitionCoordinate = Mathf.RoundToInt(UnityEngine.Random.Range(lowerCoordinate, higherCoordinate));

            if (partitionCoordinate - lowerCoordinate < nodeSizeCap.minX)
            {
                partitionCoordinate += nodeSizeCap.minX - (partitionCoordinate - lowerCoordinate);
            }

            else if (higherCoordinate - partitionCoordinate +1 < nodeSizeCap.minX)
            {
                partitionCoordinate -= nodeSizeCap.minX - (higherCoordinate - partitionCoordinate+1);
            }

            if (partitionCoordinate - lowerCoordinate < nodeSizeCap.minX || higherCoordinate - partitionCoordinate +1< nodeSizeCap.minX)
            {
                if (isXAxis)
                {
                    stopXPartition = true;
                    if (!stopYPartition) partitionByAxisX(false,node, leftBound, rightBound, stopXPartition, stopYPartition);
                }
                else {
                    stopYPartition = true;
                    if (!stopXPartition) partitionByAxisX(true,node, leftBound, rightBound, stopXPartition, stopYPartition);
                }
               
            }

            else
            {
                leftBound[higherBBoxCoordinateIndex] = partitionCoordinate-1;
                rightBound[lowerBBoxCoordinateIndex] = partitionCoordinate;
                //left = ซ้าย+ล่าง right=ขวา+บน

                node.setLeft(new SpaceNode(leftBound));
                Partition(node.getLeft(), stopXPartition, stopYPartition);

                node.setRight(new SpaceNode(rightBound));
                Partition(node.getRight(), stopXPartition, stopYPartition);

                
            }
        }

        public SpaceNode getRoot() => root;

    }
}
