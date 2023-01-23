using System;
using Depra.Toolkit.StateMachines.Runtime.Core.Interfaces;
using UnityEngine;

namespace NavySpade._PJ71.Actors.States.Transition
{
    [Serializable]
    public abstract class TransitionConditionBase : IStateTransitionCondition
    {
        [SerializeField] private bool _invertResult;

        public bool IsMet()
        {
            bool result = IsMetInner();
            return _invertResult ? !result : result;
        }

        public abstract bool IsMetInner();
    }
}