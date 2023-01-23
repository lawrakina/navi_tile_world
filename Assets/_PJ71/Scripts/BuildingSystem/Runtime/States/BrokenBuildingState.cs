using Core.Damagables;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade.Modules.StateMachines.Runtime.Mono;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime.States
{
    public class BrokenBuildingState : StateBehavior
    {
        [SerializeField] private RepairBuildingTrigger _repairTrigger;
        [SerializeField] private BuildingHandler _building;

        public override void Enter()
        {
            base.Enter();
            _repairTrigger.Init(_building.Preset.RepairCost);
            _repairTrigger.RepairFinished += FinishRepair;
        }
        
        public override void Exit()
        {
            base.Exit();
            _repairTrigger.RepairFinished -= FinishRepair;
        }
        
        private void FinishRepair()
        {
            _building.RepairBuilding();
        }
    }
}