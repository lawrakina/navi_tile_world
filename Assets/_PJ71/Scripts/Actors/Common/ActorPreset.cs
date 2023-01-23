using System.Collections.Generic;
using Core.Meta.Shop.Upgrades;
using Main.Meta.Upgrades.Parameters;
using NavySpade.NavySpade.Modules.Meta.Runtime.Upgrades;
using UnityEngine;

namespace Core.Actors
{
    [CreateAssetMenu(fileName = "SimpleActor", menuName = "Data/ActorPresets/Simple")]
    public class ActorPreset : ScriptableObject, IUpgradeable
    {
        public UnitType Type;
        public ActorHolder Prefab;
        public FloatUpgradeableParameter HP;
        
        public enum UnitType
        {
            Player,
            SimpleUnit,
        }

        public virtual List<FloatUpgradeableParameter> GetParameters()
        {
            return new List<FloatUpgradeableParameter>() {HP};
        }
    }
}