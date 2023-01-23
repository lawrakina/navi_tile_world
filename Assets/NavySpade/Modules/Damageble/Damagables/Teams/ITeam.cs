using System;
using Core.Damagables;

namespace NavySpade.NavySpade.Modules.Damageble.Damagables.Teams
{
    public interface ITeam
    {
        public event Action<Team> TeamChanged;
        
        public Team CurrentTeam { get; }
    }
}