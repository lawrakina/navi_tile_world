using Core.Actors;
using Core.Damagables;
using Misc.Physic;
using NavySpade._PJ71.Battle;
using NavySpade._PJ71.Battle.EnemyDetection;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade._PJ71.Saving;
using NavySpade._PJ71.Squad;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.Modules.Saving.Runtime.Interfaces;
using NavySpade.Modules.StateMachines.Runtime.Mono;
using NavySpade.PJ70.Weapon;
using Pj_61_Weapon_adv.Core.UnitStates;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade
{
    public class PlayerHandler : ActorHolder, ISaveable, IShootable, ICapturer
    {
        [Header("Common")] 
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private ActorDeathAnimation _actorDeathAnimation;
        
        [Header("Gather")]
        [SerializeField] private PlayerStackVisualizer _stack;

        [Header("Squad")] 
        [SerializeField] private SquadPlayer _squadPlayer;
        // [SerializeField] private SquadDetector _squadDetector;
        [Header("Attack")]
        [SerializeField] private Ragdoll _ragdoll;
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private StateMachineInitialization _stateMachine;
        [SerializeField] private UnityEvent _onReborn;
        [SerializeField] private ShootingBehaviour _shootingBehaviour;

        private const string SaveKey = "PlayerInventory";
        private PlayerSavingData _playerSavingData;
        
        private Vector3 _startingPoint;
        private ICapturable _capturable;

        public Inventory Inventory => _stack.Inventory;

        public Team Team => Damageable.CurrentTeam;
        
        public ShootingActorPreset Data => (ShootingActorPreset) Preset;
        
        public void Init()
        {
            SaveManager.Register(this);
            RestoreState(null);
            
            base.Init(null, Preset, Team.Player);
            _stack.Init(_playerSavingData.Inventory, ItemManagementConfig.Instance.InventorySize);

            _movement.Init(_squadPlayer);
            //_movement.Squad = _squadPlayer;
            
            _movement.Speed = Data.MovementSpeed.Value;
            _movement.AngularSpeed = Data.AngularSpeed;
            _movement.StartMove();
            _enemyDetector.Init(Data.ViewRadius.Value);
            _squadPlayer.Init(_stack);
            
            _stateMachine.Init();
        }

        private void OnDestroy()
        {
            SaveManager.UnRegister(this);
        }

        protected override void DieInternal()
        {
            StopAllCoroutines();
            
            _movement.StopMove();
            _actorDeathAnimation.PlayDeathAnimation();
        }

        public void Reborn() {
            // _shootingBehaviour.Cooldown = true;
            _squadPlayer.Clear();
            _enemyDetector.ClearEnemies();
            
            transform.position = _startingPoint;
            _ragdoll.Reload();
            
            IsDead = false;
            Damageable.Reset();
            _onReborn?.Invoke();
            _movement.StartMove();
        }
        
        public object CaptureState()
        {
            _playerSavingData.Inventory = _stack.Inventory.GetSavingData();
            SaveManager.Save(SaveKey, _playerSavingData);
            return null;
        }

        public void RestoreState(object state)
        {
            _playerSavingData = SaveManager.Load(SaveKey, new PlayerSavingData());
        }

        public void ClearSave()
        {
            SaveManager.DeleteKey(SaveKey);
        }
        
        public void StartCapture(ICapturable capturable)
        {
            if(capturable.CaptureProgress <= 0 && capturable.CurrentTeam == Team)
                return;
            
            _capturable = capturable;
            _capturable.AddToCapture(this);
        }
        
        public void StopCapture(ICapturable capturable)
        {
            if(_capturable == null)
                return;
            
            _capturable.RemoveFromCapture(this);
        }
    }
}