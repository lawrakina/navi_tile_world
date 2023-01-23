using System;
using NavySpade._PJ71.BuildingSystem.Runtime;
using NavySpade._PJ71.Gathering;
using NavySpade._PJ71.Utils;
using UnityEngine;

namespace NavySpade._PJ71.Actors.States.Transition
{
    [Serializable]
    [AddTypeMenu("HasMinedObjects")]
    public class HasMinedObjects : TransitionConditionBase
    {
        [SerializeField] private MinedObjectDetector _detector;
        
        public override bool IsMetInner()
        {
            return _detector.CanGather;
        }
    }
}