using System.Collections.Generic;
using Core.Damagables;
using Main.Meta.Upgrades.Parameters;
using NaughtyAttributes;
using NavySpade.NavySpade.Modules.Meta.Runtime.Upgrades;
using UnityEngine;

namespace NavySpade.PJ70.Core.Ammunition
{
    [CreateAssetMenu(fileName = "AmmoConfig", menuName = "Data/AmmoConfig")]
    public class AmmoConfig : ScriptableObject, IUpgradeable
    {
        public bool IsInstant = false;
        
        [HideIf(nameof(IsInstant))] 
        public FloatUpgradeableParameter MovementSpeed;
        
        
        [Header("Instantiating")]
        [HideIf(nameof(IsInstant))]
        public AmmoHandler Prefab;
        
        [HideIf(nameof(IsInstant))]
        public bool FromPool;
        
        [ShowIf(nameof(FromPool))]
        public string PoolName;
        
        
        [Header("Damage Info")]
        public FloatUpgradeableParameter Damage;
        public FloatUpgradeableParameter AOERadius;
        public FloatUpgradeableParameter PiercedObjects;
        public LayerMask LayerMask;
        
        [SerializeReference] 
        [SubclassSelector]
        public IDamageParameter[] Effects;
        
        
        [Header("HitEffect")]
        public bool HasHitEffect;
        
        [ShowIf(nameof(HasHitEffect))] 
        public GameObject HitEffect;
        
        [ShowIf(nameof(HasHitEffect))] 
        public bool EffectFromPool;
        
        [ShowIf(EConditionOperator.And, nameof(HasHitEffect), nameof(EffectFromPool))]  
        public string EffectPoolName;

        public List<FloatUpgradeableParameter> GetParameters()
        {
            return new List<FloatUpgradeableParameter>()
            {
                MovementSpeed,
                Damage,
                AOERadius,
                PiercedObjects
            };
        }
    }
}