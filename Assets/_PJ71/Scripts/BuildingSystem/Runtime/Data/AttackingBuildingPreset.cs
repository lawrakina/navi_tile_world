using Core.Actors;
using NavySpade.PJ70.Core.Ammunition;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime
{
    [CreateAssetMenu(fileName = "AttackingBuildingPreset", menuName = "Data/Building/AttackingBuilding")]
    public class AttackingBuildingPreset : BuildingPreset
    {
        public AttackPriority AttackPriority;
        public ShootingActorPreset ActorPreset;
    }
    
    public enum AttackPriority
    {
        Unit,
        Building
    }
}