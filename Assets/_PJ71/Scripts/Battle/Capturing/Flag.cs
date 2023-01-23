using System;
using System.Collections.Generic;
using Core.Damagables;
using NavySpade._PJ71.Scripts.Positions;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade._PJ71.Battle
{
    public class Flag : MonoBehaviour, ICapturable
    {
        [SerializeField] private float _customCaptureProgress = 0;
        [SerializeField] private Events _events;
        [SerializeField] private PointsHolder _pointsHolder;

        private bool _initialized;
        private BattleFieldConfig _battleConfig;
        private readonly Dictionary<Team, List<ICapturer>> _unitsInFlag = new Dictionary<Team, List<ICapturer>>();
        private int _totalUnitsInFlag;

        private CaptureState _captureState;

        private float _currentCaptureTime;
        private bool _isCaptureProcess;

        public Team CurrentTeam { get; private set; }

        public Team CapturedTeam { get; private set; }

        public float CaptureProgress
        {
            get
            {
                if (_battleConfig == null)
                    return 0;
                
                return _currentCaptureTime / _battleConfig.FlagCapturingTime;
            }
        } 

        private float CurrentCaptureTime
        {
            get => _currentCaptureTime;
            set
            {
                _currentCaptureTime = value;
                CaptureProgressChanged?.Invoke(CaptureProgress);
            }
        }

        public CaptureState CurrentCaptureState
        {
            get => _captureState;
            set
            {
                if(CurrentCaptureState == value)
                    return;
            
                if(CurrentCaptureState != CaptureState.Capturing && value == CaptureState.Capturing)
                    _events.OnStartCapturing.Invoke();
            
                if(CurrentCaptureState == CaptureState.Capturing && value != CaptureState.Capturing)
                    _events.OnEndCapturing.Invoke();
            
                _captureState = value;
                StateChanged?.Invoke(_captureState);
            }
        }
        
        public event Action<Team> IsCaptured;
        
        public event Action<float> CaptureProgressChanged;

        public event Action<CaptureState> StateChanged;

        public void Init(Team team)
        {
            CurrentTeam = CapturedTeam = team;
            _battleConfig = BattleFieldConfig.Instance;
            _initialized = true;
            
            CurrentCaptureTime = _battleConfig.FlagCapturingTime * _customCaptureProgress;
            UpdateCaptureState();
        }
        
        private void Update()
        {
            if (_isCaptureProcess)
            {
                float newProgress = CurrentCaptureTime;
                if (CapturedTeam == CurrentTeam)
                {
                    newProgress -= Time.deltaTime;
                }
                else
                {
                    newProgress += Time.deltaTime;
                }
                
                newProgress = Mathf.Clamp(newProgress, 0, _battleConfig.FlagCapturingTime);
                if (Mathf.Abs(newProgress - CurrentCaptureTime) > Mathf.Epsilon)
                {
                    CurrentCaptureTime = newProgress;
                }
            }
            
            UpdateCaptureState();
        }

        private void UpdateCaptureState()
        {
            if (CaptureProgress >= 1)
            {
                CapturedTeam = CapturedTeam;
                CurrentCaptureTime = 0;
                IsCaptured?.Invoke(CapturedTeam);
                _events.OnCaptured?.Invoke();
            } 
            else if (CaptureProgress <= 0)
            {
                CurrentCaptureState = CaptureState.FullControl;
            }
            else
            {
                CurrentCaptureState = _isCaptureProcess ? CaptureState.Capturing : CaptureState.Draw;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_initialized == false)
                return;
            
            if (other.TryGetComponent(out ICapturer capturer))
            {
                capturer.StartCapture(this);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ICapturer capturer))
            {
                capturer.StopCapture(this);
            }
        }
        
        public int AddToCapture(ICapturer capturer)
        {
            if (_unitsInFlag.ContainsKey(capturer.Team) == false)
            {
                _unitsInFlag.Add(capturer.Team, new List<ICapturer>());
            }

            List<ICapturer> capturers = _unitsInFlag[capturer.Team];
            if(capturers.Contains(capturer))
                return capturers.IndexOf(capturer);
            
            capturers.Add(capturer);
            _totalUnitsInFlag++;
            CheckCapturingState();
            return capturers.Count - 1;
        }

        public void RemoveFromCapture(ICapturer capturer)
        {
            if(_unitsInFlag.ContainsKey(capturer.Team) == false)
                return;
            
            List<ICapturer> capturers = _unitsInFlag[capturer.Team];
            capturers.Remove(capturer);
            _totalUnitsInFlag--;
            
            CheckCapturingState();
        }

        public Vector3 GetPositionForCapture(int index)
        {
            return _pointsHolder.GetPosition(index);
        }

        private void CheckCapturingState()
        {
            int teamUnits = 0;
            int enemyUnits = 0;
            Team enemyTeam = Team.Enemy;
            foreach (var unitsPair in _unitsInFlag)
            {
                if (unitsPair.Key == CurrentTeam)
                {
                    teamUnits = _unitsInFlag[unitsPair.Key].Count;
                    continue;
                }

                if (_battleConfig.CanAttack(unitsPair.Key, CurrentTeam))
                {
                    enemyUnits = _unitsInFlag[unitsPair.Key].Count;
                    enemyTeam = unitsPair.Key;
                }
            }

            _isCaptureProcess = teamUnits == 0 && enemyUnits > 0 || enemyUnits == 0 && teamUnits > 0;
            CapturedTeam = teamUnits > enemyUnits ? CurrentTeam : enemyTeam;
        }
        
        public void SwitchTeam(Team team)
        {
            CurrentTeam = team;
            CurrentCaptureTime = 0;
            CheckCapturingState();
        }
        
        [Serializable]
        public struct Events
        {
            public UnityEvent OnCaptured;
            public UnityEvent OnStartCapturing;
            public UnityEvent OnEndCapturing;
        }
    }
    
    public enum CaptureState
    {
        Capturing,
        Draw,
        FullControl,
    }
}