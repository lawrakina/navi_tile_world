using NavySpade._PJ71.InventorySystem;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime
{
    [CreateAssetMenu(fileName = "MinedObject", menuName = "Data/Building/MinedObject")]
    public class MinedObjectConfig : ScriptableObject
    {
        public ResourcePreset ResourcePreset;
        public int Capacity;
        
        public float FirstRecoveryTime = 1;
        public float CapacityRecoveryTime;
        
        [Min(1)]
        public int RecoveryAmount;
    }
}