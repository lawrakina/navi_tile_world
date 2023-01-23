using System;
using NavySpade._PJ71.InventorySystem.Items;
using UnityEngine;


namespace NavySpade._PJ71.InventorySystem
{
    public interface IInventoryHandler
    {
        bool HasSpace { get; }
        Vector3 StackPos { get; }

        public void AddItem(ItemObject itemObject);

        public Items.ItemInfo GetItemInfo(ResourceType type);
        
        public ItemObject PullItem(ResourceType type);

        public void TakeResource(ResourceType type, int amount = 1);
    }
}