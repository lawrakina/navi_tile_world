using NavySpade.Core.Runtime.Game;
using UnityEngine;

namespace NavySpade.PJ70.Analytics
{
    public class HoopslySDKIntegration : AnalyticsProvider
    {
        protected override void OnResetLevel()
        {
            int levelIndex = GameContext.Instance.LevelManager.LevelIndex;
           
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.white)}> Hoopsly startLevel {levelIndex} </color>");
            
            HoopslyIntegration.RaiseLevelStartEvent(levelIndex.ToString());
            IsLevelStarted = true;
        }

        protected override void OnLevelFailed()
        {
            if(IsLevelStarted == false)
                return;
            
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.red)}> Hoopsly endLevel lose {GameContext.Instance.LevelManager.LevelIndex} </color>");
            RaiseLevelEndEvent(LevelFinishedResult.lose);
        }

        protected override void OnLevelWin()
        {
            if(IsLevelStarted == false)
                return;
            
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}> Hoopsly endLevel win {GameContext.Instance.LevelManager.LevelIndex} </color>");
            RaiseLevelEndEvent(LevelFinishedResult.win);
        }

        private void RaiseLevelEndEvent(LevelFinishedResult result)
        {
            int levelIndex = GameContext.Instance.LevelManager.LevelIndex;
            HoopslyIntegration.RaiseLevelFinishedEvent(levelIndex.ToString(), result);
            IsLevelStarted = false;
        }
    }
}