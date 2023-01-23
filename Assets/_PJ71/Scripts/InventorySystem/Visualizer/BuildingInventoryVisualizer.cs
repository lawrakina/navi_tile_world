using NavySpade._PJ71.InventorySystem.Items;
using NavySpade.Core.Runtime.Game;
using pj40.Core.Tweens.Runtime;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem.Visualizer
{
    public class BuildingInventoryVisualizer : InventoryVisualizer
    {
        [SerializeField] private Transform _targetPoint;
        [SerializeField] private TextInventoryView _textInventoryView;
        
        private ItemReceiveAnimation _itemReceiveAnimation;
        private InventoryVisualizer _inventoryVisualizerImplementation;
        public override Vector3 StackPos => _inventoryVisualizerImplementation.StackPos;

        public override bool HasFreeSpace => Inventory.HasFreeSpace;

        public override void Init(ItemSavingInfo[] saving, int maxSize = -1)
        {
            base.Init(saving, maxSize);
            _itemReceiveAnimation = GameContext.Instance.ReceiveItemAnimation;
            
            if(_textInventoryView)
                _textInventoryView.Init(this);
        }

        public override void Init(int maxSize = -1)
        {
            base.Init(maxSize);
            _itemReceiveAnimation = GameContext.Instance.ReceiveItemAnimation;
            
            if(_textInventoryView) 
                _textInventoryView.Init(this);
        }

        public override void AddItem(ItemObject itemObject)
        {
            if (Inventory.TryAdd(itemObject.Preset.Type, 1))
            {
                itemObject.transform.parent = null;
                _itemReceiveAnimation.PlayAnimation(itemObject, _targetPoint.position, false,
                    (i) =>
                    {
                        AddingCompleteInvoke(itemObject);
                        i.ReturnToPool();
                    });
            }
        }
        
        public override void AddItemInstant(ResourceType type, int amount)
        {
            Inventory.TryAdd(type, amount);
        }

        public override ItemObject PullItem(ResourceType type)
        {
            ItemInfo itemInfo = Inventory.GetItemInfo(type);
            Inventory.Reduce(itemInfo);
            return itemInfo.Preset.CreateObject(_targetPoint.position, Quaternion.identity);
        }
    }
}