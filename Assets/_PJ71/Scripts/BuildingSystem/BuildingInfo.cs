using System;
using System.Collections.Generic;
using Main.Meta.Upgrades.Parameters;
using NavySpade._PJ71.InventorySystem;

namespace NavySpade._PJ71.BuildingSystem
{
    [Serializable]
    public class BuildingInfo
    {
        public string Guid;
        public ItemSavingInfo[] InputInventory;
        public ItemSavingInfo[] OutputInventory;
        public ItemSavingInfo[] RepairInventory;
        public int IsDestroyed;
        public float HP;
        public int Level = 1;
        public List<UpgradeInfo> UpgradeInfos;
    }

    [Serializable]
    public class UpgradeInfo
    {
        public ParameterTypes Type;
        public int Level;
    }
}