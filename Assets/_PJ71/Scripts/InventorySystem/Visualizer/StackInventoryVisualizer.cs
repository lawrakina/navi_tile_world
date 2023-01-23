using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.Scripts.Utils;
using NavySpade.Core.Runtime.Game;
using pj40.Core.Tweens;
using pj40.Core.Tweens.Runtime;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem.Visualizer
{
    public class StackInventoryVisualizer : InventoryVisualizer
    {
        [SerializeField] private Transform _container;
        [SerializeField] private AlignmentType _alignment;
        [SerializeField] private Vector2Int _stackSize;
        [SerializeField] private Vector3 _padding;
        
        [Foldout("Debug")] 
        [SerializeField] private bool _isDebug;
        
        [Foldout("Debug")] 
        [SerializeField] private int _zHeight; 
        
        private ItemReceiveAnimation _itemReceiveAnimation;
        
        private List<ItemObject> _allObjectsInStack = new List<ItemObject>();
        
        private List<MoveToTransformTween<ParabolaMovement, ItemObject>> _tweens = 
            new List<MoveToTransformTween<ParabolaMovement, ItemObject>>();
        private InventoryVisualizer _inventoryVisualizerImplementation;

        public override Vector3 StackPos => _inventoryVisualizerImplementation.StackPos;
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

        public override void AddItem(ItemObject itemObject)
        {
            if (Inventory.TryAdd(itemObject.Preset.Type, 1))
            {
                Vector3 spawnPosition = GetSpawnPosition(_allObjectsInStack.Count);
                MoveToTransformTween<ParabolaMovement, ItemObject> tween = _itemReceiveAnimation.PlayAnimation(
                    itemObject, 
                    () => spawnPosition,
                    true, (i) =>
                    {
                        _tweens.RemoveAll((t => t.TweenObject == i));
                    });
                
                _tweens.Add(tween);
                AddItemToStack(itemObject);
            }
        }

        public override void AddItemInstant(ResourceType type, int amount)
        {
            var resourcePreset = ItemManagementConfig.Instance.GetResourceAsset(type);
            Inventory.TryAdd(type, amount);
            for (int i = 0; i < amount; i++)
            {
                Vector3 spawnPosition = GetSpawnPosition(_allObjectsInStack.Count);
                var item = resourcePreset.CreateObject(spawnPosition, Quaternion.identity, _container);
                item.Preset = resourcePreset;
                AddItemToStack(item);
            }
        }

        public override ItemObject PullItem(ResourceType type)
        {
            ItemObject itemObject = _allObjectsInStack.Last();
            Inventory.Reduce(type);
            RemoveItemFromStack(itemObject, _allObjectsInStack.Count - 1);
            StopTween(itemObject);
            return itemObject;
        }

        private void StopTween(ItemObject itemObject)
        {
            var tween = _tweens.FirstOrDefault((t => t.TweenObject == itemObject));
            if (tween != null)
            {
                tween.Kill();
                _tweens.Remove(tween);
            }
        }
        
        private void AddItemToStack(ItemObject itemObject)
        {
            itemObject.transform.parent = _container;
            _allObjectsInStack.Add(itemObject);
        }

        private void RemoveItemFromStack(ItemObject itemObject, int itemIndex)
        {
            _allObjectsInStack.RemoveAt(itemIndex);
        }
        
        private Vector3 GetSpawnPosition(int indexOfItem)
        {
            Vector3 offsetForFirstPoint = GridUtils.GetOriginOffset(
                _alignment, _stackSize, new Vector3(_padding.x, _padding.z));
            
            int y = indexOfItem / (_stackSize.x * _stackSize.y);
            indexOfItem -= (y * _stackSize.x * _stackSize.y);
            
            int z = indexOfItem / _stackSize.x;
            int x = indexOfItem % _stackSize.x;

            Vector3 localPos = new Vector3(x * _padding.x, y * _padding.y, z * _padding.z);
            localPos -= offsetForFirstPoint;
            
            return transform.TransformPoint(localPos);
        }
        
        private void OnDrawGizmos()
        {
            if (_isDebug == false)
                return;
            
            Gizmos.color = Color.red;
            int total = _stackSize.x * _stackSize.y * _zHeight;
            for (int i = 0; i < total; i++)
            {
                Gizmos.DrawCube(GetSpawnPosition(i), Vector3.one * 0.1f);
            }
        }
    }
}