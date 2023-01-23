using System;
using NavySpade._PJ71.Battle.EnemyDetection;
using UnityEngine;

namespace NavySpade._PJ71.Actors.States.Transition
{
    [Serializable]
    [AddTypeMenu("HasEnemy")]
    public class HasEnemyTransition : TransitionConditionBase
    {
        [SerializeField] private EnemyDetector _enemyDetector;
        
        public override bool IsMetInner()
        {
            return _enemyDetector.HasTargets;
        }
    }
}