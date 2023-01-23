using NavySpade._PJ71.InventorySystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade._PJ71.InventorySystem.Visualizer
{
    public class TextInventoryView : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onEmpty;
        [SerializeField] private UnityEvent _onHasResource;
        [SerializeField] private TextMeshPro _amountText;

        private InventoryVisualizer _inventoryVisualizer;
        private int _currentValue;
        
        public void Init(InventoryVisualizer inventoryVisualizer)
        {
            _inventoryVisualizer = inventoryVisualizer;
            //_inventoryVisualizer.AddingComplete += OnAddingItem;
            _inventoryVisualizer.Inventory.ResourcesCountChanged += OnRemoveItem;

            ItemInfo itemInfo = _inventoryVisualizer.Inventory.GetItemInfo(ResourceType.Any);
            _currentValue = itemInfo.Amount;
            UpdateView();
        }
        
        private void OnDestroy()
        {
            // if (_inventoryVisualizer)
            // {
            //     _inventoryVisualizer.AddingComplete += OnAddingItem;
            //     
            // }
            
            if (_inventoryVisualizer.Inventory != null)
            {
                _inventoryVisualizer.Inventory.ResourcesCountChanged += OnRemoveItem;
            }
        }
        
        private void OnRemoveItem(ItemInfo obj)
        {
            // if(_currentValue <= obj.Amount)
            //     return;

            _currentValue = obj.Amount;
            UpdateView();
        }

        private void OnAddingItem(ItemObject obj)
        {
            _currentValue = _inventoryVisualizer.Inventory.GetItemInfo(obj.Type).Amount;
            UpdateView();
        }
        
        private void UpdateView()
        {
            if (_currentValue == 0)
            {
                _onEmpty.Invoke();
            }
            else
            {
                _onHasResource.Invoke();
                if (_inventoryVisualizer.Inventory.HasMax)
                {
                    _amountText.text = _currentValue + "/" + _inventoryVisualizer.Inventory.MaxSize;
                }
                else
                {
                    _amountText.text = _currentValue.ToString();
                }
            }
        }
    }
}