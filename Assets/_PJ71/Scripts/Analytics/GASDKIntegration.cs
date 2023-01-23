using System.Collections;
using GameAnalyticsSDK;
using NavySpade.Core.Runtime.Game;
using UnityEngine;

namespace NavySpade.PJ70.Analytics
{
    public class GASDKIntegration : AnalyticsProvider
    {
        private void Start()
        {
            GameAnalytics.Initialize();
        }

        protected override void OnResetLevel()
        {
            int levelIndex = GameContext.Instance.LevelManager.LevelIndex;
            
            Debug.Log(ColorUtility.ToHtmlStringRGB(Color.white));
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.white)} > GA startLevel {levelIndex} </color>");
            
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelIndex.ToString());
            IsLevelStarted = true;
        }

        protected override void OnLevelFailed()
        {
            if(IsLevelStarted == false)
                return;
            
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.red)}> GA endLevel lose {GameContext.Instance.LevelManager.LevelIndex} </color>");
            RaiseLevelEndEvent(GAProgressionStatus.Fail);
        }

        protected override void OnLevelWin()
        {
            if(IsLevelStarted == false)
                return;
            
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}> GA endLevel win {GameContext.Instance.LevelManager.LevelIndex} </color>");
            RaiseLevelEndEvent(GAProgressionStatus.Complete);
        }

        private void RaiseLevelEndEvent(GAProgressionStatus status)
        {
            int levelIndex = GameContext.Instance.LevelManager.LevelIndex;
            GameAnalytics.NewProgressionEvent(status, levelIndex.ToString());
            IsLevelStarted = false;
        }
    }
}