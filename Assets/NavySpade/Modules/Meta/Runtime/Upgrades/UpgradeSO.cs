using System;
using System.Linq;
using Main.Meta.Upgrades.Parameters;
using NavySpade.Meta.Runtime.Upgrades;
using NavySpade.Modules.Saving.Runtime;
using UnityEngine;

namespace Core.Meta.Shop.Upgrades
{
    [CreateAssetMenu(fileName = "UpgradeSO", menuName = "Data/Upgrade")]
    public class UpgradeSO : ScriptableObject
    {
        public ParameterTypes Type;
        public UpgradableProduct[] Upgrades;
        public bool IsLoopedUpgrades;
        
        private string PrefsKey => $"shop.{name}.Upgrades";
        private UpgradeReward[] _upgrades;
        
        public event Action<UpgradeReward> Upgraded;
        
        UpgradeReward[] AllUpgrades
        {
            get
            {
                if (_upgrades == null)
                    _upgrades = Upgrades.Select((s) => s.Reward).ToArray();

                return _upgrades;
            }
        }
        
        public int CurrentUpgradeIndex
        {
            get => SaveManager.Load(PrefsKey, 0);
            set
            {
                if (IsLoopedUpgrades)
                {
                    value = (int) Mathf.Repeat(value, Upgrades.Length - 1);
                }
                else
                {
                    value = Mathf.Clamp(value, 0, Upgrades.Length - 1);
                }

                SaveManager.Save(PrefsKey, value);
                Upgraded?.Invoke(Upgrades[value].Reward);
            }
        }
        
        public UpgradableProduct GetNextProduct()
        {
            return Upgrades[Mathf.Min(CurrentUpgradeIndex + 1, Upgrades.Length - 1)];
        }

        public UpgradableProduct GetProduct()
        {
            return Upgrades[Mathf.Min(CurrentUpgradeIndex, Upgrades.Length - 1)];
        }
        
        public bool CanBuy()
        {
            return CurrentUpgradeIndex != Upgrades.Length - 1 && GetNextProduct().CanBuy();
        }

        public bool TryBuy()
        {
            var isBuy = GetNextProduct().TryBuy();
            if (isBuy)
            {
                CurrentUpgradeIndex++;
            }

            return isBuy;
        }

        public void ClearProgress()
        {
            SaveManager.Save(PrefsKey, 0);
        }
    }
}