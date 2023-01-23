using System;
using UnityEngine;

namespace Main.Meta.Upgrades.Parameters
{
    [Serializable]
    public abstract class UpgradeableParameter<T>
    {
        [SerializeField] protected bool _isUpgradeable;
        [SerializeField] private ParameterTypes _parameterType;
        [SerializeField] protected T _valueBase;
        
        protected T AdditionalBaseAmount;
        protected T AdditionalMultiplier;
        protected T AdditionalTotalAmount;

        public bool IsUpgradeable
        {
            get => _isUpgradeable;
            set => _isUpgradeable = value;
        }
        
        public ParameterTypes Type
        {
            get => _parameterType;
            set => _parameterType = value;
        } 

        public T ValueBase
        {
            get => _valueBase;
            set => _valueBase = value;
        }

        public abstract T Value { get; }

        public virtual void Upgrade(UpgradeParameterInfo<T> upgradeParameterInfo)
        {
            if(_isUpgradeable == false)
                return;
            
            if (upgradeParameterInfo.Type != Type)
                return;
        }

        public abstract void Reset();
    }
}