using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem
{
    public class ItemPickuper : ItemReceiver
    {
        [SerializeField] private InventoryVisualizer _inventory;
        
        protected override void OnEnter(IInventoryHandler inventory)
        {
            StartPickupItems(inventory);
        }

        protected override void OnExit(IInventoryHandler inventory)
        {
            
        }

        protected override void PickupItemsFrom(IInventoryHandler playerInventory)
        {
            if(_inventory.Inventory.IsEmpty)
                return;
            
            if(playerInventory.HasSpace == false)
                return;
            
            ItemObject itemObject = _inventory.PullItem(ResourceType.Any);
            playerInventory.AddItem(itemObject);
        }
    }
}