using System;
using System.Collections.Generic;
using Core.Damagables;
using UnityEngine;

namespace NavySpade._PJ71.Battle.EnemyDetection
{
    public abstract class EnemyDetector : MonoBehaviour, IEnemyDetector
    {
        [SerializeField] private DamageableMono _ownDamageable;
        [SerializeField] private int _maxEnemies = 10;

        private List<DamageableMono> _enemies = new List<DamageableMono>();

        public bool HasTargets => _enemies.Count > 0;

        public IEnumerable<DamageableMono> Enemies => _enemies;

        private void Awake()
        {
            _enemies = new List<DamageableMono>(_maxEnemies);
        }

        private void Start()
        {
            _ownDamageable.TeamChanged += OnTeamChanged;
        }

        private void OnDestroy()
        {
            _ownDamageable.TeamChanged -= OnTeamChanged;
        }

        public abstract void Init(float viewRadiusValue);

        
        private void OnTeamChanged(Team obj)
        {
            ClearEnemies();
        }
        
        public void ClearEnemies()
        {
            _enemies.Clear();
        }
        
        protected bool TryRemoveEnemy(Collider col, out DamageableMono damageable)
        {
            Rigidbody rb = col.attachedRigidbody;
            damageable = null;
            
            if(rb == null)
                return false;
            
            if(rb.TryGetComponent(out damageable) == false)
                return false;
            
            Remove(damageable);
            return true;
        }

        protected bool TryRemove(DamageableMono damageable)
        {
            if (damageable.CanDealDamage(_ownDamageable.CurrentTeam) == false)
            {
                _enemies.Remove(damageable);
                return true;
            }
            
            return false;
        }

        protected virtual void Remove(DamageableMono damageable)
        {
            _enemies.Remove(damageable);
        }

        public bool TryAddEnemy(Collider col, out DamageableMono result)
        {
            result = null;
            Rigidbody rb = col.attachedRigidbody;
            if(rb == null)
                return false;
                
            if(rb.transform == transform)
                return false;

            if(rb.TryGetComponent(out DamageableMono damageble) == false)
                return false;
                
            if(damageble.CanDealDamage(_ownDamageable.CurrentTeam) == false)
                return false;

            result = damageble;
            _enemies.Add(damageble);
            return true;
        }
    }
}