using System;
using Core.Meta.Unlocks;
using NavySpade.Core.Runtime.Game;
using UnityEngine;

namespace NavySpade._PJ71.Meta
{
    [Serializable]
    [CustomSerializeReferenceName("LevelUnlock")]
    public class LevelUnlockCondition : IUnlockCondition
    {
        [field: SerializeField] public int Level { get; private set; }
        
        public bool IsMatch()
        {
            return GameContext.Instance.LevelManager.LevelIndex >= Level - 1;
        }
    }
}