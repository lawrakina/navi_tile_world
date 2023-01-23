using System.Collections.Generic;
using Main.Meta.Upgrades.Parameters;
using NavySpade.NavySpade.Modules.Meta.Runtime.Upgrades;
using NavySpade.PJ70.Core.Ammunition;
using UnityEngine;

namespace Core.Actors
{
    [CreateAssetMenu(fileName = "FiringActor", menuName = "Data/ActorPresets/FiringActor")]
    public class ShootingActorPreset : ActorPreset
    {
        public AmmoConfig AmmoConfig;
        public FloatUpgradeableParameter DelayBetweenShoots;
        public Vector3 FireSpread;
        public FloatUpgradeableParameter ViewRadius;
        public FloatUpgradeableParameter AttackRadius;
        public FloatUpgradeableParameter MovementSpeed;
        public float AngularSpeed;
        
        public override List<FloatUpgradeableParameter> GetParameters()
        {
            List<FloatUpgradeableParameter> parameters = base.GetParameters();
            parameters.Add(DelayBetweenShoots);
            parameters.Add(ViewRadius);
            parameters.Add(AttackRadius);
            parameters.Add(MovementSpeed);
            return parameters;
        }
    }
}