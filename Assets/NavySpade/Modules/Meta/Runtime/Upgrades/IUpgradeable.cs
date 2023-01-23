using System.Collections.Generic;
using Core.Meta.Shop.Upgrades;
using Main.Meta.Upgrades.Parameters;

namespace NavySpade.NavySpade.Modules.Meta.Runtime.Upgrades
{
    public interface IUpgradeable
    {
        public List<FloatUpgradeableParameter> GetParameters();
    }
}