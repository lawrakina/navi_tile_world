using NavySpade._PJ71.InventorySystem.Items;
using NavySpade.Core.Runtime.Game;
using pj40.Core.Tweens.Runtime;
using UnityEngine;

namespace NavySpade._PJ71.Tiles
{
    public class TileUnlocker : MonoBehaviour
    {
        private Tile _tile;

        private ItemReceiveAnimation _itemReceiveAnimation;
        private bool _gotTile;
        
        public void Init(Tile tile)
        {
            _tile = tile;
            _itemReceiveAnimation = GameContext.Instance.ReceiveItemAnimation;
        }

        public bool CanOpen()
        {
            if(_gotTile)
                return false;
            
            if(_tile == null)
                return false;
            
            if (_tile.CurrentState != Tile.TileState.CanUnlock)
                return false;

            return true;
        }

        public void Unlock(ItemObject itemObject)
        {
            itemObject.transform.parent = null;
            _tile.UnlockTile();
            _itemReceiveAnimation.PlayAnimation(itemObject, transform.position, false, (i) =>
            {
                i.ReturnToPool();
            });
            
            _gotTile = true;
        }
    }
}