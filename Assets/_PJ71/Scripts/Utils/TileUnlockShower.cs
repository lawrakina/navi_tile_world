using NavySpade._PJ71.Tiles;
using UnityEngine;

namespace NavySpade._PJ71.Utils
{
    public class TileUnlockShower : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Tile tile))
            {
                tile.ShowTileContentPreview(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Tile tile))
            {
                tile.ShowTileContentPreview(false);
            }
        }
    }
}