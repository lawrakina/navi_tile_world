using System;
using Mono.CSharp;
using NavySpade.NavySpade.Modules.Damageble.Damagables.Teams;
using UnityEngine;

namespace Core.Damagables
{
    public abstract class DamageableMono : MonoBehaviour, IDamageble, ITeam
    {
        [SerializeField] protected Team Team;
        
        public abstract float HP { get; protected set; }
        
        public abstract float MaxHp { get; protected set; }
        
        public abstract Team CurrentTeam { get; set; }

        public abstract bool IsAlive { get; }

        public abstract Transform CenterBody { get; }

        public event Action<Team> TeamChanged;

        public event Action<DamageableMono> DamageableSetupChanged;
        
        public event Action<float> OnHPChange;
        
        public event Action<DamageableMono> OnDeath;

        public abstract bool CanDealDamage(Team team);
        
        public abstract bool TryDealDamage(float damage, Team team, params IDamageParameter[] damageParameters);
        
        public abstract void DealDamage(float damage, Team team, params IDamageParameter[] damageParameters);

        public abstract void DealDamage(float damage);

        public void Init(float hp, Team team)
        {
            MaxHp = hp;
            CurrentTeam = team;
            Reset();
        }
        
        public virtual void Init(float maxHp, float hp, Team team)
        {
            MaxHp = maxHp;
            HP = hp;
            CurrentTeam = team;
        }

        protected void OnDeathInvoke()
        {
            OnDeath?.Invoke(this);
        }
        
        protected void OnHPChangeInvoke()
        {
            OnHPChange?.Invoke(HP);
        }

        protected void OnTeamChanged()
        {
            TeamChanged?.Invoke(Team);
        }
        
        protected void OnDamageableSetupChanged()
        {
            DamageableSetupChanged?.Invoke(this);
        }
        
        public abstract void Reset();

    }
}