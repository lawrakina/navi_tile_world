using System;

namespace Main.Meta.Upgrades.Parameters
{
    [Serializable]
    public class IntUpgradeableParameter : UpgradeableParameter<int>
    {
        public override int Value => (_valueBase + AdditionalBaseAmount) * GetAdditionalMultiplier() + AdditionalTotalAmount;

        private int GetAdditionalMultiplier()
        {
            if (AdditionalMultiplier == 0)
                return 1;

            return AdditionalMultiplier;
        }
        
        public override void Upgrade(UpgradeParameterInfo<int> upgradeParameterInfo)
        {
            base.Upgrade(upgradeParameterInfo);
            
            switch (upgradeParameterInfo.Mode)
            {
                case UpgradeMode.AddBase:
                    AdditionalBaseAmount += upgradeParameterInfo.Value;
                    break;
                case UpgradeMode.Multiplier:
                    AdditionalMultiplier *= upgradeParameterInfo.Value;
                    break;
                case UpgradeMode.AddTotal:
                    AdditionalTotalAmount += upgradeParameterInfo.Value;
                    break;
            }
        }

        public override void Reset()
        {
            AdditionalBaseAmount = 0;
            AdditionalMultiplier = 1;
            AdditionalTotalAmount = 0;
        }
    }
}