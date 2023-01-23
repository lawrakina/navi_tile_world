using System.Linq;
using Core.Actors;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade.Modules.StateMachines.Runtime.Mono;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime
{
    public abstract class ProductionBuilding : BuildingHandler
    {
        [SerializeField] private InventoryVisualizer _outputInventory;
        [SerializeField] private StateMachineInitialization _stateMachine;

        protected abstract ResourceType ProductionType { get; set; }

        public InventoryVisualizer OutputInventory
        {
            get => _outputInventory;
            protected set => _outputInventory = value;
        }
        
        public ProductionBuildingPreset ProductionPreset { get; private set; }
        
        public bool InProduction { get; set; }
        
        public override void Init(IBuildingsHolder holder, BuildingConfig config, string guid)
        {
            base.Init(holder, config, guid);
            ProductionPreset = (ProductionBuildingPreset) Preset;
            InitInner();
            
            _stateMachine.Init();
        }

        protected void ClearNonTargetOutputResources(BuildingInfo info) {
            if(ProductionType == ResourceType.Any || ProductionType == ResourceType.None) return;
            var result = info.OutputInventory.Where(x => x.Type == ProductionType);
            info.OutputInventory = result.ToArray();
        }

        public abstract void TakeResourceForProduction();

        protected abstract void InitInner();

        public abstract bool CanProduce();

        public abstract void CreateItem();

        public abstract void ReturnResourceForProduction();
    }
}