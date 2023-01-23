using System;
using NavySpade._PJ71.Actors.States.Transition;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime.States.Transitions
{
    [Serializable]
    [AddTypeMenu("IdleToProduceBarrack")]
    public class IdleToProduceBarrack : TransitionConditionBase
    {
        [SerializeField] private ProductionBuilding _productionBuilding;
        
        public override bool IsMetInner()
        {
            return _productionBuilding.InProduction == false && _productionBuilding.CanProduce();
        }
    }
}