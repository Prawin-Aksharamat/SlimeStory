using UnityEngine;
using Rogue.Data;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Rogue.Map
{

    public class MapGeneration : MonoBehaviour
    {
        [SerializeField] private Dungeon dungeon;


        private SpacePartition spacePartition;
        private BiomeSetup biomeSetup;
        private RoomSpaceGeneration roomSpaceGenerator;
        private Spawner spawner;
        private PassageSpaceGeneration passageSpaceGeneration;
        private DungeonManager dungeonManager;
        private GameObject gameManager;

        private void Awake()
        {
            spacePartition = new SpacePartition();
            biomeSetup = new BiomeSetup();
            roomSpaceGenerator = new RoomSpaceGeneration();
            spawner = GetComponent<Spawner>();
            passageSpaceGeneration = new PassageSpaceGeneration();
            dungeonManager= GetComponent<DungeonManager>();

            gameManager = GameObject.FindGameObjectWithTag("GameManager");
        }

        private void Start()
        {
            generateMap();
        }

        public void generateMap()
        {
            Floor currentFloor = dungeon.getFloor(dungeonManager.getCurrentFloor());
            spacePartition.StartSpacePartitionFor(currentFloor);
            biomeSetup.StartBiomeSetup(spacePartition.getRoot(), currentFloor.getBiomes());
            spawner.SpawnSpacePartition(spacePartition.getRoot());

            dungeonManager.setUp(spacePartition.getRoot(), currentFloor);
            roomSpaceGenerator.StartRoomSpaceGeneration(spacePartition.getRoot(), currentFloor);

            passageSpaceGeneration.StartPassageSpaceGeneration(spacePartition.getRoot());
            dungeonManager.setSpawnNumber(spacePartition.getRoot(), currentFloor);

            spawner.spawnRoomAndWallSprite(spacePartition.getRoot());
            spawner.SpawnBedrock(spacePartition.getRoot());

            gameManager.GetComponent<MapManager>().SetMap(spacePartition.getRoot().getGridMap(), currentFloor.getBoundaryBox());

            spawner.spawnWorldObject(currentFloor, spacePartition.getRoot());
            spawner.SpawnRoomDetails(spacePartition.getRoot(), dungeon);


        }

        public void GenerateBossMap()
        {
            int[,] bossMap = new int[,] 
            {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,2,1,1,1,2,1,1,1,2,1,1,1,2,1,1,1,0},
            {0,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,0},
            {0,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,0},
            {0,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,0},
            {0,1,1,2,1,1,1,2,2,1,1,1,2,2,1,1,1,2,1,1,0},
            {0,1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1,0},
            {0,1,1,1,1,2,1,1,1,2,1,2,1,1,1,2,1,1,1,1,0},
            {0,1,1,1,1,2,1,1,1,1,1,1,1,1,1,2,1,1,1,1,0},
            {0,1,1,1,1,2,1,1,1,2,1,2,1,1,1,2,1,1,1,1,0},
            {0,1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1,0},
            {0,1,1,2,1,1,1,2,2,1,1,1,2,2,1,1,1,2,1,1,0},
            {0,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,0},
            {0,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,0},
            {0,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,0},
            {0,1,1,1,2,1,1,1,2,1,1,1,2,1,1,1,2,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};

            int[] bossMapBoundaryBox =new int[] { 20, 20, 0, 0 };

            SpaceNode bossRoomNode = new SpaceNode(bossMapBoundaryBox);
            bossRoomNode.setGridMap(bossMap);

            Floor currentFloor = dungeon.getFloor(dungeonManager.getCurrentFloor());
            bossRoomNode.setBiome(currentFloor.getBiomes()[0]);

            spawner.spawnRoomAndWallSprite(bossRoomNode);
            gameManager.GetComponent<MapManager>().SetMap(bossMap, bossMapBoundaryBox);

            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(10, 8, 0);
            Instantiate(bossRoomNode.getBiome().GetSecondaryConsumer(), new Vector3(10, 10, 0), Quaternion.identity);

            Volume volume = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Volume>();
            volume.profile.TryGet<Vignette>(out Vignette v);
            v.intensity.value = 0.1f;

            GameObject.FindObjectOfType<AudioPlayer>().PlayBossTheme();
        }

        public int[,] getFinalMap()
        {
            return spacePartition.getRoot().getGridMap();
        }

        public int getMapSizeX()
        {
            return spacePartition.getRoot().getSizeX();
        }

        public int getMapSizeY()
        {
            return spacePartition.getRoot().getSizeY();
        }

    }        

}
