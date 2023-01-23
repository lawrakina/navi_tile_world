using Core.UI.Counters;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using UnityEngine;
using ItemInfo = NavySpade._PJ71.InventorySystem.Items.ItemInfo;

namespace NavySpade._PJ71.UI
{
    public class ResourcePresenter : MonoBehaviour
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private DigitalCounter _counter;

        private Inventory _inventory;
        
        public void Init(Inventory inventory)
        {
            _inventory = inventory;
            _inventory.ResourcesCountChanged += UpdateView;
            _counter.UpdateValueInstantly(_inventory.GetItemInfo(_resourceType).Amount);
        }

        private void OnDestroy()
        {
            _inventory.ResourcesCountChanged -= UpdateView;
        }

        private void UpdateView(ItemInfo obj)
        {
            if(obj.Preset.Type != _resourceType)
            {
                return;
            }
            
            _counter.UpdateValue(obj.Amount);
        }
    }
}