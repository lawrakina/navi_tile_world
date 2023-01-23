using System;
using NavySpade.Meta.Runtime.Upgrades;
using UnityEngine;

namespace NavySpade.Meta.Usage.Upgrade.Scripts
{
    [Serializable]
    [AddTypeMenu("Float")]
    public class FloatValue : UpgradeReward
    {
        [field: SerializeField] public float Value { get; private set; }
    }
}