using System;
using NavySpade._PJ71.Tiles;
using UnityEngine;

namespace NavySpade.pj77.Tutorial.Condition
{
    [Serializable]
    [CustomSerializeReferenceName("TileOpenCondition")]
    public class TileOpenCondition : ITutorCondition
    {
        [SerializeField] private TileMapHandler _tileMapHandler;
        
        public event Action ConditionChanged;
        
        public void Enable()
        {
            _tileMapHandler.TileUnlocked += TileUnlocked;
        }
        
        public void Disable()
        {
            _tileMapHandler.TileUnlocked -= TileUnlocked;
        }
        
        private void TileUnlocked()
        {
            ConditionChanged?.Invoke();
        }
    }
}