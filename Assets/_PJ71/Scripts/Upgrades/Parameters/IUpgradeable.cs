namespace Main.Meta.Upgrades.Parameters
{
    public interface IUpgradeable<T>
    {
        public T Value { get; }

        public void Upgrade<TU>(TU upgradeParameterInfo) where TU : UpgradeParameterInfo<T>;

        public void Reset();
    }
}