using EventSystem.Runtime.Core.Managers;
using UnityEngine;

namespace NavySpade.PJ70.Analytics
{
    public abstract class AnalyticsProvider : MonoBehaviour
    {
        protected bool IsLevelStarted;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            EventManager.Add(GameStatesEM.LevelLoaded, OnResetLevel);
            EventManager.Add(GameStatesEM.OnFail, OnLevelFailed);
            EventManager.Add(GameStatesEM.OnWin, OnLevelWin);
        }

        private void OnDestroy()
        {
            EventManager.Remove(GameStatesEM.LevelLoaded, OnResetLevel);
            EventManager.Remove(GameStatesEM.OnFail, OnLevelFailed);
            EventManager.Remove(GameStatesEM.OnWin, OnLevelWin);
        }

        protected abstract void OnResetLevel();

        protected abstract void OnLevelFailed();

        protected abstract void OnLevelWin();
    }
}