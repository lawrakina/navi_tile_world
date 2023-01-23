using Core.Damagables;
using UnityEngine;

namespace Utils.TriggerAction.Triggers.Physical.ColliderTrigger
{
    public class EnemyTrigger : TriggerBase<DamageableMono>
    {
        protected override bool TryGetComponent(Collider other, out DamageableMono component)
        {
            component = null;
            Rigidbody rb = other.attachedRigidbody;
            
            if (rb == null)
                return false;

            return rb.TryGetComponent(out component);
        }
    }
}