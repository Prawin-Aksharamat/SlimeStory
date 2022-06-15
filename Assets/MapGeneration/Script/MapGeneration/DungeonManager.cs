using UnityEngine;
using Rogue.Data;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Rogue.Map
{
    public class DungeonManager : MonoBehaviour
    {
        [SerializeField] int currentFloor = 0;

        private ArrayList ProducerList = new ArrayList();
        private ArrayList PrimaryConsumerList = new ArrayList();
        private ArrayList SecondaryConsumerList = new ArrayList();

        private TurnOrder turnOrder;

        public int getCurrentFloor() => currentFloor;

        private int floorSize;

        private int producerNumber;
        private int resourceNumber;
        private int puzzleNumber;
        private int trapNumber;
        private int primaryConsumerNumber;
        private int secondaryConsumerNumber;

        private float extraPrimaryWeight = 0;
        private float extraSecondaryWeight = 0;
        private float extraResourceWeight = 0;

        private float secondaryConsumerPerPrimary;
        private float currentFloorSecondaryDensity;

        private int numberOfRoom;

        private void Awake()
        {
            turnOrder = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnOrder>();
        }

        public void setUp(SpaceNode root,Floor currentFloor)
        {
            SpaceNode[] allNode = root.getAllChildsFormCurrentNode();

            foreach (SpaceNode node in allNode)
            {
                if (node.isLeafNode())
                {
                    numberOfRoom += 1;
                }
            }

            currentFloor.setRoomNumber(numberOfRoom);
        }

        public void nextLevel()
        {
            turnOrder.TriggerAllowPlayerInput(false);
            Action afterFadeAction = nextLevelAfterFade;
            StartCoroutine(FindObjectOfType<FadeBetweenScene>().FadeInToNextScene(afterFadeAction));

            /*currentFloor += 1;

            GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);

            for (int i = 0; i < GameObjects.Length; i++)
            {
                if(GameObjects[i].GetComponent<Undestroyable>()==null)
                Destroy(GameObjects[i]);
            }

            GetComponent<MapGeneration>().generateMap();*/

        }

        private void nextLevelAfterFade()
        {
            currentFloor += 1;

            GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);

            for (int i = 0; i < GameObjects.Length; i++)
            {
                if (GameObjects[i].transform.parent != null) continue;
                if (GameObjects[i].GetComponent<Undestroyable>() == null)
                    Destroy(GameObjects[i]);
            }

            turnOrder.ResetTurnOrder();

            if (currentFloor < 6)
            {
                GetComponent<MapGeneration>().generateMap();
            }
            else if(currentFloor==6)
            {
                GetComponent<MapGeneration>().GenerateBossMap();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public void setSpawnNumber(SpaceNode root, Floor currentFloor)
        {
            SpaceNode[] allNode = root.getAllChildsFormCurrentNode();

            foreach (SpaceNode node in allNode)
            {
                if (node.isLeafNode())
                {
                    int roomSize = 0;
                    int[,] gridMap = node.getGridMap();
                    foreach (int i in gridMap)
                    {
                        if (i == 1) roomSize += 1;
                    }
                    node.getRoom().setRoomSize(roomSize);
                    floorSize += roomSize;
                }
            }



            producerNumber = Mathf.RoundToInt((floorSize / currentFloor.getTilesPerProducer()) * currentFloor.getProducerDensity());
            resourceNumber = Mathf.RoundToInt((floorSize / currentFloor.getTilesPerResource()) * currentFloor.getResourceDensity());
            puzzleNumber = Mathf.RoundToInt((floorSize / currentFloor.getTilesPerPuzzle()) * currentFloor.getPuzzleDensity());
            trapNumber = Mathf.RoundToInt((floorSize / currentFloor.getTilesPerTrap()) * currentFloor.getTrapDensity());

            int realProducerNumber = 0;
            int realResourceNumber = 0;
            int realPuzzleNumber = 0;
            int realTrapNumber = 0;
            foreach (SpaceNode node in allNode)
            {

                if (node.isLeafNode())
                {
                    Room room = node.getRoom();
                    float roomWeight = (float)room.getRoomSize() / (float)floorSize;

                    int roomProducerNumber = Mathf.RoundToInt(roomWeight * producerNumber);
                    room.setNumberOfProducer(roomProducerNumber);
                    realProducerNumber += room.getNumberOfProducer();



                    float resourceRoomWeight = roomWeight;
                    if (Mathf.RoundToInt((resourceRoomWeight + extraResourceWeight) * resourceNumber) >= 1)
                    {
                        resourceRoomWeight = resourceRoomWeight + extraResourceWeight;
                        extraResourceWeight = 0;
                    }
                    else
                    {
                        extraResourceWeight += resourceRoomWeight;
                    }

                    int roomResourceNumber = Mathf.RoundToInt(resourceRoomWeight * resourceNumber);
                    room.setNumberOfResource(roomResourceNumber);
                    realResourceNumber += room.getNumberOfResource();



                    int roomPuzzleNumber = Mathf.RoundToInt(roomWeight * puzzleNumber);
                    room.setNumberOfPuzzle(roomPuzzleNumber);
                    realPuzzleNumber += room.getNumberOfPuzzle();

                    int roomTrapNumber = Mathf.RoundToInt(roomWeight * trapNumber);
                    room.setNumberOfTrap(roomTrapNumber);
                    realTrapNumber += room.getNumberOfTrap();
                }
            }
            if (realProducerNumber != producerNumber) producerNumber = realProducerNumber;
            if (realResourceNumber != resourceNumber) resourceNumber = realResourceNumber;
            if (realPuzzleNumber != puzzleNumber) puzzleNumber = realPuzzleNumber;
            if (realTrapNumber != trapNumber) trapNumber = realTrapNumber;


            primaryConsumerNumber = Mathf.RoundToInt((currentFloor.getPrimaryConsumerPerProducer() * producerNumber) * currentFloor.getPrimaryConsumerDensity());
            int realPrimaryConsumerNumber = 0;
            foreach (SpaceNode node in allNode)
            {
                if (node.isLeafNode())
                {
                    Room room = node.getRoom();
                    float roomWeight = (float)room.getRoomSize() / (float)floorSize;

                    if (Mathf.RoundToInt((roomWeight + extraPrimaryWeight) * primaryConsumerNumber) >= 1)
                    {
                        roomWeight = roomWeight + extraPrimaryWeight;
                        extraPrimaryWeight = 0;
                    }
                    else
                    {
                        extraPrimaryWeight += roomWeight;
                    }

                    int roomPrimaryConsumerNumber = Mathf.RoundToInt(roomWeight * primaryConsumerNumber);
                    room.setNumberOfPrimaryConsumer(roomPrimaryConsumerNumber);
                    realPrimaryConsumerNumber += room.getNumberOfPrimaryConsumer();
                }
            }
            if (realPrimaryConsumerNumber != primaryConsumerNumber) primaryConsumerNumber = realPrimaryConsumerNumber;

            secondaryConsumerNumber = Mathf.RoundToInt((currentFloor.getSecondaryConsumerPerPrimary() * primaryConsumerNumber) * currentFloor.getSecondaryConsumerDensity());
            currentFloorSecondaryDensity = currentFloor.getSecondaryConsumerDensity();
            secondaryConsumerPerPrimary = currentFloor.getSecondaryConsumerPerPrimary();
            int realSecondaryConsumerNumber = 0;
            foreach (SpaceNode node in allNode)
            {
                if (node.isLeafNode())
                {
                    Room room = node.getRoom();
                    float roomWeight = (float)room.getRoomSize() / (float)floorSize;
                    if(Mathf.RoundToInt((roomWeight +extraSecondaryWeight) * secondaryConsumerNumber) >= 1)
                    {
                        roomWeight = roomWeight + extraSecondaryWeight;
                        extraSecondaryWeight = 0;
                    }
                    else
                    {
                        extraSecondaryWeight += roomWeight;
                    }

                    int roomSecondaryConsumerNumber = Mathf.RoundToInt(roomWeight * secondaryConsumerNumber);
                    room.setNumberOfSecondaryConsumer(roomSecondaryConsumerNumber);
                    realSecondaryConsumerNumber += room.getNumberOfSecondaryConsumer();
                }
            }
            if (realSecondaryConsumerNumber != secondaryConsumerNumber) secondaryConsumerNumber = realSecondaryConsumerNumber;

        }

        public Behaviour GetBehaviour()
        {
            return Behaviour.Hunting;
        }

        public GameObject GetHuntingPrey(Entity entity,BiomeType biomeType)
        {
            if (entity == Entity.PrimaryConsumer)
            {

                if (ProducerList.Count <= 0) return null;

                ArrayList temporaryList = (ArrayList)ProducerList.Clone();


                while (temporaryList.Count > 0) {
                    int randomNumber = (int)UnityEngine.Random.Range(0, temporaryList.Count);

                    if (temporaryList[randomNumber] as GameObject == null){
                        ProducerList.Remove((GameObject)temporaryList[randomNumber]);
                        continue;
                    }

                    if (((GameObject)temporaryList[randomNumber]).GetComponent<Producer>().GetBiomeType() == biomeType)
                    {
                        return (GameObject)temporaryList[randomNumber];
                    }
                    else
                    {
                        temporaryList.RemoveAt(randomNumber);
                    }
                }
                /*
                bool found = false;
                GameObject prey=null;
                while (!found)
                {
                    prey = SelectableProducerList[UnityEngine.Random.Range(0, SelectableProducerList.Count)] as GameObject;
                    Debug.Log(prey);
                    if (prey.GetComponent<Producer>().GetBiomeType() == biomeType)
                    {
                        found = true;
                    }

                }*/

                return null;
            }
            else if(entity == Entity.SecondaryConsumer)
            {
                if (PrimaryConsumerList.Count <= 0) return null;

                ArrayList temporaryList = (ArrayList)PrimaryConsumerList.Clone();


                while (temporaryList.Count > 0)
                {
                    int randomNumber = (int)UnityEngine.Random.Range(0, temporaryList.Count);

                    if (temporaryList[randomNumber] as GameObject == null)
                    {
                        PrimaryConsumerList.Remove((GameObject)temporaryList[randomNumber]);
                        continue;
                    }

                    if (((GameObject)temporaryList[randomNumber]).GetComponent<Enemy>().GetBiomeType() == biomeType)
                    {
                        return (GameObject)temporaryList[randomNumber];
                    }
                    else
                    {
                        temporaryList.RemoveAt(randomNumber);
                    }
                }

                /*
                bool found = false;
                GameObject prey = null;
                while (!found)
                {
                    prey = SelectablePrimaryConsumerList[UnityEngine.Random.Range(0, SelectablePrimaryConsumerList.Count)] as GameObject;
                    Debug.Log(prey);
                    if (prey.GetComponent<Enemy>().GetBiomeType() == biomeType)
                    {
                        found = true;
                    }

                }*/

                return null;
            }
            return null;
        }

        public void AddEntityToDungeon(GameObject entity)
        {
            Enemy enemy = entity.GetComponent<Enemy>();
            Entity thisObjectEntity = Entity.None ;
            if (enemy!=null)thisObjectEntity = enemy.GetEntity();
            if (thisObjectEntity == Entity.PrimaryConsumer)
            {
                PrimaryConsumerList.Add(entity);
            }
            else if (thisObjectEntity == Entity.SecondaryConsumer)
            {
                SecondaryConsumerList.Add(entity);
            }
            else
            {
                ProducerList.Add(entity);
            }
        }

        public bool CanLayEgg(Entity entityType)
        {
            if (entityType == Entity.PrimaryConsumer)
            {
                if (PrimaryConsumerList.Count >= primaryConsumerNumber*2)
                {
                    return false;
                }
            }
            else
            {
                if (SecondaryConsumerList.Count >= GetCurrentMaxSecondaryConsumer()*1.5)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsNearlyExtinct(Entity entityType,BiomeType biomeType)
        {
            if (entityType == Entity.PrimaryConsumer)
            {
                int NumberOfSameBiomePrimary = 0;
                ArrayList temporaryList = (ArrayList)PrimaryConsumerList.Clone();
                foreach (GameObject i in temporaryList)
                {
                    if(i as GameObject == null)
                    {
                        PrimaryConsumerList.Remove(i);
                        continue;
                    }
                    if (i.GetComponent<Enemy>().GetBiomeType() == biomeType)
                    {
                        NumberOfSameBiomePrimary += 1;
                    }
                }

                if (NumberOfSameBiomePrimary > 1)
                {
                    return false;
                }
            }
            else
            {

                int NumberOfSameBiomeSecondary = 0;
                ArrayList temporaryList2 = (ArrayList)SecondaryConsumerList.Clone();
                foreach (GameObject i in temporaryList2)
                {
                    if (i as GameObject == null)
                    {
                        SecondaryConsumerList.Remove(i);
                        continue;
                    }
                    if (i.GetComponent<Enemy>().GetBiomeType() == biomeType)
                    {
                        NumberOfSameBiomeSecondary += 1;
                    }
                }

                if (NumberOfSameBiomeSecondary > 1)
                {
                    return false;
                }
            }

            return true;
        }

        private int GetCurrentMaxSecondaryConsumer()
        {
            return Mathf.RoundToInt((secondaryConsumerPerPrimary * PrimaryConsumerList.Count) * currentFloorSecondaryDensity);
        }

        public void RemoveGameObjectFromDungeon(GameObject gameObject)
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();
            Entity thisObjectEntity = Entity.None;
            if (enemy != null) thisObjectEntity = enemy.GetEntity();
            if (thisObjectEntity == Entity.PrimaryConsumer)
            {
                PrimaryConsumerList.Remove(gameObject);
                PrimaryConsumerList.Remove(gameObject);
            }
            else if (thisObjectEntity == Entity.PrimaryConsumer)
            {
                SecondaryConsumerList.Remove(gameObject);
                SecondaryConsumerList.Remove(gameObject);
            }
            else
            {
                ProducerList.Remove(gameObject);
                ProducerList.Remove(gameObject);
            }
        }
    }
}
