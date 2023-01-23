using UnityEngine;

namespace NavySpade._PJ71.UI
{
    public interface IItemRewardData
    {
        public Sprite ChestLockedIcon { get; }
        public Sprite ChestUnlockIcon { get; }
        public string Name { get; }
    }
}