using UnityEngine;
using UnityEngine.EventSystems;
using GameDevTV.UI.Inventories;

namespace GameDevTV.Inventories
{
    public class ItemRightClickHandler : MonoBehaviour, IPointerClickHandler
    {
        sfxPlayer sfx;

        private void Awake()
        {
            sfx = GameObject.FindGameObjectWithTag("SFXplayer").GetComponent<sfxPlayer>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                sfx.PlayInventoryClick();
                var thisSlot = GetComponent<InventorySlotUI>();
                if (thisSlot != null)
                {
                    var i = GameObject.FindWithTag("Player").GetComponent<Inventory>();
                    var itemAsEquipable = i.GetItemInSlot(thisSlot.getIndex()) as EquipableItem;
                    if (itemAsEquipable != null)
                    {
                        var e = GameObject.FindWithTag("Player").GetComponent<Equipment>();

                        var indexToEquip = 0;
                        var equipItemList = e.GetEquipableItemList(itemAsEquipable.GetAllowedEquipLocation());
                        for(int j = equipItemList.Length-1; j >= 0; j--)
                        {
                            if (equipItemList[j] == null)
                            {
                                indexToEquip = j;
                            }
                        }
                        var prevItem = e.GetItemInSlot(itemAsEquipable.GetAllowedEquipLocation(), indexToEquip);


                        e.AddItem(itemAsEquipable.GetAllowedEquipLocation(), itemAsEquipable, indexToEquip);
                        i.RemoveFromSlot(thisSlot.getIndex(), 1);

                        if (prevItem != null)
                        {
                            i.AddItemToSlot(thisSlot.getIndex(), prevItem, 1);
                        }
                    }
                    else
                    {
                        var itemAsConsumable = i.GetItemInSlot(thisSlot.getIndex()) as ConsumableItem;
                        itemAsConsumable.Use();
                        i.RemoveFromSlot(thisSlot.getIndex(), 1);
                    }
                }
                else
                {
                    var thisSlot2 = GetComponent<EquipmentSlotUI>();
                    var i = GameObject.FindWithTag("Player").GetComponent<Inventory>();
                    var e = GameObject.FindWithTag("Player").GetComponent<Equipment>();
                    if (i.AddToFirstEmptySlot(thisSlot2.GetItem(), 1))
                    {
                        e.RemoveItem(thisSlot2.getEquipLocation(), thisSlot2.getSlotIndex());
                    }
                }
            }
        }
    }
}