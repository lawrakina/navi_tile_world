using System;

namespace Main.Meta.Upgrades.Parameters
{
    [Serializable]
    public abstract class UpgradeParameterInfo<T> : IUpgradeInfo
    {
        public ParameterTypes Type;
        public UpgradeMode Mode;
        public T Value;
    }

    public interface IUpgradeInfo
    {
        
    }
}