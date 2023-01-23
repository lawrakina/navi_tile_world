using System;
using System.Collections.Generic;
using System.Linq;
using NavySpade._PJ71.InventorySystem.Items;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem
{
    public class Inventory
    {
        private Dictionary<ResourceType, ItemInfo> _itemsDict;
        private int _maxSize;
        private bool _ignoreUncountableItem;

        public Inventory(int maxSize, bool ignoreUncountableItem)
        {
            _itemsDict = new Dictionary<ResourceType, ItemInfo>();
            _ignoreUncountableItem = ignoreUncountableItem;
            _maxSize = maxSize;
        }

        public bool HasMax => _maxSize > 0;

        public int MaxSize => _maxSize;

        public int CurrentCount { get; private set; }

        public bool IsEmpty => CurrentCount <= 0;

        public bool HasFreeSpace
        {
            get
            {
                if (HasMax == false)
                    return true;

                return CurrentCount < _maxSize;
            }
        }

        public event Action<ItemInfo> ResourcesCountChanged;

        public ItemInfo GetItemInfo(ResourceType type)
        {
            if (_itemsDict.ContainsKey(type))
            {
                return _itemsDict[type];
            }

            if (type == ResourceType.Any && _itemsDict.Count > 0)
            {
                return _itemsDict.First().Value;
            }

            return new ItemInfo(type, 0);
        }

        public bool TryAdd(ResourceType type, int amount)
        {
            if (HasFreeSpace == false)
                return false;

            AddItem(type, amount);
            return true;
        }

        public void AddItem(ResourceType type, int amount = 1)
        {
            if (_itemsDict.ContainsKey(type) == false)
            {
                _itemsDict.Add(type, new ItemInfo(type, 0));
            }

            ItemInfo itemInfo = _itemsDict[type];
            itemInfo.Amount = Mathf.Clamp(itemInfo.Amount + amount, 0, Int32.MaxValue);

            UpdateCurrentCount();
            ResourcesCountChanged?.Invoke(itemInfo);
        }

        public void Reduce(ResourceType type, int amount = 1)
        {
            ItemInfo itemInfo = null;
            if (type == ResourceType.Any)
            {
                itemInfo = _itemsDict.First().Value;
            }
            else
            {
                if (_itemsDict.ContainsKey(type) == false)
                    return;

                itemInfo = _itemsDict[type];
            }

            Reduce(itemInfo, amount);
        }

        public void Reduce(ItemInfo itemInfo, int amount = 1)
        {
            itemInfo.Amount = Mathf.Clamp(itemInfo.Amount - amount, 0, Int32.MaxValue);
            UpdateCurrentCount();
            ResourcesCountChanged?.Invoke(itemInfo);
        }

        public void Clear()
        {
            _itemsDict.Clear();
            CurrentCount = 0;
        }

        private void UpdateCurrentCount()
        {
            CurrentCount = _itemsDict.Sum((i) =>
            {
                if (i.Value.Preset.IsCountInInventory)
                {
                    return i.Value.Amount;
                }
                
                return _ignoreUncountableItem ? i.Value.Amount : 0;
            });
        }

        public ItemSavingInfo[] GetSavingData()
        {
            ItemSavingInfo[] inventoryInfo = new ItemSavingInfo[_itemsDict.Count];
            int index = 0;
            foreach (var item in _itemsDict)
            {
                inventoryInfo[index].Type = item.Key;
                inventoryInfo[index].Amount = item.Value.Amount;
                index++;
            }

            return inventoryInfo;
        }
    }
}