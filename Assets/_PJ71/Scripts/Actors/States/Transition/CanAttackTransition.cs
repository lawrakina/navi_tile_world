using System;
using NavySpade._PJ71.Scripts.Actors.Runtime;
using UnityEngine;

namespace NavySpade._PJ71.Actors.States.Transition
{
    [Serializable]
    [AddTypeMenu("CanAttack")]
    public class CanAttackTransition : TransitionConditionBase
    {
        [SerializeField] private ShootingUnit _unit;

        public override bool IsMetInner()
        {
            return _unit.CanAttack;
        }
    }
}