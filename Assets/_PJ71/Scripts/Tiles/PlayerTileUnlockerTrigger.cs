using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade._PJ71.Tiles
{
    public class PlayerTileUnlockerTrigger : MonoBehaviour
    {
        [SerializeField] private InventoryVisualizer _inventory;
        [SerializeField] private UnityEvent _onHasResources;
        [SerializeField] private UnityEvent _onNotHasResources;

        private int _cannotOpenTiles;
        
        private void OnTriggerEnter(Collider other)
        {
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