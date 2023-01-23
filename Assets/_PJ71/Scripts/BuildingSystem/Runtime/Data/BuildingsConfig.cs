using System.Linq;
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime
{
    public class BuildingsConfig : ObjectConfig<BuildingsConfig>
    {
        [SerializeField] private BuildingConfig[] _configs;

        public BuildingConfig GetBuildingConfig(BuildingType type)
        {
            return _configs.FirstOrDefault((c) => c.BuildingType == type);
        }
    }
}