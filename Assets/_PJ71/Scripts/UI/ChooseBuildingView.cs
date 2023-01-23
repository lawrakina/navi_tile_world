using System;
using System.Linq;
using Core.Meta;
using NavySpade._PJ71.BuildingSystem;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.Meta;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade._PJ71.UI
{
    public class ChooseBuildingView : MonoBehaviour
    {
        [SerializeField] private BuildingButton[] _buildingButtons;
        [SerializeField] private GameObject _container;
        [SerializeField] private UnityEvent _onOpened;
        [SerializeField] private UnityEvent _onClosed;
        
        private BuildingPlace _buildingPlace;
        private bool _isShowing;
        private Inventory _inventory;
        
        private void Start()
        {
            _container.SetActive(false);
        }

        public void Init(Inventory inventory)
        {
            _inventory = inventory;
        }
        
        public void Show(BuildingPlace buildingPlace)
        {
            _buildingPlace = buildingPlace;
            for (int i = 0; i < _buildingButtons.Length; i++)
            {
                if (i < buildingPlace.ConfigsToBuild.Length && IsUnlocked(buildingPlace.ConfigsToBuild[i]))
                {
                    _buildingButtons[i].Init(this, buildingPlace.ConfigsToBuild[i], _inventory);
                    _buildingButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    _buildingButtons[i].gameObject.SetActive(false);
                }
            }

            _inventory.ResourcesCountChanged += UpdateView;
            
            _onOpened?.Invoke();
            _isShowing = true;
        }
        
        private bool IsUnlocked(BuildingConfig buildingConfig)
        {
            if (MetaGameConfig.Instance.EnableMetaGame == false)
                return true;
            
            var shopItems = MetaGameConfig.Instance.Rewards;
            foreach (var shopItem in shopItems)
            {
                ItemReward itemReward = shopItem.Product.Reward as ItemReward;
                BuildingConfig bc = (BuildingConfig) itemReward.Data.Value;
                if (bc == buildingConfig)
                {
                    return shopItem.IsUnlocked();
                }
            }

            return true;
        }

        public void Hide()
        {
            if(_isShowing == false)
                return;
            
            _inventory.ResourcesCountChanged -= UpdateView;
            _onClosed?.Invoke();
        }
        
        private void UpdateView(ItemInfo obj)
        {
            Array.ForEach(_buildingButtons, (b) => b.UpdateView());
        }


        public void OnClicked(BuildingButton buildingButton)
        {
            _buildingPlace.ForceBuild(buildingButton.BuildingConfig);
        }
    }
}