using UnityEngine;
using GameDevTV.Inventories;

public class PlayerDummy : MonoBehaviour
{
    [SerializeField] InventoryItem[] item;
    // Start is called before the first frame update
    void Start()
    {
        Inventory i = GetComponent<Inventory>();
        foreach (InventoryItem x in item)
        {
            i.AddToFirstEmptySlot(x, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
