using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryExample.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;
        private TurnOrder turnOrder;
        private bool isLocked = false;
        sfxPlayer sfx;

        private void Awake()
        {
            sfx = GameObject.FindGameObjectWithTag("SFXplayer").GetComponent<sfxPlayer>();
        }

        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
            turnOrder= GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnOrder>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(toggleKey) && turnOrder.AllowPlayerInput() && !isLocked)
            {
                if (uiContainer.activeSelf)
                {
                    sfx.PlayInventoryClose();
                }
                else
                {
                    sfx.PlayInventoryOpen();
                }
                uiContainer.SetActive(!uiContainer.activeSelf);
                GetComponentInParent<Player>().TriggerIsOpenUI();
            }
        }

        public void LockInventory() => isLocked = true;
        public void UnlockInventory() => isLocked = false;
    }
}