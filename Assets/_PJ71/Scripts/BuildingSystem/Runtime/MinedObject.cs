using System;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade.NavySpade.Modules.Utils.Timers;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade._PJ71.BuildingSystem.Runtime
{
    public class MinedObject : MonoBehaviour
    {
        [SerializeField] private MinedObjectConfig _config;
        [SerializeField] private Collider _collider;
        [SerializeField] private CapacityVisualPreset[] _damageVisualPresets;
        [SerializeField] private UnityEvent _onGathered;

        private int _currentCapacity;
        private Timer _recoverTimer;

        public bool CanGather => _currentCapacity > 0;
        
        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _currentCapacity = _config.Capacity;
            _recoverTimer = new Timer(_config.CapacityRecoveryTime);
        }

        private void Update()
        {
            if(_currentCapacity >= _config.Capacity)
                return;
            
            _recoverTimer.Update(Time.deltaTime);
            if (_recoverTimer.IsFinish())
            {
                RecoverResource();
                _recoverTimer.initTime = _config.CapacityRecoveryTime;
                _recoverTimer.Reload();
            }
        }

        public (ResourcePreset, int) GatherResource()
        {
            _currentCapacity -= 1;
            _onGathered?.Invoke();
            
            UpdateVisual();
            return (_config.ResourcePreset, 1);
        }
        
        public void ResetRecovering()
        {
            _recoverTimer.initTime = _config.FirstRecoveryTime;
            _recoverTimer.Reload();
        }

        private void RecoverResource()
        {
            _currentCapacity = Mathf.Clamp(_currentCapacity + _config.RecoveryAmount, 0, _config.Capacity);
            UpdateVisual();
        }
        
        private void UpdateVisual()
        {
            float capacityPercent = ((float) _currentCapacity / _config.Capacity) * 100;
           
            Array.ForEach(_damageVisualPresets, (v) => v.Visual.SetActive(false));
            for (int i = 0; i < _damageVisualPresets.Length; i++)
            {
                if (capacityPercent >= _damageVisualPresets[i].CapacityPercent)
                {
                    _damageVisualPresets[i].Visual.SetActive(true);
                }
            }

            _collider.isTrigger = capacityPercent == 0;
        }
        
        [Serializable]
        private struct CapacityVisualPreset
        {
            [Range(0, 100)] 
            public float CapacityPercent;
            public GameObject Visual;
        }
    }
}