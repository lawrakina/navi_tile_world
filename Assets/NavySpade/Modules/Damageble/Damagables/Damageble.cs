﻿using System;
using System.Collections.Generic;
using Core.Meta.Analytics;
using JetBrains.Annotations;
using NavySpade.Meta.Runtime.Analytics;
using UnityEngine;

namespace Core.Damagables
{
    public class Damageble : DamageableMono
    {
        [Trackable(EnemyKillsAnalyticsKey)] private const string EnemyKillsAnalyticsKey = "Enemy Kills";

        [SerializeField] private Team _team;

        [SerializeField] private bool _isImmortal;
        [SerializeField] private float _maxHp;
        [SerializeField] private DamagebleEvents _events = new DamagebleEvents();

        public static event Action<Damageble> OnCreate, OnDestroy;
        public static event Action<Damageble> OnTakeDamageGlobal;

        private bool _isAlive;
        private float _hp;
        private DamagablesEffect[] _damagablesEffects;

        private static Dictionary<Team, List<Damageble>> _allDamagableByTeam;

        //public event Action<float> OnHPChange;
        
        public event Action<float> MaxHPChanged; 
        
        public event Action<float> TakeDamage, OnHeal;
        
        public override Transform CenterBody => null;
        
        public event Action<Damageble> OnDeathDamagable;
        
        public override float MaxHp
        {
            get => _maxHp;
            protected set
            {
                float diff = value - _maxHp;
                _maxHp = value;
                
                MaxHPChanged?.Invoke(diff);
            }
        }

        public override float HP
        {
            get => _hp;
            protected set
            {
                if (value == _hp)
                    return;

                if (value > _maxHp || IsImmortal)
                    value = _maxHp;

                if (value < 0)
                    value = 0;

                if (value < _hp)
                {
                    TakeDamage?.Invoke(_hp - value);
                    OnTakeDamageGlobal?.Invoke(this);
                    _events.OnTakeDamage.Invoke();
                }

                _hp = value;
                OnHPChangeInvoke();
                OnTakeDamageGlobal?.Invoke(this);
            }
        }

        public virtual Vector3 RagdollDir { get; set; }

        public override Team CurrentTeam
        {
            get => _team;
            set
            {
                if (_team == value)
                    return;

                if (_allDamagableByTeam == null)
                    _allDamagableByTeam = new Dictionary<Team, List<Damageble>>();

                if (_allDamagableByTeam.ContainsKey(_team))
                    _allDamagableByTeam[_team].Remove(this);

                if (_allDamagableByTeam.ContainsKey(value) == false)
                    _allDamagableByTeam.Add(value, new List<Damageble>());

                _allDamagableByTeam[value].Add(this);

                _team = value;
                OnDamageableSetupChanged();
            }
        }

        public virtual bool IsImmortal
        {
            get => _isImmortal;
            set => _isImmortal = value;
        }

        public override bool IsAlive => _isAlive;

        private void Awake()
        {
            _damagablesEffects = GetComponents<DamagablesEffect>();

            ResetHP();

            OnAwake();
        }

        protected virtual void OnEnable()
        {
            if (_allDamagableByTeam == null)
                _allDamagableByTeam = new Dictionary<Team, List<Damageble>>();

            if (_allDamagableByTeam.ContainsKey(CurrentTeam) == false)
                _allDamagableByTeam.Add(CurrentTeam, new List<Damageble>());

            _allDamagableByTeam[CurrentTeam].Add(this);

            OnCreate?.Invoke(this);

            VariableTracker.BindKey(EnemyKillsAnalyticsKey);
        }

        protected virtual void OnDisable()
        {
            _allDamagableByTeam[CurrentTeam].Remove(this);
            OnDestroy?.Invoke(this);
        }

        protected virtual void OnAwake()
        {
        }

        [CanBeNull]
        public static List<Damageble> GetAllEnemysOfTeam(Team team)
        {
            switch (team)
            {
                case Team.Neutral:
                {
                    var list = new List<Damageble>();
                    list.AddRange(_allDamagableByTeam[Team.Player]);
                    list.AddRange(_allDamagableByTeam[Team.Enemy]);
                    return list;
                }
                case Team.Enemy:
                    
                    if (_allDamagableByTeam.ContainsKey(Team.Player) == false)
                        return null;
                    
                    return _allDamagableByTeam[Team.Player];
                case Team.Player:
                    
                    if (_allDamagableByTeam.ContainsKey(Team.Enemy) == false)
                        return null;
                    
                    return _allDamagableByTeam[Team.Enemy];
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }

        public static List<Damageble> GetAll(Team team)
        {
            return _allDamagableByTeam[team];
        }

        public override bool CanDealDamage(Team team)
        {
            return true;
        }

        public override bool TryDealDamage(float damage, Team team, params IDamageParameter[] damageParameters)
        {
            throw new NotImplementedException();
        }

        public override void DealDamage(float damage, Team team, params IDamageParameter[] damageParameters)
        {
            if (IsImmortal)
                return;

            if (_team == team && team != Team.Neutral)
                return;

            foreach (var damagablesEffect in _damagablesEffects)
            {
                damagablesEffect.TakeDamage(damage, team, damageParameters);
            }

            HP -= damage;

            if (HP == 0 && IsAlive == true)
            {
                if (CurrentTeam == Team.Enemy)
                {
                    VariableTracker.AddValue(EnemyKillsAnalyticsKey, 1);
                }

                OnDeathInvoke();
                _events.OnDead.Invoke();
                OnDeathDamagable?.Invoke(this);
                _isAlive = false;
            }
        }

        public virtual void Heal(int value, Team team)
        {
            if (_team != team)
                return;

            OnHeal?.Invoke(value);
            HP += value;
        }

        public virtual void ResetHP()
        {
            _hp = _maxHp;
            _isAlive = true;
        }

        public void ForceKill()
        {
            var isImmortal = IsImmortal;
            IsImmortal = false;
            TryKill();
            IsImmortal = isImmortal;
        }

        public bool TryKill()
        {
            DealDamage(HP, Team.Neutral);
            return IsImmortal == false;
        }

        public void SetImmortal(bool value)
        {
            _isImmortal = value;
        }

        public override void DealDamage(float damage)
        {
            HP -= damage;
        }

        public override void Reset()
        {
            HP = MaxHp;
            _isAlive = true;
        }
    }
}