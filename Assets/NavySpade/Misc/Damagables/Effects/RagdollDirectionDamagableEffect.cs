using System;
using System.Linq;
using Core.Actors;
using Core.Damagables;
using UnityEngine;

namespace Misc.Damagables.Effects
{
    public class RagdollDirectionDamagableEffect : DamagablesEffect
    {
        public Vector3 RagdollPower { get; private set; }
        
        public Vector3 HitPosition { get; private set; }

        private ActorConfig _actorConfig;
        
        private void Awake()
        {
            _actorConfig = ActorConfig.Instance;
        }

        public override void TakeDamage(float damage, Team team, IDamageParameter[] damageParameters)
        {
            var parameter = damageParameters.FirstOrDefault(param => param is HitData) as HitData;
            
            if(parameter == null)
                return;

            HitPosition = parameter.HitPoint;
            RagdollPower = -1 * _actorConfig.DeathForce * parameter.HitNormal ;
        }
    }
}