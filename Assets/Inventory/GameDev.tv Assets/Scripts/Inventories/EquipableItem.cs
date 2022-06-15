using UnityEngine;
using Rogue.Stats;
using System.Collections.Generic;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Equipable Item"))]
    public class EquipableItem : InventoryItem
    {
        [SerializeField] EquipableStatModifier[] EquipmentModifier;
        private Dictionary<Stats, EquipableStatModifier> statModDict = new Dictionary<Stats, EquipableStatModifier>();

        // CONFIG DATA
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;

        // PUBLIC

        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }

        private void CreateDictionary()
        {
            foreach (EquipableStatModifier x in EquipmentModifier)
            {
                Debug.Log(x);
                statModDict.Add(x.GetStat(), x);
            }
        }

        public float GetAdditiveModifier(Stats stat) {

            if (statModDict.Count==0)
            {
                CreateDictionary();
            }

            if (statModDict.ContainsKey(stat))
                return statModDict[stat].GetAdditiveModifier();


            return 0;
        }

        public float GetPercentageModifier(Stats stat) {

            if (statModDict.Count == 0)
            {
                CreateDictionary();
            }

            if (statModDict.ContainsKey(stat)) return statModDict[stat].GetPercentageModifier();

            return 0;
        }

        [System.Serializable]
        private class EquipableStatModifier{
            [SerializeField] Stats stat;
            [SerializeField] float AdditiveModifier;
            [SerializeField] float PercentangeModifier;

            public Stats GetStat() => stat;
            public float GetAdditiveModifier() => AdditiveModifier;
            public float GetPercentageModifier() => PercentangeModifier;
            }
    }
}