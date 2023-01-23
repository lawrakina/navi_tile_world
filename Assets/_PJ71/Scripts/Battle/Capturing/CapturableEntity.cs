using AYellowpaper;
using Core.Damagables;
using NavySpade.Modules.Extensions.UnityTypes;
using NavySpade.NavySpade.Modules.Damageble.Damagables.Teams;
using UnityEngine;

namespace NavySpade._PJ71.Battle.Capturing
{
    public class CapturableEntity : ExtendedMonoBehavior<CapturableEntity>
    {
        [SerializeField] private InterfaceReference<ITeam> _team;

        public Team Team => _team.Value.CurrentTeam;
    }
}