using System;
using System.Collections.Generic;
using Core.Damagables;
using NavySpade._PJ71.BuildingSystem.Runtime;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade._PJ71.Saving;
using NavySpade._PJ71.UI;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.Modules.Saving.Runtime.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using ItemInfo = NavySpade._PJ71.InventorySystem.Items.ItemInfo;

namespace NavySpade._PJ71.BuildingSystem
{
    public class BuildingPlace : ItemReceiver, IRequirementsHolder, ISaveable
    {
        [Serializable]
        private struct Events
        {
            public UnityEvent OnCompleteBuild;
            public UnityEvent OnBuildingChoosed;
        }
        
        [SerializeField] private BuildingConfig[] _configToBuild;
        [SerializeField] private Events _events;
        [SerializeField] protected BuildingInventoryVisualizer _inventory;
        [SerializeField] private RequirementsView _requirementsView;

        public BuildingHandler Building { get; private set; }
        
        private IBuildingsHolder _buildingsHolder;
        protected BuildingConfig _choosingBuilding;
        private ResourceRequirements _resourceRequirements;
        private bool _resourcesCollected;
        
        private IInventoryHandler _enteredInventory;
        private readonly List<ItemObject> _itemsInAnimation = new List<ItemObject>();

        public BuildingConfig[] ConfigsToBuild => _configToBuild;

        public ResourceRequirements Requirement => _resourceRequirements;

        public int LeftResources => _resourceRequirements.Value -
                                    (_inventory.GetItemInfo(_resourceRequirements.Preset.Type).Amount - 
                                     _itemsInAnimation.Count);
        
        
        private const string SaveKey = "BuildingPlace";
        private BuildingPlaceSavingData _savingData;
        
        public event Action ProgressUpdated;
        
        public virtual void Init(IBuildingsHolder holder)
        {
            SaveManager.Register(this);
            RestoreState(null);
            
            _buildingsHolder = holder;
            _inventory.Init(_savingData.Inventory);
            _inventory.AddingComplete += UpdateProgress;

            _choosingBuilding = BuildingsConfig.Instance.GetBuildingConfig(_savingData.BuildingType);
            if (_choosingBuilding != null)
            {
                BuildBuilding(_choosingBuilding, 1);    
            }
        }

        // public virtual void Init()
        // {
        //     _buildingsHolder = null;
        //     _inventory.Init();
        //     _inventory.AddingComplete += UpdateProgress;
        // }

        private void OnDestroy()
        {
            _inventory.AddingComplete -= UpdateProgress;
            SaveManager.UnRegister(this);
        }

        public void SetBuilding(BuildingConfig config)
        {
            _choosingBuilding = config;
            _resourceRequirements = config.GetPreset().BuildRequirements;
            _requirementsView.Init(this);
            _events.OnBuildingChoosed?.Invoke();

            if (_enteredInventory != null)
            {
                StartPickupItems(_enteredInventory);
            }
            
            Gui.Instance.ChooseBuildingView.Hide();
            
            CheckBuildStatus();
            TryBuild();
        }

        public void ForceBuild(BuildingConfig config)
        {
            _choosingBuilding = config;
            _events.OnBuildingChoosed?.Invoke();
            Gui.Instance.ChooseBuildingView.Hide();
            BuildBuilding(config, 1);
        }

        protected override void OnEnter(IInventoryHandler inventory)
        {
            if (Building != null)
                return;

            if (_choosingBuilding)
            {
                StartPickupItems(inventory);
                return;
            }

            _enteredInventory = inventory;
            Gui.Instance.ChooseBuildingView.Show(this);
        }

        protected override void OnExit(IInventoryHandler inventory)
        {
            _enteredInventory = null;
            Gui.Instance.ChooseBuildingView.Hide();
        }

        protected override void PickupItemsFrom(IInventoryHandler playerInventory)
        {
            if (_resourcesCollected)
                return;

            ItemInfo itemInfo = playerInventory.GetItemInfo(_resourceRequirements.Preset.Type);
            if (itemInfo.Amount <= 0)
                return;
            
            if (itemInfo.Preset.ShowVisual)
            {
                ItemObject itemObject = playerInventory.PullItem(_resourceRequirements.Preset.Type);
                _itemsInAnimation.Add(itemObject);
                _inventory.AddItem(itemObject);
            }
            else
            {
                playerInventory.TakeResource(_resourceRequirements.Preset.Type);
                _inventory.Inventory.AddItem(_resourceRequirements.Preset.Type);
            }
            
            CheckBuildStatus();
        }

        private void CheckBuildStatus()
        {
            if (_inventory.GetItemInfo(_resourceRequirements.Preset.Type).Amount >= _resourceRequirements.Value)
            {
                _resourcesCollected = true;
            }
        }

        private void UpdateProgress(ItemObject item)
        {
            _itemsInAnimation.Remove(item);
            ProgressUpdated?.Invoke();
            
            TryBuild();
        }

        private void TryBuild()
        {
            if (_resourcesCollected && _itemsInAnimation.Count <= 0)
            {
                BuildBuilding(_choosingBuilding, 1);
            }
        }

        protected virtual void BuildBuilding(BuildingConfig config, int level)
        {
            Building = Instantiate(config.GetPreset(level).Prefab, transform);
            Building.Init(_buildingsHolder, config, _savingData.BuildingGuid);
            Building.SetTeam(Team.Player);
            _events.OnCompleteBuild?.Invoke();
        }
        
        public object CaptureState()
        {
            _savingData.Inventory = _inventory.Inventory.GetSavingData();
            if (_choosingBuilding)
            {
                _savingData.BuildingType = _choosingBuilding.BuildingType;
            }
            else
            {
                _savingData.BuildingType = BuildingType.None;
            }

            if (Building != null)
            {
                _savingData.BuildingGuid = Building.Info.Guid;
            }

            SaveManager.Save(SaveKey + transform.GetSiblingIndex(), _savingData);
            return null;
        }

        public void RestoreState(object state)
        {
            _savingData = SaveManager.Load(SaveKey + transform.GetSiblingIndex(), new BuildingPlaceSavingData());
            if (_savingData.Inventory == null)
            {
                _savingData.Inventory = Array.Empty<ItemSavingInfo>();
            }
        }

        public void ClearSave()
        {
            SaveManager.DeleteKey(SaveKey + transform.GetSiblingIndex());
        }
    }
}