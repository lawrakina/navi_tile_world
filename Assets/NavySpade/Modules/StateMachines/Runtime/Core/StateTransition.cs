using System;
using System.Collections.Generic;
using System.Linq;
using Depra.Toolkit.StateMachines.Runtime.Core.Interfaces;
using NavySpade.Modules.StateMachines.Runtime.Mono;
using UnityEngine;

namespace Depra.Toolkit.StateMachines.Runtime.Core
{
    [Serializable]
    public class StateTransition
    {
        [field: SerializeField] public StateBehavior NextState { get; private set; }

        [SerializeReference, SubclassSelector] private List<IStateTransitionCondition> _conditions;

        public bool ShouldTransition()
        {
            for (int i = 0; i < _conditions.Count; i++)
            {
                if (_conditions[i].IsMet() == false)
                    return false;
            }

            return true;
        }
    }
}