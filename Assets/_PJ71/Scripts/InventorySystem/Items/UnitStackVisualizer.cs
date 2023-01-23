using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade.Core.Runtime.Game;
using pj40.Core.Tweens.Runtime;
using UnityEngine;


namespace NavySpade._PJ71.InventorySystem.Items {
    public class UnitStackVisualizer : InventoryVisualizer {
        [SerializeField] private Transform _container;
        
        private PlayerStackVisualizer _owner;
        private Vector3 _offset;
        private List<ItemObject> _allObjectsInStack = new List<ItemObject>();
        private ItemReceiveAnimation _itemReceiveAnimation;

        public override bool HasFreeSpace => Inventory.HasFreeSpace;
        public override Inventory Inventory => base.Inventory;

        public override Vector3 StackPos => _container.position;

        public PlayerStackVisualizer Owner => _owner;

        public void Init(PlayerStackVisualizer owner) {
            _owner = owner;
            base.Init();
            _itemReceiveAnimation = GameContext.Instance.ReceiveItemAnimation;
        }
        
        
        public void ToDefault() {
            _owner = null;
            _allObjectsInStack.Clear();
            Inventory.Clear();
            Inventory = null;
        }

        public void AddItemFromSquad(ItemObject itemObject) {
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
            _owner.AddItemToSquad(itemObject);
        }

        public override void AddItemInstant(ResourceType type, int amount) {
            var resourcePreset = ItemManagementConfig.Instance.GetResourceAsset(type);
            Inventory.TryAdd(type, amount);

            if (resourcePreset.ShowVisual == false)
                return;

            for (int i = 0; i < amount; i++)
            {
                Vector3 offset = GetOffsetInStack(resourcePreset);
                var item = resourcePreset.CreateObject(_container.TransformPoint(offset), Quaternion.identity,
                                                       _container);
                item.Preset = resourcePreset;
                AddItemToStack(item);
            }
        }

        public override void TakeResource(ResourceType type, int amount = 1) {
            _owner.TakeResource(type, amount);
        }

        public void TakeResourceOnMe(ResourceType type, int amount) {
            base.TakeResource(type, amount);
        }

        public override ItemObject PullItem(ResourceType type) {
            return _owner.PullItemToSquad(type);
        }

        public ItemObject PullItemFromSquad(ResourceType type) {
            var indexData = _allObjectsInStack
                            .Select((val, indexvalue) => new{
                                Data = val,
                                IndexPosition = indexvalue
                            })
                            .First(n => n.Data.Type == type);

            Inventory.Reduce(type);
            RemoveItemFromStack(indexData.Data, indexData.IndexPosition);
            indexData.Data.transform.DOKill();
            return indexData.Data;
        }

        public override ItemInfo GetItemInfo(ResourceType type) {
            return _owner.GetFullItemInfo(type);
            // return base.GetItemInfo(type);
        }

        private Vector3 GetOffsetInStack(ResourcePreset data) {
            Vector3 halfSize = data.SizeInStack / 2;
            return _offset + halfSize;
        }

        private void AddItemToStack(ItemObject itemObject) {
            itemObject.transform.parent = _container;
            _allObjectsInStack.Add(itemObject);
            _offset += itemObject.Preset.SizeInStack;
        }

        private void RemoveItemFromStack(ItemObject itemObject, int itemIndex) {
            _offset -= itemObject.Preset.SizeInStack;
            _allObjectsInStack.RemoveAt(itemIndex);
            RecalculateObjectPositions(itemIndex);
        }

        private void RecalculateObjectPositions(int fromIndex) {
            if (fromIndex >= _allObjectsInStack.Count)
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

        public int GetItemInfoOnMe(ResourceType type) {
            return Inventory.GetItemInfo(type).Amount;
        }

        public void Clear() {
            var children = new List<GameObject>();
            foreach (Transform child in _container) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }
    }
}