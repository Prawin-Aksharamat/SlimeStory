using System;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Stats;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// Provides a store for the items equipped to a player. Items are stored by
    /// their equip locations.
    /// 
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class Equipment : MonoBehaviour,IModifierProvider
    {
        // STATE
        Dictionary<EquipLocation, EquipableItem[]> equippedItems = new Dictionary<EquipLocation, EquipableItem[]>();

        // PUBLIC

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action equipmentUpdated;

        /// <summary>
        /// Return the item in the given equip location.
        /// </summary>
        /// 

        private void Awake()
        {
            equippedItems[EquipLocation.Core] = new EquipableItem[4];
        }

        public EquipableItem GetItemInSlot(EquipLocation equipLocation,int slotIndex)
        {
            if (!equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return (equippedItems[equipLocation])[slotIndex];
        }

        /// <summary>
        /// Add an item to the given equip location. Do not attempt to equip to
        /// an incompatible slot.
        /// </summary>
        public void AddItem(EquipLocation slot, EquipableItem item, int slotIndex)
        {
            Debug.Assert(item.GetAllowedEquipLocation() == slot);

            (equippedItems[slot])[slotIndex] = item;

            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }

        /// <summary>
        /// Remove the item for the given slot.
        /// </summary>
        public void RemoveItem(EquipLocation slot, int slotIndex)
        {
            equippedItems[slot][slotIndex]=null;
            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }
        
        public EquipableItem[] GetEquipableItemList(EquipLocation equipLocation)
        {
            return equippedItems[equipLocation];
        }

        public IEnumerable<float> GetAdditiveModifiers(Stats stat)
        {
            foreach(EquipableItem x in equippedItems[EquipLocation.Core])
            {
                if (x == null) yield return 0;
                else
                {
                    yield return x.GetAdditiveModifier(stat);
                }
            }

        }
        public IEnumerable<float> GetPercentageModifiers(Stats stat)
        {
            foreach (EquipableItem x in equippedItems[EquipLocation.Core])
            {
                if (x == null) yield return 0;
                else
                {
                    yield return x.GetPercentageModifier(stat);
                }
            }
        }
    }
}