using System;
using System.Collections;
using AYellowpaper;
using Core.Actors;
using Core.Damagables;
using NavySpade._PJ71.Battle;
using NavySpade._PJ71.Battle.EnemyDetection;
using NavySpade._PJ71.Scripts.Actors.Runtime;
using NavySpade._PJ71.Utils;
using NavySpade._PJ71.Utils.AnimatorUtils;
using NavySpade.Modules.Extensions.UnityTypes;
using NavySpade.Modules.StateMachines.Runtime.Mono;
using NavySpade.PJ70.Weapon;
using UnityEngine;

namespace NavySpade._PJ71.Actors.States
{
    public class AttackState : StateBehavior
    {
        [SerializeField] private InterfaceReference<IShootable> _shootable;
        [SerializeField] private ShootingBehaviour _shootingBehaviour;
        [SerializeField] private EnemyDetector _fovEnemyDetector;
        [SerializeField] private UnitAnimatorController _animator;
        [SerializeField] private SyncTransforms _shootPoint;
        
        [SerializeField] private bool _lookAtTarget;
        [SerializeField] private Transform _rotatePart;

        [SerializeField] private ParticleSystem _shoot;

        private Coroutine _attackCoroutine;

        public DamageableMono Target { get; set; }
        
        public override void Enter()
        {
            base.Enter();
            
            _animator.SetMoveBool(false);
            _animator.AttackRig.weight = 1;

            if (_attackCoroutine == null)
            {
               _attackCoroutine = StartCoroutine(Attacking());
            }
        }

        public override void Exit()
        {
            base.Exit();

            _animator.SetMoveBool(true);
            _animator.AttackRig.weight = 0;

            StopAllCoroutines();
            _attackCoroutine = null;
        }

        private void Update()
        {
            if (_lookAtTarget)
            {
                LookAtTarget();
            }
        }
        
        private void LookAtTarget()
        {
            Vector3 dir = Target.CenterBody.position - _rotatePart.position;
            dir.y = 0;
            
            _rotatePart.rotation = Quaternion.RotateTowards(
                _rotatePart.rotation,
                Quaternion.LookRotation(dir),
                _shootable.Value.Data.AngularSpeed * Time.deltaTime);
        }
        
        private IEnumerator Attacking()
        {
            while (true)
            {
                if (TrySetTarget() == false)
                {
                    yield return null;
                }
                else
                {
                    AnimActionCallbackData attackCallback = new AnimActionCallbackData()
                    {
                        AnimEvent = AnimEvent.Attack,
                        EventCallback = Shoot
                    };

                    yield return _animator.PlayAndWait(UnitAnimatorController.AnimType.Attack, 1, 0, attackCallback);
                    if (_shoot)
                    {
                        // _shoot.gameObject.SetActive(true);
                        _shoot.Play();
                    }
                    
                    
                    yield return new WaitForSeconds(_shootable.Value.Data.DelayBetweenShoots.Value);
                    if(_shoot)
                        _shoot.Stop();
                    // _shoot.gameObject.SetActive(false);
                }
            }
        }

        private void Shoot()
        {
            if(gameObject.activeInHierarchy)
                _shootingBehaviour.TryShooting(SetupShootInfo());
        }
        
        private bool TrySetTarget()
        {
            Target = _fovEnemyDetector.Enemies.FindClosed(transform.position);
            if (Target == null)
                return false;
            
            _shootPoint.SyncWith = Target.CenterBody;
            return true;
        }
        
        private ShootInfo SetupShootInfo()
        {
            return new ShootInfo()
            {
                Target = Target.transform,
                Offset = Target.CenterBody.position - Target.transform.position,
                AmmoConfig = _shootable.Value.Data.AmmoConfig,
                Team = _shootable.Value.Damageable.CurrentTeam,
                DelayBetweenShoots = _shootable.Value.Data.DelayBetweenShoots.Value,
                DelayInShootQueue = 0,
                SpreadRange = _shootable.Value.Data.FireSpread
            };
        }
    }
}