using System;
using NavySpade._PJ71.BuildingSystem;
using NavySpade._PJ71.InventorySystem;

namespace NavySpade._PJ71.Saving
{
    [Serializable]
    public struct BuildingPlaceSavingData
    {
        public BuildingType BuildingType;
        public ItemSavingInfo[] Inventory;
        public string BuildingGuid;

        //public BuildingInfo BuildingInfo;
    }
}