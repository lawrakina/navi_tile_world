using System;
using NavySpade._PJ71.BuildingSystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using UnityEngine;

namespace NavySpade.pj77.Tutorial.Condition
{
    [Serializable]
    [CustomSerializeReferenceName("BuildingComplete")]
    public class BuildingComplete: ITutorCondition
    {
        [SerializeField] private BuildingPlace _buildingPlace;
        
        public event Action ConditionChanged;
        
        public void Enable()
        {
            _buildingPlace.ProgressUpdated += CheckCondition;
        }

        public void Disable()
        {
            _buildingPlace.ProgressUpdated -= CheckCondition;
        }

        private void CheckCondition()
        {
            if (_buildingPlace.LeftResources <= 0)
            {
                ConditionChanged?.Invoke();
            }
        } 
    }
}