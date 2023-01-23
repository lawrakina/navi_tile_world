using Core.Damagables;
using NavySpade._PJ71.UI.Common.ProgressBar;
using NavySpade.Modules.StateMachines.Runtime.Mono;
using NavySpade.NavySpade.Modules.Utils.Timers;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime.States
{
    public class ProductionState : StateBehavior
    {
        [SerializeField] private ProductionBuilding _productionBuilding;
        [SerializeField] private ScaleProgressBar _productionProgress;
        
        private Timer _productionTimer;
        private float _currentProgress;
        private bool _showProgress;
        
        public override void Enter()
        {
            base.Enter();
            _showProgress = _productionBuilding.Damageable.CurrentTeam == Team.Player;
        }

        public override void Exit()
        {
            base.Exit();
            if (_productionBuilding.InProduction)
            {
                _productionBuilding.ReturnResourceForProduction();
            }
            _productionBuilding.InProduction = false;
        }

        private void Update()
        {
            if (_productionBuilding.InProduction == false)
            {
                if(_productionBuilding.CanProduce() == false)
                    return;
                
                StartProduction();
            }
            
            _productionTimer.Update(Time.deltaTime);
            float productionTime = _productionTimer.initTime - _productionTimer.currentTime;
            
            if(_showProgress)
                _productionProgress.SetupProgressbar(_productionTimer.initTime,  productionTime);
            
            if (_productionTimer.IsFinish())
            {
                CreateItem();
            }
        }

        private void StartProduction()
        {
            _productionBuilding.TakeResourceForProduction();
            
            if (_productionTimer == null)
            {
                _productionTimer = new Timer(_productionBuilding.ProductionPreset.ConversionSpeed.Value);
            }

            _productionTimer.initTime = _productionBuilding.ProductionPreset.ConversionSpeed.Value;
            _productionTimer.Reload();
            _productionBuilding.InProduction = true;
        }

        private void CreateItem()
        {
            _productionBuilding.CreateItem();
            _productionBuilding.InProduction = false;
        }
    }
}