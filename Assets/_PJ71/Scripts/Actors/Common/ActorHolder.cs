using System;
using Core.Damagables;
using NS.Core.Actors;
using UnityEngine;

namespace Core.Actors
{
    public abstract class ActorHolder : MonoBehaviour
    {
        [SerializeField] private DamageableMono _damageable;
        [SerializeField] private ActorPreset _actorPreset;

        protected IActorHandler Handler;

        private bool _isDead;
        
        public bool IsDead
        {
            get => _isDead;
            set => _isDead = value;
        }

        public ActorPreset Preset
        {
            get => _actorPreset;
            set => _actorPreset = value;
        }
        
        public DamageableMono Damageable => _damageable;

        public event Action<ActorHolder> OnDied;
        
        public virtual void Init(IActorHandler handler, ActorPreset actorPreset, Team team)
        {
            Preset = actorPreset;
            Handler = handler;

            _isDead = false;
            _damageable.Init(actorPreset.HP.Value, team);
            Damageable.OnDeath += Die;
        }

        private void OnDisable()
        {
            Damageable.OnDeath -= Die;
        }

        private void Die(DamageableMono damageable)
        {
            if(IsDead)
                return;
            
            DieInternal();
            OnDied?.Invoke(this);
            IsDead = true;
        }
        
        protected abstract void DieInternal();
    }
}