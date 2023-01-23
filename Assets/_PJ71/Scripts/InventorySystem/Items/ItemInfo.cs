using System;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem.Items
{
    [Serializable]
    public class ItemInfo
    {
        public ResourcePreset Preset;
        public int Amount;
        
        public ItemInfo(ResourcePreset preset, int amount)
        {
            Preset = preset;
            Amount = amount;
        }
        
        public ItemInfo(ResourceType type, int amount)
        {
            Preset = ItemManagementConfig.Instance.GetResourceAsset(type);
            Amount = amount;
        }
    }
}