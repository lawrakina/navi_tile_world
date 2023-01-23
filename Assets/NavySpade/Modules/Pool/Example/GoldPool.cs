using NavySpade._PJ71.InventorySystem.Items;
using UnityEngine;

namespace NS.Core.Utils.Pool.Example
{
    public class GoldPool : PoolInGame<Gold>
    {
        private void Awake()
        {
            Initialize();
        }
    }
}