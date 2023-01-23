using Core.Actors;
using Core.Damagables;
using NavySpade._PJ71.Battle;
using NavySpade._PJ71.Battle.EnemyDetection;
using NavySpade.Modules.StateMachines.Runtime.Mono;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime
{
    public class Tower : BuildingHandler, IShootable
    {
        [SerializeField] private StateMachineInitialization _stateMachineInitialization;
        [SerializeField] private EnemyDetector _enemyDetector;

        public ShootingActorPreset Data => ((AttackingBuildingPreset) Preset).ActorPreset;
        
        public override void Init(IBuildingsHolder holder)
        {
            base.Init(holder);
            _enemyDetector.Init(Data.ViewRadius.Value);
            _stateMachineInitialization.Init();
        }

        public override void Init(IBuildingsHolder holder, BuildingConfig config, string guid)
        {
            base.Init(holder, config, guid);
            _enemyDetector.Init(Data.ViewRadius.Value);
            _stateMachineInitialization.Init();
        }
    }
}