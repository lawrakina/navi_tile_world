using System;
using NavySpade._PJ71.BuildingSystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using UnityEngine;

namespace NavySpade.pj77.Tutorial.Condition
{
    [Serializable]
    [CustomSerializeReferenceName("ResourceChanged")]
    public class ResourceChanged : ITutorCondition
    {
        [SerializeField] private InventoryVisualizer _inventoryVisualizer;
     
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
            ConditionChanged?.Invoke();
        }
    }
}