using System;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade._PJ71.Utils;
using UnityEngine;
using UnityEngine.Events;


namespace NavySpade._PJ71.Tiles {
    public class UnitSquadTileUnlockerTrigger : MonoBehaviour {
        [SerializeField] private TileUnlockShower _tileUnlockShower;
        [SerializeField] private UnityEvent _onHasResources;
        [SerializeField] private UnityEvent _onNotHasResources;
        
        private InventoryVisualizer _inventory;
        private int _cannotOpenTiles;
        private bool _isOn = false;

        private void Awake() {
            _tileUnlockShower.gameObject.SetActive(false);
        }

        public void Init(InventoryVisualizer inventory) {
            _inventory = inventory;
            _isOn = true;
            _tileUnlockShower.gameObject.SetActive(true);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(!_isOn) return;
            if (other.TryGetComponent(out TileUnlocker tileUnlocker))
            {
                if (tileUnlocker.CanOpen())
                {
                    ItemInfo itemInfo = _inventory.GetItemInfo(ResourceType.Tile);
                    if (itemInfo.Amount <= 0)
                    {
                        _cannotOpenTiles++;
                    }
                    else
                    {
                        ItemObject itemObject = _inventory.PullItem(ResourceType.Tile);
                        tileUnlocker.Unlock(itemObject);
                    }
                }
                
                CheckState();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(!_isOn) return;

            _cannotOpenTiles = Mathf.Clamp(_cannotOpenTiles - 1, 0, int.MaxValue);
            CheckState();
        }

        private void CheckState()
        {
            if (_cannotOpenTiles > 0)
            {
                _onHasResources?.Invoke();
            }
            else
            {
                _onNotHasResources?.Invoke();
            }
        }
    }
}