using System.Collections.Generic;
using Main.Meta.Upgrades.Parameters;
using NaughtyAttributes;
using NavySpade._PJ71.InventorySystem;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime
{
    [CreateAssetMenu(fileName = "ProductionBuilding", menuName = "Data/Building/ProductionBuilding")]
    public class ProductionBuildingPreset : BuildingPreset
    {
        public float ConversionСost;
        public FloatUpgradeableParameter ConversionSpeed; // производство в секундах
        
        public bool _doNotNeedInputResources;
        
        [HideIf(nameof(_doNotNeedInputResources))]
        public ResourceRequirements ResourceRequirement;
        
        public int MaxCapacityOfInputInventory;
        public ResourcePreset OutputResource;
        
        public override List<FloatUpgradeableParameter> GetParameters()
        {
            var result = base.GetParameters();
            result.Add(ConversionSpeed);
            return result;
        }
    }
}