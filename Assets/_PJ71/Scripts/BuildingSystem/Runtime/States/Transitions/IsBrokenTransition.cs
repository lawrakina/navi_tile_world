using System;
using NavySpade._PJ71.Actors.States.Transition;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime.States.Transitions
{
    [Serializable]
    [AddTypeMenu("IsBroken")]
    public class IsBrokenTransition : TransitionConditionBase
    {
        [SerializeField] private BuildingHandler _buildingHandler;
        
        public override bool IsMetInner()
        {
            return _buildingHandler.IsBroken;
        }
    }
}