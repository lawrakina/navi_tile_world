using EventSystem.Runtime.Core.Managers;
using Main.Levels.Data;
using NaughtyAttributes;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels
{
    public abstract class LevelBase : MonoBehaviour
    {
        private bool _popupInvoked;
        
        public abstract void Init(LevelDataBase data);
        
        [Button]
        protected void InvokeWinState()
        {
            if (_popupInvoked == false)
            {
                _popupInvoked = true;
                
                // GameContext.Instance.EarnedCurrency.ObservedCurrency.Count += 
                //     BattleFieldConfig.Instance.MoneyForWinFight;
                
                EventManager.Invoke(MainEnumEvent.Win);
            }
        }

        [Button]
        protected void InvokeLoseState()
        {
            if (_popupInvoked == false)
            {
                _popupInvoked = true;
                
                // GameContext.Instance.EarnedCurrency.ObservedCurrency.Count += 
                //     BattleFieldConfig.Instance.MoneyForLoseFight;
                
                EventManager.Invoke(MainEnumEvent.Fail);
            }
        }
    }
}