using UnityEngine;
using UnityEditor;

namespace Rogue.Map
{
    public class VisulizedSpacePartition : MonoBehaviour
    {
        private SpaceNode current;
        [SerializeField] int[] boundaryBox;

        public void SetSpaceNode(SpaceNode i)
        {
            current = i;
            boundaryBox = i.getBoundaryBox();
        }

        private void OnDrawGizmos()
        {
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(current.getSizeX(), current.getSizeY(), 0));
        }
    }
}
