using System;
using System.Collections.Generic;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade._PJ71.UI;
using UnityEngine;
using ItemInfo = NavySpade._PJ71.InventorySystem.Items.ItemInfo;

namespace NavySpade
{
    public class RepairBuildingTrigger : ItemReceiver, IRequirementsHolder
    {
        [SerializeField] private InventoryVisualizer _inventory;
        [SerializeField] private RequirementsView _view;
        
        private ResourceRequirements _resourceRequirements;
        private readonly List<ItemObject> _itemsInAnimation = new List<ItemObject>();
        private bool _resourcesCollected;

        public ResourceRequirements Requirement => _resourceRequirements;
        
        public int LeftResources => _resourceRequirements.Value -
                                    (_inventory.GetItemInfo(_resourceRequirements.Preset.Type).Amount - _itemsInAnimation.Count);
        
        public event Action ProgressUpdated;
        
        public event Action RepairFinished;
        
        public void Init(ResourceRequirements resourceRequirements)
        {
            _inventory.Init();
            _inventory.AddingComplete += UpdateProgress;
            _resourceRequirements = resourceRequirements;
            _view.Init(this);
            _resourcesCollected = false;
        }
        
        protected override void OnEnter(IInventoryHandler inventory)
        {
            StartPickupItems(inventory);
        }

        protected override void OnExit(IInventoryHandler inventory)
        {
            Gui.Instance.ChooseBuildingView.Hide();
        }

        protected override void PickupItemsFrom(IInventoryHandler playerInventory)
        {
            if (_resourcesCollected)
                return;

            ItemInfo itemInfo = playerInventory.GetItemInfo(_resourceRequirements.Preset.Type);
            if (itemInfo.Amount <= 0)
                return;
            
            if (itemInfo.Preset.ShowVisual)
            {
                ItemObject itemObject = playerInventory.PullItem(_resourceRequirements.Preset.Type);
                _itemsInAnimation.Add(itemObject);
                _inventory.AddItem(itemObject);
            }
            else
            {
                if (playerInventory is PlayerStackVisualizer playerStackVisualizer)
                {
                    // PlayerStackVisualizer playerStack = (PlayerStackVisualizer) playerInventory;
                    playerInventory.TakeResource(_resourceRequirements.Preset.Type);
                
                    ItemObject itemObject = itemInfo.Preset.CreateObject(playerStackVisualizer.StackPos, Quaternion.identity);
                    _itemsInAnimation.Add(itemObject);
                    _inventory.AddItem(itemObject);    
                }else if (playerInventory is UnitStackVisualizer unitStackVisualizer)
                {
                    // PlayerStackVisualizer playerStack = (PlayerStackVisualizer) playerInventory;
                    playerInventory.TakeResource(_resourceRequirements.Preset.Type);
                
                    ItemObject itemObject = itemInfo.Preset.CreateObject(unitStackVisualizer.StackPos, Quaternion.identity);
                    _itemsInAnimation.Add(itemObject);
                    _inventory.AddItem(itemObject);  
                }
                
            }
            
            if (_inventory.GetItemInfo(_resourceRequirements.Preset.Type).Amount >= _resourceRequirements.Value)
            {
                _resourcesCollected = true;
            }
        }
        
        private void UpdateProgress(ItemObject item)
        {
            _itemsInAnimation.Remove(item);
            ProgressUpdated?.Invoke();
            
            if (_resourcesCollected)
            {
                _inventory.AddingComplete -= UpdateProgress;
                RepairFinished?.Invoke();
            }
        }


    }
}
