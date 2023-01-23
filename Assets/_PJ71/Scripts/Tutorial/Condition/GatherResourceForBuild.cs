using System;
using NavySpade._PJ71.BuildingSystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using UnityEngine;

namespace NavySpade.pj77.Tutorial.Condition
{
    [Serializable]
    [CustomSerializeReferenceName("GatherResourceForBuild")]
    public class GatherResourceForBuild : ITutorCondition
    {
        [SerializeField] private InventoryVisualizer _inventoryVisualizer;
        [SerializeField] private BuildingPlace _buildingPlace;
        
        public event Action ConditionChanged;
        
        public void Enable()
        {
            _inventoryVisualizer.Inventory.ResourcesCountChanged += CheckCondition;
        }

        public void Disable()
        {
            _inventoryVisualizer.Inventory.ResourcesCountChanged -= CheckCondition;
        }

        private void CheckCondition(ItemInfo obj)
        {
            if (obj.Amount >= _buildingPlace.LeftResources)
            {
                ConditionChanged?.Invoke();
            }
        }
    }
}