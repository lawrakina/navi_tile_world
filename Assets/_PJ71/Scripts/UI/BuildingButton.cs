using NavySpade._PJ71.BuildingSystem;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NavySpade._PJ71.UI
{
    public class BuildingButton : ExtButton
    {
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _blockColor;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _icon;


        private ChooseBuildingView _holder;
        private Inventory _inventory;
        private ResourceRequirements _requirements;

        public BuildingConfig BuildingConfig { get; private set; }


        public void Init(ChooseBuildingView holder, BuildingConfig config, Inventory inventory)
        {
            BuildingConfig = config;
            _holder = holder;
            _inventory = inventory;
            _requirements = BuildingConfig.GetPreset().BuildRequirements;

            UpdateView();
        }

        public void UpdateView()
        {
            if(BuildingConfig == null)
                return;
            
            _nameText.text = BuildingConfig.Name;
            _icon.sprite = BuildingConfig.Icon;
            _costText.text = _requirements.Value.ToString();

            ItemInfo itemInfo = _inventory.GetItemInfo(_requirements.Preset.Type);
            CanPress = itemInfo.Amount >= _requirements.Value;

            _costText.color = CanPress ? _normalColor : _blockColor;
        }

        protected override void OnClicked()
        {
            if (CanPress)
            {
                _inventory.Reduce(_requirements.Preset.Type, _requirements.Value);
                _holder.OnClicked(this);
            }
        }
    }
}