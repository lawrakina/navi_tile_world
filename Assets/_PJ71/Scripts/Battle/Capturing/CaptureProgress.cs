using System;
using System.Collections.Generic;
using Core.Damagables;
using NavySpade.NavySpade.Modules.Utils.Timers;
using UnityEngine;
using UnityEngine.UI;

namespace NavySpade._PJ71.Battle.Capturing
{
    public class CaptureProgress : MonoBehaviour
    {
        [Serializable]
        public struct Preset
        {
            public Team Team;
            public Slider Slider;
        }

        public Preset[] _presets;
        public float _timeUpdate;
        
        private Dictionary<Team, int> _entityDict = new Dictionary<Team, int>();
        private int _totalCount;
        private Timer _timer;

        private void Start()
        {
            _timer = new Timer(_timeUpdate);
        }

        private void Update()
        {
            _timer.Update(Time.deltaTime);
            if (_timer.IsFinish())
            {
                UpdateProgress();
                _timer.Reload();
            }
        }

        private void UpdateProgress()
        {
            _totalCount = 0;
            _entityDict.Clear();
            
            foreach (var capturableEntity in CapturableEntity.Active)
            {
                _totalCount++;
                if (_entityDict.ContainsKey(capturableEntity.Team) == false)
                {
                    _entityDict.Add(capturableEntity.Team, 0);
                }

                _entityDict[capturableEntity.Team]++;
            }

            foreach (var preset in _presets)
            {
                int count = _entityDict.ContainsKey(preset.Team) ? _entityDict[preset.Team] : 0;
                preset.Slider.value = (float) count / _totalCount;
            }
        }
    }
}