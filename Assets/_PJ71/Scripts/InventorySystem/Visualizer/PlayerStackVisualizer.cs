using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade.Core.Runtime.Game;
using pj40.Core.Tweens.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;


namespace NavySpade._PJ71.InventorySystem.Visualizer
{
    public class PlayerStackVisualizer : InventoryVisualizer
    {
        [SerializeField] private Transform _container;
        
        private ItemReceiveAnimation _itemReceiveAnimation;
        
        private Vector3 _offset;
        private List<ItemObject> _allObjectsInStack = new List<ItemObject>();
        private List<UnitItem> _squad= new List<UnitItem>();

        public override Vector3 StackPos => _container.position;
        
        public override bool HasFreeSpace => Inventory.HasFreeSpace;

        public override void Init(ItemSavingInfo[] saving, int maxSize = -1)
        {
            base.Init(saving, maxSize);
            _itemReceiveAnimation = GameContext.Instance.ReceiveItemAnimation;
        }

        public override void Init(int maxSize = -1)
        {
            base.Init(maxSize);
            _itemReceiveAnimation = GameContext.Instance.ReceiveItemAnimation;
        }

        public void AddUnitToSquad(UnitItem item) {
            _squad.Add(item);
            item.ActorHolder.OnDied += holder => { _squad.Remove(item); };
        }

        public void AddItemToSquad(ItemObject itemObject) {
            if (itemObject.Preset.Type == ResourceType.Wood)
            {
                AddItemFromSquad(itemObject);
                return;
            }
            
            var index = Random.Range(0, _squad.Count + 1);
            if (index > _squad.Count) index = _squad.Count;
            if(index == _squad.Count)
                AddItemOnMe(itemObject);
            else
                _squad[index].Inventory.AddItemFromSquad(itemObject);
        }

        private void AddItemOnMe(ItemObject itemObject) {
            if (Inventory.TryAdd(itemObject.Preset.Type, 1))
            {
                Vector3 offset = GetOffsetInStack(itemObject.Preset);  
                _itemReceiveAnimation.PlayAnimation(
                    itemObject, 
                    () => _container.TransformPoint(offset),
                    _container);
                
                AddItemToStack(itemObject);
            }
        }

        private void AddItemFromSquad(ItemObject itemObject) {
            if (Inventory.TryAdd(itemObject.Preset.Type, 1))
            {
                Vector3 offset = GetOffsetInStack(itemObject.Preset);
                _itemReceiveAnimation.PlayAnimation(
                    itemObject,
                    () => _container.TransformPoint(offset),
                    _container);

                AddItemToStack(itemObject);
            }
        }

        public override void AddItem(ItemObject itemObject) {
            AddItemToSquad(itemObject);
        }

        public override void AddItemInstant(ResourceType type, int amount)
        {
            var resourcePreset = ItemManagementConfig.Instance.GetResourceAsset(type);
            Inventory.TryAdd(type, amount);
            
            if(resourcePreset.ShowVisual == false)
                return;
            
            for (int i = 0; i < amount; i++)
            {
                Vector3 offset = GetOffsetInStack(resourcePreset);
                var item = resourcePreset.CreateObject(_container.TransformPoint(offset), Quaternion.identity, _container);
                item.Preset = resourcePreset;
                AddItemToStack(item);
            }
        }

        public ItemObject PullItemToSquad(ResourceType type) {
            if (Inventory.GetItemInfo(type).Amount > 0)
                return PullItemFromMe(type);
            
            var items = _squad.Where(x => x.Inventory.Inventory.CurrentCount > 0).ToList();
            return items[Random.Range(0, items.Count - 1)].Inventory.PullItemFromSquad(type);
        }

        public override void TakeResource(ResourceType type, int amount = 1) {
            if (Inventory.GetItemInfo(type).Amount > 0)
            {
                base.TakeResource(type, amount);
                return;
            }
            
            var items = _squad.Where(x => x.Inventory.Inventory.CurrentCount >= amount).ToList();
            items[Random.Range(0, items.Count - 1)].Inventory.TakeResourceOnMe(type,amount);
        }

        public override ItemObject PullItem(ResourceType type) {
            return PullItemToSquad(type);
        }

        public override ItemInfo GetItemInfo(ResourceType type) {
            return GetFullItemInfo(type);
        }

        private ItemObject PullItemFromMe(ResourceType type) {
            var indexData = _allObjectsInStack
                            .Select((val, indexvalue) => new
                            {
                                Data = val,
                                IndexPosition = indexvalue
                            })
                            .First(n => n.Data.Type == type);
            
            Inventory.Reduce(type);
            RemoveItemFromStack(indexData.Data, indexData.IndexPosition);
            indexData.Data.transform.DOKill();
            return indexData.Data;
        }
        
        private Vector3 GetOffsetInStack(ResourcePreset data)
        {
            Vector3 halfSize = data.SizeInStack / 2;
            return _offset + halfSize;
        }

        private void AddItemToStack(ItemObject itemObject)
        {
            itemObject.transform.parent = _container;
            _allObjectsInStack.Add(itemObject);
            _offset += itemObject.Preset.SizeInStack;
        }

        private void RemoveItemFromStack(ItemObject itemObject, int itemIndex)
        {
            _offset -= itemObject.Preset.SizeInStack;
            _allObjectsInStack.RemoveAt(itemIndex);
            RecalculateObjectPositions(itemIndex);
        }

        private void RecalculateObjectPositions(int fromIndex)
        {
            if(fromIndex >= _allObjectsInStack.Count)
                return;
            
            _offset = Vector3.zero;
            for (int i = 0; i < _allObjectsInStack.Count; i++)
            {
                ItemObject itemObject = _allObjectsInStack[i];
                Vector3 pos = GetOffsetInStack(itemObject.Preset);
                itemObject.transform.DOLocalMove(pos, 0.5f);
                _offset += itemObject.Preset.SizeInStack;
            }
        }

        public ItemInfo GetFullItemInfo(ResourceType type) {
            var result = new ItemInfo(type,0);
            foreach (var unitItem in _squad)
            {
                result.Amount += unitItem.Inventory.GetItemInfoOnMe(type);
            }

            result.Amount += Inventory.GetItemInfo(type).Amount;
            return result;
        }

        public void ClearSquad() {
            _squad.Clear();
        }
    }
}