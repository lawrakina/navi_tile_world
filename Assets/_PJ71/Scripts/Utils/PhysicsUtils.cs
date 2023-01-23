using System;
using Core.Damagables;
using Misc.Damagables.Effects;
using NavySpade.pj46.Extensions;
using NS.Core.Utils;
using UnityEngine;

namespace Core.Extensions
{
    public static class PhysicsUtils
    {
        public static void RaycastNonAlloc(
            Vector3 startPoint, 
            Vector3 dir, 
            float maxDistance, 
            LayerMask layerMask, 
            Action<Collider> action)
        {
            Ray ray = new Ray(startPoint, dir);
            var hits = PhysicCacheBinder.Read();
            int hitCount = Physics.RaycastNonAlloc(ray, hits, maxDistance, layerMask);
            for (int i = 0; i < hitCount; i++)
            {
                action?.Invoke(hits[i].collider);
            }
        }

        public static void BoxRaycastNonAlloc(
            Vector3 startPoint, 
            Vector3 dir, 
            Vector3 extends,
            float maxDistance, 
            LayerMask layerMask, 
            Action<Collider> action)
        {
            var hits = PhysicCacheBinder.Read();
            int hitCount = Physics.BoxCastNonAlloc(
                startPoint, 
                extends / 2, 
                dir,
                hits,
                Quaternion.identity, 
                maxDistance, 
                layerMask);
            
            // ExtDebug.DrawBoxCastBox(
            //     startPoint, 
            //     extends / 2, 
            //     Quaternion.identity,
            //     dir, 
            //     maxDistance, 
            //     Color.red,
            //     1);
            
            //Debug.Log("HitCount: " + hitCount);
            for (int i = 0; i < hitCount; i++)
            {
                action?.Invoke(hits[i].collider);
            }
        }
        
        public static void SphereCastNonAlloc(
            Vector3 startPoint, 
            float radius, 
            Vector3 dir,
            float maxDistance, 
            LayerMask layerMask, 
            Action<Collider> action)
        {
            var hits = PhysicCacheBinder.Read();
            int hitCount = Physics.SphereCastNonAlloc(
                startPoint, 
                radius, 
                dir,
                hits,
                maxDistance, 
                layerMask);
            
            for (int i = 0; i < hitCount; i++)
            {
                action?.Invoke(hits[i].collider);
            }
        }
        
        public static void OverlapSphereCastNonAlloc(
            Vector3 startPoint, 
            float radius,
            LayerMask layerMask, 
            Action<Collider> action)
        {
            Collider[] hitColliders = CacheSingle<Collider[]>.Read();
            int hitCount = Physics.OverlapSphereNonAlloc(
                startPoint, 
                radius, 
                hitColliders,
                layerMask);
            
            for (int i = 0; i < hitCount; i++)
            {
                action?.Invoke(hitColliders[i]);
            }
        }

        public static bool TryDealSingleDamage(
            Collider collider, 
            int damage, 
            Team team,
            LayerMask layerMask,
            params IDamageParameter[] parameters)
        {
            if(LayerUtils.CheckForComparerLayer(layerMask, collider.gameObject) == false)
                return false;
            
            Rigidbody rigid = collider.attachedRigidbody;
            if(rigid == null)
                return false;
                
            if (rigid.TryGetComponent(out DamageableMono damageable))
            {
                return damageable.TryDealDamage(damage, team, parameters);
            }

            return false;
        }

        public static bool TryDealOverlapAOEDamage(Vector3 position,
            float radius,
            int damage,
            Team team,
            LayerMask layerMask,
            params IDamageParameter[] parameters)
        {
            bool damageDealing = true;
            Collider[] hitColliders = CacheSingle<Collider[]>.Read();
            var hitsCount = Physics.OverlapSphereNonAlloc(position, radius, hitColliders, layerMask);

            for (int i = 0; i < hitsCount; i++)
            {
                damageDealing &= TryDealSingleDamage(hitColliders[i], damage, team, layerMask, parameters);
            }

            return damageDealing;
        }

        public static HitData GetHitInfo(Transform transform, LayerMask mask)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, mask))
            {
                return new HitData(hit.point, hit.normal);
            }

            return null;
        }
    }
}