using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Core.Damagables;
using Core.Meta.Shop.Upgrades;
using NaughtyAttributes;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade.Meta.Runtime.Upgrades;
using NavySpade.Meta.Usage.Upgrade.Scripts;
using NavySpade.Modules.Extensions.UnityTypes;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.Modules.Saving.Runtime.Interfaces;
using NavySpade.NavySpade.Modules.Meta.Runtime.Upgrades;
using UnityEditor;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem
{
    [ExecuteInEditMode]
    public abstract class BuildingHandler : ExtendedMonoBehavior<BuildingHandler>, ISaveable
    {
        [SerializeField] private BuildingConfig _config;
        [SerializeField] private BuildingPreset _customPreset;
        
        [Space]
        [SerializeField] private string _saveGuid;
        [SerializeField] private DamageableMono _damageble;
        [SerializeField] private InventoryVisualizer _repairInventory;

        public int Level => Info.Level;
        
        public IBuildingsHolder BuildingsHolder { get; private set; }

        public BuildingConfig Config { get; private set; }
        
        public BuildingInfo Info { get; private set; }
        
        public BuildingPreset Preset { get; private set; }

        public DamageableMono Damageable => _damageble;

        public Team Team => Damageable.CurrentTeam;
        
        public bool IsBroken { get; set; }

        private const string SaveKey = "Building";

        private void Awake()
        {
#if UNITY_EDITOR
            GenerateGuid();
#endif
        }
        
        public virtual void Init(IBuildingsHolder holder)
        {
            Init(holder, _config, _saveGuid);
        }
        
        public virtual void Init(IBuildingsHolder holder, BuildingConfig config, string guid)
        {
            Config = config;
            BuildingsHolder = holder;
            Preset = _customPreset == null ? Instantiate(Config.GetPreset()) : _customPreset;
            _saveGuid = guid;
            
            _repairInventory.Init();
            
            SaveManager.Register(this);
            RestoreState(null);
            
            InitDamageable();
            UpdateBuildingInfo();
            //InitBuildingInfo();
        }
        
        private void OnDestroy()
        {
            _damageble.OnHPChange -= CheckBrokenState;
            SaveManager.UnRegister(this);
        }

        private void InitDamageable()
        {
            float initHP = Info.HP;
            
            //if first load
            if (Info.HP <= 0 && Info.IsDestroyed == 0)
            {
                initHP = Preset.Hp;
            }
            
            _damageble.Init(Preset.Hp, initHP, Team);
            _damageble.OnHPChange += CheckBrokenState;
            CheckBrokenState(_damageble.HP);
        }

        // private void InitBuildingInfo()
        // {
        //     if (Info.UpgradeInfos == null || Info.UpgradeInfos.Count != Config.Upgrades.Length)
        //     {
        //         Info.UpgradeInfos = new List<UpgradeInfo>(Config.Upgrades.Length);
        //         Array.ForEach(Config.Upgrades, (u) => Info.UpgradeInfos.Add(new UpgradeInfo()
        //         {
        //             Level = 1,
        //             Type = u.Type
        //         }));
        //     }
        //
        //     UpdateBuildingInfo();
        // }
        
        public void UpdateBuildingInfo()
        {
            ApplyUpgrades(Preset);
        }

        public void ApplyUpgrades(IUpgradeable upgradeable)
        {
            if(Team != Team.Player)
                return;
            
            if(Config == null)
                return;
            
            var upgradeableParameters = upgradeable.GetParameters();
            foreach (var upgrade in Config.Upgrades)
            {
                //var upgradeInfo =  Info.UpgradeInfos.FirstOrDefault((u) => u.Type == upgrade.Type);
                //var upgradeData = upgrade.Upgrades[upgradeInfo.Level - 1];
                var upgradeData = upgrade.GetProduct();
                foreach (var floatUpgradeableParameter in upgradeableParameters)
                {
                    if(floatUpgradeableParameter.IsUpgradeable == false)
                        continue;
                    
                    if (upgrade.Type == floatUpgradeableParameter.Type)
                    {
                        floatUpgradeableParameter.ValueBase = ((FloatValue) upgradeData.Reward).Value;
                    }
                }
            }
        }
        
        public virtual void SetTeam(Team team)
        {
            Damageable.CurrentTeam = team;
        }

        // public void TryUpgradeBuilding(int upgradeIndex)
        // {
        //     var upgradeInfo = GetUpgradeInfo(upgradeIndex);
        //     
        //     if (upgradeInfo.Item2.Level >= upgradeInfo.Item1.Upgrades.Length)
        //         return;
        //
        //     bool maxLevel = upgradeInfo.Item2.Level >= upgradeInfo.Item1.Upgrades.Length;
        //     if (maxLevel)
        //         return;
        //
        //     UpgradableProduct upgrades = upgradeInfo.Item1.Upgrades[upgradeInfo.Item2.Level - 1];
        //     if (upgrades.CanBuy())
        //     {
        //         upgrades.TryBuy();
        //         upgradeInfo.Item2.Level++;
        //         UpdateBuildingInfo();
        //     }
        // }

        public UpgradeSO GetUpgradeInfo(int upgradeIndex)
        {
            return Config.Upgrades[upgradeIndex];
        }
        
        private void CheckBrokenState(float hpValue)
        {
            float hpPercent = hpValue / _damageble.MaxHp * 100;
            if (hpPercent <= Preset.BreakingPercent)
            {
                IsBroken = true;
            }
        }

        public void RepairBuilding()
        {
            _damageble.Reset();
            IsBroken = false;
        }

        public virtual object CaptureState()
        {
            Info.HP = Damageable.HP;
            Info.IsDestroyed = Damageable.HP == 0 ? 1 : 0;
            Info.RepairInventory = _repairInventory.Inventory.GetSavingData();
            SaveManager.Save(SaveKey + Info.Guid, Info);
            return null;
        }

        public void RestoreState(object state)
        {
            Info = SaveManager.Load(SaveKey + _saveGuid, new BuildingInfo());
            Info.Guid = _saveGuid;
        }

        public void ClearSave()
        {
            SaveManager.DeleteKey(SaveKey + _saveGuid);
        }
        
#if UNITY_EDITOR
        [Button()]
        public void GenerateGuid()
        {
            if (String.IsNullOrEmpty(_saveGuid))
            {
                _saveGuid = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}