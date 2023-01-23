using System.Collections.Generic;
using Main.Meta.Upgrades.Parameters;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.Tutorial;
using NavySpade.NavySpade.Modules.Meta.Runtime.Upgrades;
using NavySpade.pj77.Tutorial;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem
{
    [CreateAssetMenu(fileName = "Building", menuName = "Data/Building/BuildingPreset")]
    public class BuildingPreset : ScriptableObject, IUpgradeable
    {
        [SerializeField] private ResourceRequirements _buildRequirements;
        public ResourceRequirements RepairCost;
        public BuildingHandler Prefab;
        public float Hp;
        
        [Range(0, 100)]
        public float BreakingPercent;

        public ResourceRequirements BuildRequirements
        {
            get
            {
                if (TutorialController.InstanceExists && TutorialController.Instance.TutorDone == false)
                {
                    return TutorialConfig.Instance.TutorialResourceRequirements;
                }

                return _buildRequirements;
            }
        }
        
        public virtual List<FloatUpgradeableParameter> GetParameters()
        {
            return new List<FloatUpgradeableParameter>();
        }
    }
}