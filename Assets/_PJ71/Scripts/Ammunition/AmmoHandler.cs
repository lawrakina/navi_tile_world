using System.Collections;
using Core.Damagables;
using Core.Extensions;
using Core.Extensions.UnityTypes;
using Core.Movement;
using Misc.Damagables.Effects;
using NavySpade._PJ71.Battle;
using NS.Core.Utils;
using NS.Core.Utils.Pool;
using Pool;
using UnityEngine;

namespace NavySpade.PJ70.Core.Ammunition
{
    public class AmmoHandler : MonoBehaviour
    {
        [SerializeField] private DirectMovementBehaviour _movement;
        
        private AmmoConfig _ammoConfig;
        private Team _team;
        private int _damagedCollidersCount;
        private BattleFieldConfig _config;
        
        private void Awake()
        {
            _movement.StopMove();
            _config = BattleFieldConfig.Instance;
        }
        
        public void Fire(Vector3 dir, AmmoConfig config, Team team)
        {
            transform.rotation = Quaternion.LookRotation(dir);
            _movement.enabled = true;
            _movement.SetDirection(dir);
            _movement.StartMove();
            _movement.Speed = config.MovementSpeed.Value;
            _ammoConfig = config;
            _team = team;
            
            StartCoroutine(WaitForDisable());
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_ammoConfig == null)
                return;
            
            if(LayerUtils.CheckForComparerLayer(_ammoConfig.LayerMask, other.gameObject) == false)
                return;
            
            CheckCollision(other);
        }

        private void CheckCollision(Collider other)
        {
            if (TryDealDamage(other))
            {
                if (_ammoConfig.HasHitEffect)
                {
                    Transform effectTr;
                    if (_ammoConfig.EffectFromPool)
                    {
                        var hitEntry = PoolHandler.Get<PoolObject>(_ammoConfig.EffectPoolName);
                        hitEntry.InPoolOnDisable = true;
                        effectTr = hitEntry.transform;
                    }
                    else
                    {
                        effectTr = Instantiate(_ammoConfig.HitEffect).transform;
                    }
                
                    effectTr.parent = null;
                    effectTr.position = transform.position;
                }
                
                StopAllCoroutines();
                DoDestroy();
            }
        }

        private bool TryDealDamage(Collider other)
        {
            HitData hitData = new HitData(transform.position, transform.backward());
            
            if (_ammoConfig.AOERadius.Value > 0)
            {
                return PhysicsUtils.TryDealOverlapAOEDamage(
                    transform.position,
                    _ammoConfig.AOERadius.Value,
                    (int) _ammoConfig.Damage.Value,
                    _team,
                    _ammoConfig.LayerMask,
                    hitData);
            }
            else
            {
                return PhysicsUtils.TryDealSingleDamage(
                    other,
                    (int) _ammoConfig.Damage.Value,
                    _team,
                    _ammoConfig.LayerMask,
                    hitData);
            }
        }
        
        private IEnumerator WaitForDisable()
        {
            yield return new WaitForSeconds(_config.AmmoDestroyTime);
            DoDestroy();
        }
        
        private void DoDestroy()
        {
            if (_ammoConfig.FromPool)
            {
                this.ReturnToThePool();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}