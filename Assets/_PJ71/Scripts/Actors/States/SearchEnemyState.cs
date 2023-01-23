using Core.Damagables;
using Core.Extensions;
using NavySpade._PJ71.Battle.EnemyDetection;
using NavySpade._PJ71.Utils;
using NavySpade.Modules.StateMachines.Runtime.Mono;
using NavySpade.NavySpade.Modules.Utils.Timers;
using UnityEngine;

namespace NavySpade._PJ71.Actors.States
{
    public class SearchEnemyState : StateBehavior
    {
        [SerializeField] private FieldOfView _fieldOfView;
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private Transform _rotatedObj;
        [SerializeField] private float _checkTime;

        private Timer _timer;
        
        public override void Enter()
        {
            base.Enter();
            if (_timer == null)
            {
                _timer = new Timer(_checkTime);
            }
        }

        private void Update()
        {
            _timer.Update(Time.deltaTime);
            if (_timer.IsFinish())
            {
                SearchEnemyAround();
                _timer.Reload();
            }
        }

        private void SearchEnemyAround()
        {
            PhysicsUtils.OverlapSphereCastNonAlloc(
                transform.position, 
                _fieldOfView.ViewRadius, 
                _fieldOfView.TargetMask, CheckEnemy);
        }

        private void CheckEnemy(Collider col)
        {
            if (_enemyDetector.TryAddEnemy(col, out DamageableMono _))
            {
                Vector3 dir = col.transform.position - _rotatedObj.position;
                dir.y = 0;
                _rotatedObj.rotation = Quaternion.LookRotation(dir);
            }
        }
    }
}