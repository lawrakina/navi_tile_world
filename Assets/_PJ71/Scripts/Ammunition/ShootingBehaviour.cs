using System.Collections;
using Core.Damagables;
using Core.Extensions;
using NaughtyAttributes;
using NavySpade._PJ71.Utils.AnimatorUtils;
using NavySpade.PJ70.Core.Ammunition;
using NS.Core.Utils.AnimatorUtils;
using Pool;
using UnityEngine;

namespace NavySpade.PJ70.Weapon
{
    public class ShootingBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform[] _shootPoints;

        [SerializeField] private bool _hasAnimation;

        [SerializeField] [EnableIf(nameof(_hasAnimation))]
        private WeaponAnimatorControl _animatorControl;

        [SerializeField] private bool _hasEffect;

        [SerializeField] [EnableIf(nameof(_hasEffect))]
        private ParticleSystem[] _shootEffects;

        [SerializeField] private bool _recoverAmmo;

        [SerializeField] private int _effectsInRow = 1;

        [SerializeField] [EnableIf(nameof(_recoverAmmo))]
        public AmmoHandler[] _ammoInstances;

        [SerializeField] [EnableIf(nameof(_recoverAmmo))]
        private float _recoverTime;

        private int _currentBulletIndex;
        private bool _shootInProgress;
        private bool _isCooldown;
        // public bool Cooldown {
        //     get => _isCooldown;
        //     set => _isCooldown = value;
        // }

        public void TryShooting(ShootInfo info)
        {
            if (_isCooldown)
                return;

            if (_shootInProgress)
                return;

            StartCoroutine(ShootingWithCooldown(info));
        }

        private IEnumerator ShootingWithCooldown(ShootInfo info)
        {
            _shootInProgress = true;
            yield return StartCoroutine(Shooting(info));
            _shootInProgress = false;
            StartCoroutine(CooldownProcess(info.DelayBetweenShoots));
        }

        private IEnumerator Shooting(ShootInfo info)
        {
            _currentBulletIndex = 0;

            if (_hasAnimation)
            {
                var attackCallback =
                    new AnimActionCallbackData()
                    {
                        AnimEvent = AnimEvent.Attack,
                        EventCallback = () => { SpawnBullet(info); }
                    };

                yield return _animatorControl.PlayShootingAnim(attackCallback);
                _animatorControl.PlayIdle();
            }
            else
            {
                for (int i = 0; i < _shootPoints.Length; i++)
                {
                    SpawnBullet(info);
                    yield return new WaitForSeconds(info.DelayInShootQueue);
                }
            }
        }

        private void SpawnBullet(ShootInfo info)
        {
            if (info.AmmoConfig.IsInstant)
            {
                ShootInstant(info);
                return;
            }

            AmmoHandler ammoHandler = GetAmmo(info.AmmoConfig);
            Vector3 dir = GetShootingDirection(
                info.Target, info.Offset,ammoHandler.transform.position, info.SpreadRange);
            
            ammoHandler.transform.SetParent(null);
            ammoHandler.Fire(dir, info.AmmoConfig, info.Team);

            if (_recoverAmmo)
            {
                StartCoroutine(RecoveringAmmo(info.AmmoConfig, _currentBulletIndex));
            }

            PlayEffects();
            _currentBulletIndex++;
        }

        private void ShootInstant(ShootInfo info)
        {
            if (_currentBulletIndex >= _shootPoints.Length)
                return;

            Transform shootPoint = _shootPoints[_currentBulletIndex];
            Vector3 origin = shootPoint.position;
            Vector3 dir = GetShootingDirection(info.Target, info.Offset, origin, info.SpreadRange);

            int piercedObjects = 0;
            PhysicsUtils.BoxRaycastNonAlloc(
                origin,
                dir,
                new Vector3(5, 0.1f, 0.1f),
                100,
                info.AmmoConfig.LayerMask, SingleDamageDealing);

            void SingleDamageDealing(Collider hitCollider)
            {
                if (piercedObjects >= info.AmmoConfig.PiercedObjects.Value)
                    return;

                if (PhysicsUtils.TryDealSingleDamage(
                        hitCollider,
                        (int) info.AmmoConfig.Damage.Value,
                        info.Team,
                        info.AmmoConfig.LayerMask, null)) ;
                {
                    piercedObjects++;
                }
            }

            PlayEffects();
            _currentBulletIndex++;
        }

        private Vector3 GetShootingDirection(Transform target, Vector3 offset, Vector3 origin, Vector3 spreadRange)
        {
            Vector3 targetPos = target.position + offset;
            targetPos.x += Random.Range(-spreadRange.x, spreadRange.x);
            targetPos.y += Random.Range(-spreadRange.y, spreadRange.y);
            targetPos.z += Random.Range(-spreadRange.z, spreadRange.z);
            return (targetPos - origin).normalized;
        }

        private AmmoHandler GetAmmo(AmmoConfig ammoData)
        {
            if (_currentBulletIndex >= _shootPoints.Length)
                _currentBulletIndex = 0;

            if (_recoverAmmo)
            {
                AmmoHandler ammoHandler = _ammoInstances[_currentBulletIndex];
                _ammoInstances[_currentBulletIndex] = null;
                return ammoHandler;
            }
            else
            {
                Transform spawnPoint = _shootPoints[_currentBulletIndex];
                return GetAmmoInstance(ammoData, spawnPoint);
            }
        }

        private void PlayEffects()
        {
            int startIndex = _currentBulletIndex * _effectsInRow;

            if (_hasEffect)
            {
                for (int i = 0; i < _effectsInRow; i++)
                {
                    _shootEffects[startIndex + i].Play();
                }
            }
        }

        public void PlayIdle()
        {
            if (_animatorControl)
                _animatorControl.PlayIdle();
        }

        private IEnumerator RecoveringAmmo(AmmoConfig ammoData, int index)
        {
            if (_ammoInstances[index] != null)
                yield break;

            yield return new WaitForSeconds(_recoverTime);
            Transform spawnPoint = _shootPoints[index];
            _ammoInstances[index] = GetAmmoInstance(ammoData, spawnPoint);
        }

        private AmmoHandler GetAmmoInstance(AmmoConfig ammoData, Transform spawnPoint)
        {
            AmmoHandler ammoHandler;
            if (ammoData.FromPool)
            {
                ammoHandler = PoolHandler.Get<AmmoHandler>(ammoData.PoolName);
            }
            else
            {
                ammoHandler = Instantiate(ammoData.Prefab);
            }

            ammoHandler.transform.SetParent(spawnPoint);
            ammoHandler.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            return ammoHandler;
        }

        private IEnumerator CooldownProcess(float cooldownTime)
        {
            _isCooldown = true;
            yield return new WaitForSeconds(cooldownTime);
            _isCooldown = false;
        }
    }
    
    public struct ShootInfo
    {
        public Transform Target;
        public Vector3 Offset;
        public AmmoConfig AmmoConfig;
        public Team Team;
        public Vector3 SpreadRange;
        public float DelayInShootQueue;
        public float DelayBetweenShoots;
    }
}