using Rogue.Data;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Rogue.Map
{

    public class Spawner : MonoBehaviour
    {
        [SerializeField] GameObject spacePartition;

        private delegate GameObject Delegate();
        private Delegate getSpawnPrefab;
        private ArrayList worldRestrictSpawnPoint;
        public void spawnWorldObject(Floor currentFloor, SpaceNode root)
        {
            int[,] roomSpace = root.getGridMap();
            int[] boundaryBox = root.getBoundaryBox();
            ArrayList possibleSpawnPoint = new ArrayList();
            for (int k = 0; k < roomSpace.GetLength(0); k++)
            {
                for (int l = 0; l < roomSpace.GetLength(1); l++)
                {
                    if (roomSpace[k, l] != 1) continue;
                    Dictionary<string, int> tiles = root.getSurroundingTiles(k, l, root);
                    if ((tiles["r"] == 0 || tiles["r"] == 2) &&
                    (tiles["l"] == 0 || tiles["l"] == 2) &&
                    (tiles["t"] == 0 || tiles["t"] == 2) &&
                    (tiles["b"] == 0 || tiles["b"] == 2)
                    ) continue;

                    possibleSpawnPoint.Add(new int[] { k, l });
                }
            }

            ArrayList restrictSpawnPoint = (ArrayList)possibleSpawnPoint.Clone();
            worldRestrictSpawnPoint = (ArrayList)possibleSpawnPoint.Clone();
            //spawn WorldLevelItem
            getSpawnPrefab = currentFloor.getWorldSpawnListByOrder;
            spawnDetail(root, possibleSpawnPoint, restrictSpawnPoint, getSpawnPrefab,currentFloor.getWorldSpawnList().Length, root);

            //spawn Player
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                getSpawnPrefab = currentFloor.GetPlayer;
                spawnDetail(root, possibleSpawnPoint, restrictSpawnPoint, getSpawnPrefab, currentFloor.getWorldSpawnList().Length, root);
            }
            else
            {
                int randIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, possibleSpawnPoint.Count));
                int[] point = (int[])possibleSpawnPoint[randIndex];
                int i = point[0];
                int j = point[1];

                possibleSpawnPoint.Remove(point);
                GameObject.FindGameObjectWithTag("Player").transform.position=new Vector3(i + boundaryBox[2], j + boundaryBox[3], 0);
            }

            //update Map
            foreach(int[] x in possibleSpawnPoint)
            {
                worldRestrictSpawnPoint.Remove(x);
            }

        }

        public void spawnRoomAndWallSprite(SpaceNode root)
        {
            //เริ่มจาก spawn space sprite ก่อน
            //ต้อง map array กับ boundary ด้วย

            SpaceNode[] allNode = root.getAllChildsFormCurrentNode();

            foreach (SpaceNode node in allNode)
            {
                    if (!node.isLeafNode()) continue;
                    int[,] roomSpace = node.getGridMap();
                    int[] boundaryBox = node.getBoundaryBox();
                    Biome nodeBiome = node.getBiome();

                    GameObject[] spaceTiles = nodeBiome.getSpaceTiles();                                      
                    GameObject[] pebbleTiles = nodeBiome.getPebbleTiles();
                    GameObject[] wallTiles = nodeBiome.getWallTiles();
                    GameObject[] topLeftTiles= nodeBiome.getTopLeftTiles();
                    GameObject[] topTiles = nodeBiome.getTopTiles();
                    GameObject[] topRightTiles = nodeBiome.getTopRightTiles();
                    GameObject[] bottomLeftTiles= nodeBiome.getBottomLeftTiles();
                    GameObject[] bottomTiles = nodeBiome.getBottomTiles();
                    GameObject[] bottomRightTiles = nodeBiome.getBottomRightTiles();
                    GameObject[] leftTiles = nodeBiome.getLeftTiles();
                    GameObject[] rightTiles = nodeBiome.getRightTiles();
                    GameObject[] bottomLeftProtrudeTiles = nodeBiome.getBottomLeftProtrudeTiles();
                    GameObject[] bottomRightProtrudeTiles = nodeBiome.getBottomRightProtrudeTiles();
                    GameObject[] topLeftProtrudeTiles = nodeBiome.getTopLeftProtrudeTiles();
                    GameObject[] topRightProtrudeTiles= nodeBiome.getTopRightProtrudeTiles();



                    int dungeonPositionX;
                    int dungeonPositionY;

                    for (int i = 0; i < roomSpace.GetLength(0); i++)
                    {
                        for (int j = 0; j < roomSpace.GetLength(1); j++)
                        {
                            dungeonPositionX = i + boundaryBox[2];
                            dungeonPositionY = j + boundaryBox[3];

                            if (roomSpace[i, j] == 1)
                            {
                                spawnTiles(spaceTiles, dungeonPositionX, dungeonPositionY, node);
                            }
 
                            if (roomSpace[i, j] == 2)
                            {
                                spawnTiles(pebbleTiles, dungeonPositionX, dungeonPositionY, node);
                            }

                            if (roomSpace[i, j] == 0)
                            {
                                Dictionary<string, int> tiles = node.getSurroundingTiles(i, j, root);
                                int rightTile = tiles["r"];
                                int leftTile = tiles["l"];
                                int topTile = tiles["t"];
                                int bottomTile = tiles["b"];
                                int topRightTile = tiles["tr"];
                                int bottomRightTile = tiles["br"];
                                int topLeftTile = tiles["tl"];
                                int bottomLeftTile = tiles["bl"];

                            //bottomLeftProtrudeTiles
                            if (

                                    rightTile != 0 && topTile != 0 && topRightTile == 0 ||
                                    topRightTile != 0 && rightTile != 0 && topTile != 0 && leftTile == 0 && bottomTile == 0)

                                    { spawnTiles(bottomLeftProtrudeTiles, dungeonPositionX, dungeonPositionY, node); }
                                //bottomRightProtrudeTiles
                                else if (
                                    leftTile != 0 && topTile != 0 && topLeftTile == 0 ||
                                    topLeftTile != 0 && leftTile != 0 && topTile != 0 && bottomTile == 0 && rightTile == 0
                                    ) { spawnTiles(bottomRightProtrudeTiles, dungeonPositionX, dungeonPositionY, node); }
                                //topLeftProtrudeTiles
                                else if ( 
                                    rightTile != 0 && bottomTile != 0 && bottomRightTile == 0 ||
                                    bottomRightTile != 0 && rightTile != 0 && bottomTile != 0 && leftTile == 0 && topTile == 0
                                    ) { spawnTiles(topLeftProtrudeTiles, dungeonPositionX, dungeonPositionY, node); }


                                //topRightProtrudeTiles
                                else if (

                                    bottomTile != 0 && leftTile != 0 && bottomLeftTile == 0 ||
                                    bottomLeftTile != 0 && bottomTile != 0 && leftTile != 0 && topTile == 0 && rightTile == 0

                                    ) { spawnTiles(topRightProtrudeTiles, dungeonPositionX, dungeonPositionY, node); }
                                //topLeftTiles
                                else if (

                                    rightTile == 0 && bottomTile == 0 && bottomRightTile != 0

                                        )
                                {
                                    spawnTiles(topLeftTiles, dungeonPositionX, dungeonPositionY, node);
                                }

                                //topRightTiles
                                else if (
                                    leftTile == 0 && bottomTile == 0 && bottomLeftTile != 0
                                    ) { spawnTiles(topRightTiles, dungeonPositionX, dungeonPositionY, node); }
                                //bottomLeftTiles
                                else if (
                                    rightTile == 0 && topTile == 0 && topRightTile != 0
                                    ) { spawnTiles(bottomLeftTiles, dungeonPositionX, dungeonPositionY, node); }

                                //bottomRightTiles
                                else if (
                                    leftTile == 0 && topTile == 0 && topLeftTile != 0
                                    ) { spawnTiles(bottomRightTiles, dungeonPositionX, dungeonPositionY, node); }
                            //wallTiles
                            else if (
                                rightTile == 0 && leftTile == 0 && topTile == 0 && bottomTile == 0
                                )
                            {
                                spawnTiles(wallTiles, dungeonPositionX, dungeonPositionY, node);
                            }

                            //topTiles
                            else if (
                                    bottomTile != 0
                                    ) { spawnTiles(topTiles, dungeonPositionX, dungeonPositionY, node); }
                                //bottomTiles
                                else if (
                                    topTile != 0
                                    ) { spawnTiles(bottomTiles, dungeonPositionX, dungeonPositionY, node); }
                                //leftTiles
                                else if (
                                    rightTile != 0
                                    ) { spawnTiles(leftTiles, dungeonPositionX, dungeonPositionY, node); }
                                //rightTiles
                                else if (

                                    leftTile!=0
                                    ) { spawnTiles(rightTiles, dungeonPositionX, dungeonPositionY, node); }

                            }
                        }
                    }

                    
                

                
            }



        }

        public void SpawnRoomDetails(SpaceNode root,Dungeon dungeon)
        {

            foreach(SpaceNode node in root.getAllChildsFormCurrentNode())
            {
                if (!node.isLeafNode()) continue;
                Room room = node.getRoom();
                Biome biome = node.getBiome();

                //อันดับแรกหา possible spawn point ก่อน
                int[,] roomSpace = node.getGridMap();
                int[] boundaryBox = node.getBoundaryBox();
                ArrayList possibleSpawnPoint = new ArrayList();

                int dummyX=-1;
                int dummyY=-1;

                for (int k = 0; k < roomSpace.GetLength(0); k++)
                {
                    for (int l = 0; l < roomSpace.GetLength(1); l++)
                    {
                        if (roomSpace[k, l] != 1) continue;
                        Dictionary<string, int> tiles = node.getSurroundingTiles(k, l, root);
                        if ((tiles["r"] == 0||tiles["r"] == 2)&&
                        (tiles["l"] == 0 || tiles["l"] == 2) &&
                        (tiles["t"] == 0 || tiles["t"] == 2) &&
                        (tiles["b"] == 0 || tiles["b"] == 2)
                        ) continue;

                        //แต่ index มันคนละระดับนี่ดิ ต้องแปลงก่อน 
                        //Debug.Log(node.getWorldCoordinateX(k)+"-"+ node.getWorldCoordinateY(l));
                        

                        possibleSpawnPoint.Add(new int[] { k, l });

                        /*
                        if (worldRestrictSpawnPoint.Contains(new int[] { node.getWorldCoordinateX(k), node.getWorldCoordinateY(l) }))
                        {
                            
                            continue; }*/


                        //possibleSpawnPoint.Add(new int[] { k, l });
                    }
                }

                ArrayList temporaryStorage = new ArrayList();

                foreach(int[] x in possibleSpawnPoint)
                {
                    foreach (int[] y in worldRestrictSpawnPoint)
                    {
                        if(node.getWorldCoordinateX(x[0])==y[0] && node.getWorldCoordinateY(x[1]) == y[1])
                        {
                            temporaryStorage.Add(x);
                        }
                    }
                }

                foreach(int[] x in temporaryStorage)
                {
                    possibleSpawnPoint.Remove(x);
                }
                /*
                foreach(int[] x in possibleSpawnPoint)
                {
                    Debug.Log(node.getWorldCoordinateX(x[0]) + "+++" + node.getWorldCoordinateY(x[1]));
                }*/

                ArrayList restrictSpawnPoint = (ArrayList)possibleSpawnPoint.Clone();

                //spawn Producer
                getSpawnPrefab = biome.GetProducer; 
                spawnDetail(node, possibleSpawnPoint, restrictSpawnPoint,getSpawnPrefab, room.getNumberOfProducer(), root);
                
                //spawn PrimaryConsumer
                getSpawnPrefab = biome.GetPrimaryConsumer;
                spawnDetail(node, possibleSpawnPoint, restrictSpawnPoint, getSpawnPrefab, room.getNumberOfPrimaryConsumer(), root);

                //spawn SecondaryConsumer
                getSpawnPrefab = biome.GetSecondaryConsumer;
                spawnDetail(node, possibleSpawnPoint, restrictSpawnPoint, getSpawnPrefab, room.getNumberOfSecondaryConsumer(), root);
                
                //spawn Trap
                getSpawnPrefab = biome.GetTrap;
                spawnDetail(node, possibleSpawnPoint, restrictSpawnPoint, getSpawnPrefab, room.getNumberOfTrap(), root);
                
               /*//spawn Puzzle
                getSpawnPrefab = biome.GetPuzzle;
                spawnDetail(node, possibleSpawnPoint, restrictSpawnPoint, getSpawnPrefab, room.getNumberOfPuzzle(), root);*/
                
                //spawn resource
                getSpawnPrefab = biome.GetResource;
                spawnDetail(node, possibleSpawnPoint, restrictSpawnPoint, getSpawnPrefab, room.getNumberOfResource(), root);


            }

        }

        private void spawnDetail(SpaceNode node, ArrayList possibleSpawnPoint, ArrayList restrictSpawnPoint, Delegate getSpawnPrefab,int spawnNumber,SpaceNode root)
        {
            int[] boundaryBox = node.getBoundaryBox();
            GameObject toSpawnPrefab;

            for (int num = 0; num < spawnNumber; num++)
            {
                toSpawnPrefab = getSpawnPrefab();
                while (possibleSpawnPoint.Count != 0) // หาวิธี check แบบอื่น remove มันไม่ null
                {
                    SpaceRestriction spawnRestriction = toSpawnPrefab.GetComponent<SpaceRestriction>();
                    if (spawnRestriction.IsNotRestricted())
                    {
                        int randIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, possibleSpawnPoint.Count));
                        int[] point = (int[])possibleSpawnPoint[randIndex];
                        int i = point[0];
                        int j = point[1];

                        possibleSpawnPoint.Remove(point);


                        //RestrictSpawnPointRemove
                        ArrayList temporaryStorage = new ArrayList();

                        foreach (int[] x in restrictSpawnPoint)
                        {
                            if (x[0] == i && x[1] == j)
                            {
                                temporaryStorage.Add(x);
                            }
                        }

                        foreach(int[] x in temporaryStorage)
                        {
                            restrictSpawnPoint.Remove(x);
                        }

                        SpawnObject(toSpawnPrefab, new Vector3(i + boundaryBox[2], j + boundaryBox[3], 0));
                        break;
                    }
                    else
                    {
                        if (restrictSpawnPoint.Count == 0) break;
                        int randIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, restrictSpawnPoint.Count));
                        int[] point = (int[])restrictSpawnPoint[randIndex];
                        restrictSpawnPoint.Remove(point);

                        Dictionary<string, int> surroundTiles = node.getSurroundingTiles(point[0], point[1], root);
                        int totalNumberOfWall = 0;
                        int numberOfLongestAdjacentWall = 0;
                        int dummyNumberOfLongestAdjacentWall = 0;

                            foreach (string s in new string[] { "tr", "r", "br", "b", "bl", "l", "tl", "t" })
                        {
                            if(surroundTiles[s] == 3)
                            {
                                totalNumberOfWall += 1;
                            }
                            if (surroundTiles[s] == 0)
                            {
                                totalNumberOfWall += 1;
                                dummyNumberOfLongestAdjacentWall += 1;
                            }
                            else
                            {
                                numberOfLongestAdjacentWall = Mathf.Max(numberOfLongestAdjacentWall, dummyNumberOfLongestAdjacentWall);
                                dummyNumberOfLongestAdjacentWall = 0;
                            }
                        }

                        foreach (string s in new string[] { "tr", "r", "br", "b", "bl", "l", "tl", "t" })
                        {
                            if (surroundTiles[s] == 0)
                            {
                                dummyNumberOfLongestAdjacentWall += 1;
                            }
                            else
                            {
                                numberOfLongestAdjacentWall = Mathf.Max(numberOfLongestAdjacentWall, dummyNumberOfLongestAdjacentWall);
                                dummyNumberOfLongestAdjacentWall = 0;
                            }
                        }

                            if (numberOfLongestAdjacentWall >= totalNumberOfWall)
                        {
                            node.setGridMapAtPoint(point[0], point[1], 3);
                            SpawnObject(toSpawnPrefab, new Vector3(point[0] + boundaryBox[2], point[1] + boundaryBox[3], 0));

                            //PossibleSpawnPointRemove
                            ArrayList temporaryStorage2 = new ArrayList();
                            foreach (int[] x in possibleSpawnPoint)
                            {
                                if (x[0] == point[0] && x[1] == point[1])
                                {
                                    temporaryStorage2.Add(x);
                                }
                            }

                            foreach (int[] x in temporaryStorage2)
                            {
                                possibleSpawnPoint.Remove(x);
                            }

                            break;
                        }


                    }
                }

            }
        }

        private void spawnTiles(GameObject[] tiles, int i, int j,SpaceNode node)
        {
            GameObject selectedTile = tiles[Mathf.RoundToInt(UnityEngine.Random.Range(0, tiles.Length))];
            GameObject spawnedSprite = SpawnObject(selectedTile, new Vector3(i, j, 0));
            if (node.getGizmozObject() != null)
            spawnedSprite.transform.parent =node.getGizmozObject().transform;
        }

        public void SpawnSpacePartition(SpaceNode root)
        {
            SpaceNode[] allNode = root.getAllChildsFormCurrentNode();
            foreach (SpaceNode node in allNode)
            {           
                GameObject gizmoz=Instantiate(spacePartition,
                    new Vector3(node.getCenterX(),node.getCenterY(),0),
                    Quaternion.identity
                    )as GameObject;

                gizmoz.GetComponent<VisulizedSpacePartition>().SetSpaceNode(node);

                node.setGizmozObject(gizmoz);

                if (node.getParent() != null)
                {
                    node.getGizmozObject().transform.parent =
                    node.getParent().getGizmozObject().transform;
                }
            }

        }

        public void SpawnBedrock(SpaceNode root)
        {
            GameObject[] bedrockTiles = root.getLeft().getBiome().getBedrockTiles();
            //ขอบซ้าย+ขวา
            for (int i = 0; i <= root.getSizeY()+1; i++)
            {
                int dungeonPositionY = i + root.getBoundaryBox()[3]-1;
                GameObject selectedSpaceTile = bedrockTiles[Mathf.RoundToInt(UnityEngine.Random.Range(0, bedrockTiles.Length))];
                SpawnObject(selectedSpaceTile, new Vector3(root.getBoundaryBox()[0]+1, dungeonPositionY, 0));//ขอบขวา
                SpawnObject(selectedSpaceTile, new Vector3(root.getBoundaryBox()[2]-1, dungeonPositionY, 0));//ขอบซ้าย
            }
            
            //ขอบบน+ล่าง
            for (int i = 0; i <= root.getSizeX()+1; i++)
            {
                int dungeonPositionX = i + root.getBoundaryBox()[2]-1;
                GameObject selectedSpaceTile = bedrockTiles[Mathf.RoundToInt(UnityEngine.Random.Range(0, bedrockTiles.Length))];
                SpawnObject(selectedSpaceTile, new Vector3(dungeonPositionX, root.getBoundaryBox()[1]+1, 0));//ขอบบน
                SpawnObject(selectedSpaceTile, new Vector3(dungeonPositionX, root.getBoundaryBox()[3]-1, 0));//ขอบล่าง
            }
        }

        private GameObject SpawnObject(GameObject objectToSpawn,Vector3 position)
        {
            GameObject x=Instantiate(objectToSpawn, position,Quaternion.identity) as GameObject;
            return x;
        }

       
    }
}
