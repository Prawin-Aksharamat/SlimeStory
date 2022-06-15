using UnityEngine;

namespace Rogue.Data
{

    [CreateAssetMenu(menuName = "Dungeon")]
    public class Dungeon : ScriptableObject
    {
        

        [SerializeField] Floor[] floor;


        public Floor getFloor(int floorNumber) => floor[floorNumber];

    }
}

