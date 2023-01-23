using System;
using NavySpade._PJ71.InventorySystem.Items;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem.Visualizer
{
    public abstract class InventoryVisualizer : MonoBehaviour, IInventoryHandler
    {
        [SerializeField] private bool _ignoreUncountableItem;
        
        public virtual Inventory Inventory { get; protected set; }
        
        public abstract bool HasFreeSpace { get; }

        public bool HasSpace => Inventory.HasFreeSpace;
        public abstract Vector3 StackPos { get; }

        public event Action<ItemObject> AddingComplete;
        
        public virtual void Init(ItemSavingInfo[] saving, int maxSize = -1)
        {
            Inventory = new Inventory(maxSize, _ignoreUncountableItem);

            if (saving != null)
            {
                foreach (var savedItem in saving)
                {
                    AddItemInstant(savedItem.Type, savedItem.Amount);
                }
            }
        }
        
        public virtual void Init(int maxSize = -1)
        {
            Inventory = new Inventory(maxSize, _ignoreUncountableItem);
        }

        public abstract void AddItem(ItemObject itemObject);

        public abstract void AddItemInstant(ResourceType type, int amount);
        
        public abstract ItemObject PullItem(ResourceType type);
        
        public virtual void TakeResource(ResourceType type, int amount = 1)
        {
            Inventory.Reduce(type, amount);
        }

        public virtual Items.ItemInfo GetItemInfo(ResourceType type)
        {
            return Inventory.GetItemInfo(type);
        }

        protected virtual void AddingCompleteInvoke(ItemObject item)
        {
            AddingComplete?.Invoke(item);
        }
    }
}