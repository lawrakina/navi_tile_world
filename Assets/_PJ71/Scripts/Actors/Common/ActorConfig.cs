using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace Core.Actors
{
    public class ActorConfig : ObjectConfig<ActorConfig>
    {
        public Material DeathMaterial;
        public float DyingTime = 1f;
        public float MINVelocityForDeath = 1f;
        public float DisposeYPoint = -1f;
        public float DisposeTime = 1f;
        public bool CheckMinVelocityForDeath = false;
        public float DeathForce = 5;
    }
}