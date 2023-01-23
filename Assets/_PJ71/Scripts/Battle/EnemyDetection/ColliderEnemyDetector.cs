using System;
using Core.Damagables;
using UnityEngine;

namespace NavySpade._PJ71.Battle.EnemyDetection
{
    [RequireComponent(typeof(SphereCollider))]
    public class ColliderEnemyDetector : EnemyDetector
    {
        public override void Init(float viewRadiusValue)
        {
            GetComponent<SphereCollider>().radius = viewRadiusValue;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (TryAddEnemy(other, out DamageableMono damageable))
            {
                SubscribeOnChange(damageable);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (TryRemoveEnemy(other, out DamageableMono damageableMono))
            {
                UnsubscribeOnChange(damageableMono);
            }
        }

        private void OnDisable()
        {
            foreach (var enemy in Enemies)
            {
                UnsubscribeOnChange(enemy);
            }
            
            ClearEnemies();
        }
        
        protected override void Remove(DamageableMono damageable)
        {
            UnsubscribeOnChange(damageable);
            base.Remove(damageable);
        }
        
        private void CheckDamageable(DamageableMono damageable)
        {
            if (TryRemove(damageable))
            {
                UnsubscribeOnChange(damageable);
            }
        }

        private void SubscribeOnChange(DamageableMono damageable)
        {
            damageable.OnDeath += Remove;
            damageable.DamageableSetupChanged += CheckDamageable;
        }

        private void UnsubscribeOnChange(DamageableMono damageable)
        {
            damageable.OnDeath -= Remove;
            damageable.DamageableSetupChanged -= CheckDamageable;
        }
        
       
    }
}