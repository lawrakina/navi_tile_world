using System;
using Core.Damagables;
using JetBrains.Annotations;
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace NavySpade._PJ71.Battle
{
    public class BattleFieldConfig : ObjectConfig<BattleFieldConfig>
    {
        [Header("Settings")]
        public float AmmoDestroyTime = 3f;
        public float FlagCapturingTime = 3f;
        public TeamPreset[] BattleTeamPreset;
        
        [Serializable]
        public struct TeamPreset
        {
            public Team Team;
            public Team[] Enemies;
        }

        public bool CanAttack(Team team, Team target)
        {
            foreach (var teamPreset in BattleTeamPreset)
            {
                if (teamPreset.Team == team)
                {
                    foreach (var teamEnemies in teamPreset.Enemies)
                    {
                        if (target == teamEnemies)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }   
}