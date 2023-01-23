using Core.Meta.Shop.Upgrades;
using NaughtyAttributes;
using NavySpade._PJ71.UI;
using NavySpade.Meta.Runtime.Shop.Items;
using NavySpade.Meta.Runtime.Unlocks;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "Data/Building/Config")]
    public class BuildingConfig : ScriptableObject, IItemRewardData
    {
        public BuildingType BuildingType;
        [SerializeField] private string _name;

        [ShowAssetPreview(32, 32)]
        public Sprite Icon;

        [ShowAssetPreview(32, 32)]
        [SerializeField]
        private Sprite _chestLockedIcon;
        
        [ShowAssetPreview(32, 32)]
        [SerializeField]
        public Sprite _chestUnlockIcon;

        public BuildingPreset[] Presets;
        public UpgradeSO[] Upgrades;

        public BuildingPreset GetPreset(int level = 1)
        {
            return Presets[level - 1];
        }

        public Sprite ChestLockedIcon => _chestLockedIcon;
        
        public Sprite ChestUnlockIcon => _chestUnlockIcon;

        public string Name => _name;
    }
}