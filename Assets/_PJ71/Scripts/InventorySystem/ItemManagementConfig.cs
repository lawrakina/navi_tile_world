using System.Collections.Generic;
using System.Linq;
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem
{
    public class ItemManagementConfig : ObjectConfig<ItemManagementConfig>
    {
        public int InventorySize;
        public float MiningSpeed = 1f;
        
        [Tooltip("Cкорость анимации получения/сдачи ресурсов")]
        public float ReceivingResourceSpeed;
        
        [Tooltip("Задержка между получением ресурсов")]
        public float DelayBetweenResourceRequest;

        [SerializeField] private ResourcePreset[] AllResources;

        private Dictionary<ResourceType, ResourcePreset> ResourceDict;

        public void Init()
        {
            ResourceDict = new Dictionary<ResourceType, ResourcePreset>();
            //TODO: load from specific folder
            
        }
        
        public ResourcePreset GetResourceAsset(ResourceType type)
        {
            return AllResources.FirstOrDefault((r) => r.Type == type);
        }
    }
}