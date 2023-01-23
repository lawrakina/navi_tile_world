using System;
using System.Collections.Generic;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem
{
    public class ItemSelling : ItemReceiver
    {
        [SerializeField] private InventoryVisualizer _inventory;
        
        private ResourceRequirements _requirements;
        private readonly List<ItemObject> _itemsInAnimation = new List<ItemObject>();

        public event Action ProgressUpdated;
        
        public void Init(ResourceRequirements requirements)
        {
            _requirements = requirements;
            _inventory.AddingComplete += UpdateProgress;
        }

        private void OnDestroy()
        {
            _inventory.AddingComplete -= UpdateProgress;
        }

        protected override void OnEnter(IInventoryHandler inventory)
        {
            StartPickupItems(inventory);
        }

        protected override void OnExit(IInventoryHandler inventory)
        {
            
        }

        protected override void PickupItemsFrom(IInventoryHandler unitInventory)
        {
            if(_inventory.HasFreeSpace == false)
                return;
            
            ItemInfo itemInfo = unitInventory.GetItemInfo(_requirements.Preset.Type);
            if (itemInfo.Amount <= 0)
                return;

            if (itemInfo.Preset.ShowVisual)
            {
                ItemObject itemObject = unitInventory.PullItem(_requirements.Preset.Type);
                _itemsInAnimation.Add(itemObject);
                _inventory.AddItem(itemObject);
            }
            else
            {
                // PlayerStackVisualizer playerStack = (PlayerStackVisualizer) playerInventory;
                unitInventory.TakeResource(_requirements.Preset.Type);
                
                ItemObject itemObject = itemInfo.Preset.CreateObject(unitInventory.StackPos, Quaternion.identity);
                _itemsInAnimation.Add(itemObject);
                _inventory.AddItem(itemObject);
            }
        }
        
        private void UpdateProgress(ItemObject item)
        {
            _itemsInAnimation.Remove(item);
            ProgressUpdated?.Invoke();
        }
    }
}