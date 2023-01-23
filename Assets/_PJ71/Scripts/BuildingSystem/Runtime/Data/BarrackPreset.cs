using Core.Actors;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime
{
    [CreateAssetMenu(fileName = "Barrack", menuName = "Data/Building/Barrack")]
    public class BarrackPreset : ProductionBuildingPreset
    {
        public ActorPreset ActorPreset;
    }
}