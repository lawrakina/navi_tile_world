using System;
using AYellowpaper;
using NavySpade._PJ71.UI;
using NavySpade.Meta.Runtime.Economic.Rewards.Interfaces;
using UnityEngine;

namespace NavySpade._PJ71.Meta
{
    
    [Serializable]
    [CustomSerializeReferenceName("Item")]
    public class ItemReward : IReward
    {
        [field: SerializeField] public InterfaceReference<IItemRewardData> Data { get; private set; }
        
        public void TakeReward() { }
    }
}