using System;
using AYellowpaper;
using Core.Damagables;
using NavySpade.NavySpade.Modules.Damageble.Damagables.Teams;
using UnityEngine;

namespace NavySpade._PJ71.Battle.Capturing
{
    public class FlagVisual : MonoBehaviour, ITeam
    {
        [SerializeField] private InterfaceReference<ICapturable> _capturableRef;
        [SerializeField] private Transform _flagVisual;
        [SerializeField] private Vector3 _upFlagPoint;
        [SerializeField] private Vector3 _bottomFlagPoint;
        
        public event Action<Team> TeamChanged;

        public Team CurrentTeam => _capturableRef.Value.CurrentTeam;
        
        private void Start()
        {
            _capturableRef.Value = _capturableRef.Value;
            _capturableRef.Value.CaptureProgressChanged += UpdateVisual;
            UpdateVisual(_capturableRef.Value.CaptureProgress);
        }

        private void OnDestroy()
        {
            _capturableRef.Value.CaptureProgressChanged -= UpdateVisual;
        }

        private void UpdateVisual(float progress)
        {
            if (progress < 0.5f)
            {
                float lerp = Mathf.InverseLerp(0, 0.5f, progress);
                _flagVisual.localPosition = Vector3.Lerp(_upFlagPoint, _bottomFlagPoint, lerp);
                TeamChanged?.Invoke(_capturableRef.Value.CurrentTeam);
            }
            else
            {
                float lerp = Mathf.InverseLerp(0.5f, 1, progress);
                _flagVisual.localPosition = Vector3.Lerp(_bottomFlagPoint, _upFlagPoint, lerp);
                TeamChanged?.Invoke(_capturableRef.Value.CapturedTeam);
            }
        }
    }
}