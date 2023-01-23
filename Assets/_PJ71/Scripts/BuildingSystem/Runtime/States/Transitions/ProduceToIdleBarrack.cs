using System;
using NavySpade._PJ71.Actors.States.Transition;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime.States.Transitions
{
    [Serializable]
    [AddTypeMenu("ProduceToIdle")]
    public class ProduceToIdleBarrack : TransitionConditionBase
    {
        [SerializeField] private ProductionBuilding _building;
        
        public override bool IsMetInner()
        {
            return _building.InProduction == false && _building.CanProduce() == false;
        }
    }
}