using System;
using Core.Meta.Shop.Upgrades;
using NavySpade._PJ71.BuildingSystem;
using NavySpade._PJ71.UI.Common;
using NavySpade.Meta.Runtime.Economic.Prices.DifferentTypes;
using NavySpade.Meta.Runtime.Upgrades;
using TMPro;
using UnityEngine;

namespace NavySpade._PJ71.UI
{
    public class UpgradeButton : ExtButton
    {
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _blockColor;
        
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private GameObject[] _fillStars;

        private BuildingHandler _buildingHandler;
        private UpgradeSO _upgradeSo;
        private int _upgradeIndex;
        private UpgradePanel _buttonsHolder;
        
        public void Init(UpgradePanel panel, BuildingHandler buildingHandler, int upgradeIndex)
        {
            _buildingHandler = buildingHandler;
            _upgradeIndex = upgradeIndex;
            _buttonsHolder = panel;
            
            _upgradeSo = _buildingHandler.GetUpgradeInfo(_upgradeIndex);
            UpdateView();
        }
        
        public void UpdateView()
        {
            int level = _upgradeSo.CurrentUpgradeIndex;
            CanPress = _upgradeSo.CanBuy();
            for (int i = 0; i < _fillStars.Length; i++)
            {
                _fillStars[i].SetActive(i <= level);
            }

            bool maxLevel = level >= _upgradeSo.Upgrades.Length;
            if (maxLevel)
            {
                _costText.text = "MAX";
            }
            else
            {
                UpgradableProduct upgrades = _upgradeSo.GetNextProduct();
                _costText.text = ((CurrencyPrice) upgrades.Price).Count.ToString();
            }
            
            _costText.color = CanPress ? _normalColor : _blockColor;
        }
        
        protected override void OnClicked()
        {
            if (_upgradeSo.TryBuy())
            {
                foreach (var handler in BuildingHandler.Active)
                {
                    handler.UpdateBuildingInfo();
                }

                _buttonsHolder.UpdateButtons();
            }
        }
    }
}
