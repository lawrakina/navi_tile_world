using System;
using NaughtyAttributes;
using NavySpade._PJ71.Battle;
using NavySpade.Modules.Sound.Runtime.Core;
using UnityEngine;

namespace Core.Damagables
{
    public class DamageableLight : DamageableMono
    {
        [SerializeField] private Transform _center;
        [SerializeField] private bool _isImmortal;
        [SerializeField] private float _maxHp;
        
        private bool _isAlive;
        private float _hp;
        private Vector3 _currentPunchForce;
        private DamagablesEffect[] _damagablesEffects;
        private BattleFieldConfig _battleField;
        
        public override Team CurrentTeam
        {
            get => Team;
            set
            {
                if (Team != value)
                {
                    Team = value;
                    OnTeamChanged();
                    OnDamageableSetupChanged();
                }
            }
        }

        public override float HP
        {
            get => _hp;
            protected set
            {
                if (value > _maxHp || IsImmortal)
                    value = _maxHp;

                if (value < 0)
                    value = 0;

                if (value < _hp)
                {
                    TakeDamage?.Invoke(_hp - value);
                }

                _hp = value;
                OnHPChangeInvoke();
            }
        }
        
        public override float MaxHp
        {
            get => _maxHp;
            protected set
            {
                var diff = value - _maxHp;
                _maxHp = value;
                
                MaxHPChanged?.Invoke(diff);
            }
        }
        
        public bool IsImmortal
        {
            get => _isImmortal;
            set => _isImmortal = value;
        }
        
        public override bool IsAlive => _isAlive;

        public override Transform CenterBody => _center;

        public event Action<float> TakeDamage;
        public event Action<DamageableLight> OnDeathDamagable;
        public event Action<float> MaxHPChanged; 

        private void Awake()
        {
            _damagablesEffects = GetComponents<DamagablesEffect>();
            _battleField = BattleFieldConfig.Instance;
            ResetHp();
        }

        public override void Init(float maxHp, float hp, Team team)
        {
            base.Init(maxHp, hp, team);
            CheckDeath();
        }

        public override bool CanDealDamage(Team team)
        {
            if (IsAlive == false)
                return false;
            
            if (IsImmortal)
                return false;

            if (_battleField.CanAttack(team, Team) == false)
                return false;
            
            return true;
        }
        
        public override bool TryDealDamage(float damage, Team team, params IDamageParameter[] damageParameters)
        {
            if (CanDealDamage(team) == false)
                return false;
            
            DealDamage(damage, team, damageParameters);
            return true;
        }

        public override void DealDamage(float damage, Team team, params IDamageParameter[] damageParameters)
        {
            if (CurrentTeam == Team.Enemy)
            {
                SoundPlayer.PlaySoundFx("EnemyHit");
            }

            foreach (var damagablesEffect in _damagablesEffects)
            {
                damagablesEffect.TakeDamage(damage, team, damageParameters);
            }

            HP -= damage;
            CheckDeath();
        }

        public override void DealDamage(float damage)
        {
            HP -= damage;
            CheckDeath();
        }

        public override void Reset()
        {
            HP = MaxHp;
            _isAlive = true;
        }

        private void CheckDeath()
        {
            if (HP <= 0 && IsAlive == true)
            {
                if (CurrentTeam == Team.Enemy)
                {
                    SoundPlayer.PlaySoundFx("EnemyDie");
                }

                _isAlive = false;
                OnDeathInvoke();
                OnDeathDamagable?.Invoke(this);
            }
        }
        
        public void ResetHp()
        {
            _hp = _maxHp;
            _isAlive = true;
        }

        [Button()]
        private void Kill()
        {
            HP = 0;
            CheckDeath();
        }

        public void Heal(float recoverHp)
        {
            HP = Mathf.Clamp(_hp + recoverHp, 0, MaxHp);
        }
    }
}