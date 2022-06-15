namespace Rogue.Data
{
    public class NodeSizeCap
    {
        public NodeSizeCap(int minX=0, int minY = 0, int maxX = 0, int maxY = 0)
        {
            this.minX = minX;
            this.minY = minY;
            this.maxX = minX*2-1;
            this.maxY = minY*2-1;

        }

        public int minX { get; }
        public int minY { get; }
        public int maxX { get; }
        public int maxY { get; }

        public bool isSizeXInRange(int sizeX) => (sizeX >= minX && sizeX <= maxX) ? true : false;
        public bool isSizeYInRange(int sizeY) => (sizeY >= minX && sizeY <= maxX) ? true : false;
    }
}