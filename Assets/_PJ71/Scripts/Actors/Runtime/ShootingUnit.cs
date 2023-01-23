using System;
using System.Threading.Tasks;
using Core.Actors;
using Core.Damagables;
using Core.Input.Commands;
using Core.Movement;
using NavySpade._PJ71.Battle;
using NavySpade._PJ71.Battle.EnemyDetection;
using NavySpade._PJ71.Utils;
using NavySpade.Core.Runtime.Game;
using NavySpade.Modules.StateMachines.Runtime.Mono;
using NS.Core.Actors;
using Pj_61_Weapon_adv.Core.UnitStates;
using UnityEngine;
using UnityEngine.Events;


namespace NavySpade._PJ71.Scripts.Actors.Runtime {
    public class ShootingUnit : ActorHolder, IShootable, ICapturer {
        [Serializable] public struct Events {
            public UnityEvent OnDisable;
            public UnityEvent OnEnable;
        }

        [SerializeField] private UnitMovement _movement;
        [SerializeField] private ActorDeathAnimation _actorDeathAnimation;
        [SerializeField] private StateMachineInitialization _stateMachine;
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private Events _events;

        private ICapturable _capturable;
        private int _captureIndex;

        private bool IsMovingToFlag { get; set; }

        public ShootingActorPreset Data => (ShootingActorPreset) base.Preset;

        public UnitMovement Movement => _movement;

        public Team Team => Damageable.CurrentTeam;

        public bool CanAttack { get; set; }

        public override void Init(IActorHandler handler, ActorPreset actorPreset, Team team) {
            base.Init(handler, actorPreset, team);

            CanAttack = true;
            IsMovingToFlag = false;
            _capturable = null;

            _movement.StartMove();
            _movement.Speed = Data.MovementSpeed.Value;
            _movement.AngularSpeed = Data.AngularSpeed;
            _enemyDetector.Init(Data.ViewRadius.Value);

            _stateMachine.Init();
        }

        private void OnEnable() {
            _events.OnEnable.Invoke();
        }

        private void OnDisable() {
            _events.OnDisable.Invoke();
        }

        protected override void DieInternal() {
            StopAllCoroutines();

            _movement.StopMove();
            HandlerRemoveOn();
            _actorDeathAnimation.PlayDeathAnimation();
            StopCapture(_capturable);

            GameContext.Instance.GoldSpawner.DropGold(transform.position);
        }

        public void HandlerRemoveOn() {
            Handler.EnemyDestroyed(this);
        }

        public void StartCapture(ICapturable capturable) {
            if (IsMovingToFlag == false)
                return;

            // if(capturable.CaptureProgress <= 0 && capturable.CurrentTeam == Team)
            //     return;

            _capturable = capturable;
            _captureIndex = _capturable.AddToCapture(this);
            Movement.MoveTo(new MoveCommand(_capturable.GetPositionForCapture(_captureIndex)), true);
        }

        public void StopCapture(ICapturable capturable) {
            if (_capturable == null)
                return;

            _capturable.RemoveFromCapture(this);
        }

        public void MoveToFlag(Flag flag) {
            if (_capturable == null)
            {
                Movement.MoveTo(new MoveCommand(flag.transform.position));
            } else
            {
                Movement.MoveTo(new MoveCommand(flag.GetPositionForCapture(_captureIndex)), true);
            }

            IsMovingToFlag = true;
        }

        private async void Awake() {
            await Task.Delay(2000);
            var hits = Physics.OverlapSphere(transform.position, 5);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Flag flag))
                {
                    if (flag.TryGetComponent(out TeamVisualSwitcher otherTeam))
                        if (otherTeam.Team != GetComponent<TeamVisualSwitcher>().Team)
                        {
                            IsMovingToFlag = true;
                            StartCapture(flag);
                        }
                }
            }
        }
    }
}