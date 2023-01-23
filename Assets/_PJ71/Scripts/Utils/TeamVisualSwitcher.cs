using System;
using System.Linq;
using AYellowpaper;
using Core.Damagables;
using NavySpade.NavySpade.Modules.Damageble.Damagables.Teams;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade._PJ71.Utils
{
    public class TeamVisualSwitcher : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<ITeam> _team;
        [SerializeField] private VisualSetup[] _visualSetups;
        public Team Team => _team.Value.CurrentTeam;

        public void OnEnable()
        {
            _team.Value.TeamChanged += SwitchVisual;
            SwitchVisual(_team.Value.CurrentTeam);
        }
        
        private void OnDisable()
        {
            _team.Value.TeamChanged -= SwitchVisual;
        }
        
        private void SwitchVisual(Team team)
        {
            Array.ForEach(_visualSetups, (v) =>
            {
                if (v.Team == team)
                {
                    v.Event.Invoke();
                }
            });
        }
        
        [Serializable]
        public struct VisualSetup
        {
            public Team Team;
            public UnityEvent Event;
        }
    }
}