using System;
using GameAnalyticsSDK;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.Modules.Saving.Runtime.Interfaces;
using UnityEngine;

namespace NavySpade.PJ70.Analytics
{
    public class FTUE : MonoBehaviour, ISaveable
    {
        private const string SaveKey_TimeInGame = "Analytics.FTUE.Time";
        private const string SaveKey_LastEventTime = "Analytics.FTUE.LastTime";
        
        [SerializeField] private FTUEStep[] _ftueSteps;

        private FTUEStep _currentFtueStep;
        private float _timeInGame;
        private float _lastTimeInvoked;
        
        private void Start()
        {
            RestoreState(null);
            UpdateFtueStep();
        }
        
        private void Update()
        {
            if(_currentFtueStep.UntilTime == 0)
                return;

            _timeInGame += Time.deltaTime;
            if (_timeInGame - _lastTimeInvoked > _currentFtueStep.Step)
            {
                SendEvent((int) _timeInGame);
                _lastTimeInvoked = _timeInGame;

                if (_timeInGame > _currentFtueStep.UntilTime)
                {
                    UpdateFtueStep();
                }
            }
        }

        private void SendEvent(int intTime)
        {
            //Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(Color.blue)}> FTUE: {intTime} </color>");
            GameAnalytics.NewDesignEvent($"FTUE:TimeInGame:{intTime}");
        }

        private void UpdateFtueStep()
        {
            foreach (var ftueStep in _ftueSteps)
            {
                if (_timeInGame < ftueStep.UntilTime)
                {
                    _currentFtueStep = ftueStep;
                    return;
                }
            }

            _currentFtueStep = new FTUEStep();
        }
        
        public object CaptureState()
        {
            SaveManager.Save(SaveKey_TimeInGame, _timeInGame);
            SaveManager.Save(SaveKey_LastEventTime, _lastTimeInvoked);
            return null;
        }

        public void RestoreState(object state)
        {
            _timeInGame = SaveManager.Load(SaveKey_TimeInGame, 0f);
            _lastTimeInvoked = SaveManager.Load(SaveKey_LastEventTime, 0f);
        }

        public void ClearSave() { }
        
        [Serializable]
        public struct FTUEStep
        {
            public float UntilTime;
            public float Step;
        }
    }
}