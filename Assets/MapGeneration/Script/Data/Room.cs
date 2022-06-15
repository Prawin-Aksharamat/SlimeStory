using UnityEngine;

namespace Rogue.Data
{
    public class Room 
    {
        private int numberOfProducer;
        private int numberOfPrimaryConsumer;
        private int numberOfSecondaryConsumer;
        private int numberOfPuzzle;
        private int numberOfResource;
        private int numberOfTrap;

        private float producerDensity;
        private float primaryConsumerDensity;
        private float secondaryConsumerDensity;
        private float puzzleDensity;
        private float resourceDensity;
        private float trapDensity;


        private int roomSize;

        private int minTurn;
        private int maxTurn;
        private int probabilityIncrement;

        public Room(RoomTemplate roomTemplate)
        {
            minTurn = roomTemplate.getMinTurn();
            maxTurn = roomTemplate.getMaxTurn();
            probabilityIncrement = roomTemplate.getProbabilityIncrement();
            producerDensity= roomTemplate.getProducerDensity();
            primaryConsumerDensity= roomTemplate.getPrimaryConsumerDensity();
            secondaryConsumerDensity= roomTemplate.getSecondaryConsumerDensity();
            puzzleDensity= roomTemplate.getPuzzleDensity();
            resourceDensity= roomTemplate.getResourceDensity();
            trapDensity= roomTemplate.getTrapDensity();
    }

        private int[][] possibleDirection = new int[][]{
                new int[]{0,1},
                new int[] { 0,-1},
                new int[] { 1,0},
                new int[] { -1,0},
                new int[] { 1,1},
                new int[] { 1,-1},
                new int[] { -1,-1},
                new int[] { -1,1} };

        public int[] getRandomDirection() => possibleDirection[Mathf.RoundToInt(UnityEngine.Random.Range(0, possibleDirection.Length))];

        public int getRandomTurnNumber() => Mathf.RoundToInt(UnityEngine.Random.Range(minTurn, maxTurn));

        public int getProbabilityIncrement()
        {
            return probabilityIncrement;
        }

        public int getNumberOfProducer()
        {
            return Mathf.RoundToInt(numberOfProducer*producerDensity);
        }

        public int getNumberOfPrimaryConsumer()
        {
            return Mathf.RoundToInt(numberOfPrimaryConsumer * primaryConsumerDensity);
        }

        public int getNumberOfSecondaryConsumer()
        {
            return Mathf.RoundToInt(numberOfSecondaryConsumer * secondaryConsumerDensity);
        }

        public int getNumberOfResource()
        {
            return Mathf.RoundToInt(numberOfResource * resourceDensity);
        }

        public int getNumberOfTrap()
        {
            return Mathf.RoundToInt(numberOfTrap * trapDensity);
        }

        public int getNumberOfPuzzle()
        {
            return Mathf.RoundToInt(numberOfPuzzle * puzzleDensity);
        }

        public void setMinTurn(int minTurn) => this.minTurn = minTurn;
        public void setMaxTurn(int maxTurn) => this.maxTurn = maxTurn;
        public void setProbabilityIncrement(int probabilityIncrement) => this.probabilityIncrement = probabilityIncrement;

        public void setRoomSize(int roomSize) => this.roomSize = roomSize;
        public int getRoomSize() => this.roomSize;

        public void setNumberOfProducer(int numberOfProducer) => this.numberOfProducer = numberOfProducer;
        public void setNumberOfResource(int numberOfResource) => this.numberOfResource = numberOfResource;
        public void setNumberOfPuzzle(int numberOfPuzzle) => this.numberOfPuzzle = numberOfPuzzle;
        public void setNumberOfTrap(int numberOfTrap) => this.numberOfTrap = numberOfTrap;

        public void setNumberOfPrimaryConsumer(int numberOfPrimaryConsumer) => this.numberOfPrimaryConsumer = numberOfPrimaryConsumer;
        public void setNumberOfSecondaryConsumer(int numberOfSecondaryConsumer) => this.numberOfSecondaryConsumer = numberOfSecondaryConsumer;
    }
}