using System.Linq;
using Core.Actors;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using UnityEngine;


namespace NavySpade._PJ71.BuildingSystem.Runtime {
    public class TankFactory : ProductionBuilding
    {
        [SerializeField] private Transform _spawnPoint;

        private UnitInventoryVisualizer _unitInventoryVisualizer;
        protected override ResourceType ProductionType { get; set; } = ResourceType.Tank;

        public BarrackPreset BarrackPreset { get; private set; }

        protected override void InitInner()
        {
            BarrackPreset = (BarrackPreset) Preset;
            OutputInventory = BuildingsHolder.TankInventoryVisualizer;
            _unitInventoryVisualizer = (UnitInventoryVisualizer) OutputInventory;
            ClearNonTargetOutputResources(Info);
            _unitInventoryVisualizer.InitSavingData(Info.OutputInventory, CreateUnit);
        }

        public override bool CanProduce()
        {
            if (OutputInventory == null)
                return false;
            
            return _unitInventoryVisualizer.HasFreeSpace;
        }

        public override void TakeResourceForProduction()
        {
            _unitInventoryVisualizer.AddUnitToProduction();
        }

        public override void CreateItem()
        {
            ItemObject itemObject = CreateUnit();
            
            OutputInventory.AddItem(itemObject);
            _unitInventoryVisualizer.RemoveUnitFromProduction();
        }

        private ItemObject CreateUnit()
        {
            ItemObject itemObject = BarrackPreset.OutputResource
                                                 .CreateObject(_spawnPoint.position, Quaternion.identity, transform);

            ActorHolder actorHolder = ((UnitItem) itemObject).ActorHolder;
            ShootingActorPreset shootingPreset = (ShootingActorPreset) BarrackPreset.ActorPreset;
            
            ShootingActorPreset copyPreset = Instantiate(shootingPreset);
            ApplyUpgrades(copyPreset);
            
            var ammoConfig = Instantiate(copyPreset.AmmoConfig);
            copyPreset.AmmoConfig = ammoConfig;
            ApplyUpgrades(ammoConfig);
            
            actorHolder.Init(_unitInventoryVisualizer, copyPreset, Team);
            return itemObject;
        }
        
        public override void ReturnResourceForProduction()
        {
            _unitInventoryVisualizer.RemoveUnitFromProduction();
        }

        public override object CaptureState()
        {
            Info.OutputInventory = OutputInventory.Inventory.GetSavingData();
            return base.CaptureState();
        }
    }
}