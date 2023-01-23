using System;
using NavySpade._PJ71.InventorySystem;

namespace NavySpade._PJ71.Saving
{
    [Serializable]
    public struct PlayerSavingData
    {
        public ItemSavingInfo[] Inventory;
    }
}