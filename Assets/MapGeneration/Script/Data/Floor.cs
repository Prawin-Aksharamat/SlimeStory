using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Data
{
    [CreateAssetMenu(menuName = "Floor")]
    public class Floor : ScriptableObject
    {
 
        private int roomLeft;
        private int roomNumber;

        public void setRoomNumber(int roomNumber) {
            this.roomNumber = roomNumber;
            this.roomLeft = roomNumber;
        }


        private static int currentWorldSpawnListIndex=0;
        [SerializeField] GameObject[] worldSpawnList;
        public GameObject[] getWorldSpawnList() => worldSpawnList;

        [SerializeField] RoomTemplate[] specialRoomList;
        private static int currentSpecialRoomListIndex = 0;

        public GameObject getWorldSpawnListByOrder() {
            if (currentWorldSpawnListIndex >= worldSpawnList.Length)
            {
                currentWorldSpawnListIndex = 0;
            }
            return worldSpawnList[currentWorldSpawnListIndex++];
        }

        [SerializeField] GameObject player;

        public GameObject GetPlayer() => player;

        [SerializeField] RoomTemplate[] roomTemplates;

        [SerializeField] int tilesPerProducer; // กี่ tiles สำหรับ producer 1 ตัว
        [SerializeField] int producerDensity = 1;

        public int getTilesPerProducer() => tilesPerProducer;
        public float getProducerDensity() => producerDensity;

        [SerializeField] float primaryConsumerPerProducer;
        [SerializeField] float primaryConsumerDensity = 1;

        public float getPrimaryConsumerPerProducer() => primaryConsumerPerProducer;
        public float getPrimaryConsumerDensity() => primaryConsumerDensity;

        [SerializeField] float secondaryConsumerPerPrimary;
        [SerializeField] float secondaryConsumerDensity = 1;

        public float getSecondaryConsumerPerPrimary() => secondaryConsumerPerPrimary;
        public float getSecondaryConsumerDensity() => secondaryConsumerDensity;

        [SerializeField] int tilesPerResource;
        [SerializeField] float resourceDensity = 1;

        public int getTilesPerResource() => tilesPerResource;
        public float getResourceDensity() => resourceDensity;

        [SerializeField] int tilesPerTrap;
        [SerializeField] float trapDensity = 1;

        public int getTilesPerTrap() => tilesPerTrap;
        public float getTrapDensity() => trapDensity;

        [SerializeField] int tilesPerPuzzle;
        [SerializeField] float puzzleDensity = 1;

        public int getTilesPerPuzzle() => tilesPerPuzzle;
        public float getPuzzleDensity() => puzzleDensity;


        [SerializeField] int[] boundaryBox = new int[4];
        /*
         0: x บน
         1: y บน
         2: x ล่าง
         3: y ล่าง
             */
        [SerializeField] int capX;
        [SerializeField] int capY;

        [SerializeField] Biome[] biomes;

        public Biome[] getBiomes() => biomes;

        public int[] getBoundaryBox() => boundaryBox;
        public NodeSizeCap getPartitionSizeCap() => new NodeSizeCap(capX, capY);

        //For Automated Test
        public void setBoundartBox(int xTop, int yTop, int xBottom, int yBottom)
        {
            boundaryBox[0] = xTop;
            boundaryBox[1] = yTop;
            boundaryBox[2] = xBottom;
            boundaryBox[3] = yBottom;
        }

        public void setPartitionSizeCap(int capX, int capY)
        {
            this.capX = capX;
            this.capY = capY;
        }

        public Room getRandomRoom()
        {
            if (currentSpecialRoomListIndex >= specialRoomList.Length
                || (UnityEngine.Random.value > (1f/roomNumber) &&
                ((specialRoomList.Length-currentSpecialRoomListIndex+1) < roomLeft))
                )
            {
                roomLeft -= 1;
                return new Room(roomTemplates[Mathf.RoundToInt(UnityEngine.Random.Range(0, roomTemplates.Length))]);
            }
            roomLeft -= 1;
            return new Room(specialRoomList[currentSpecialRoomListIndex++]);
        }


    }
}
