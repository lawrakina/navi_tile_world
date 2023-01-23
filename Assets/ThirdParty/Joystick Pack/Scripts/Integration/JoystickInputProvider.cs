using System;
using Core.Input;
using Core.Input.Providers;
using NaughtyAttributes;
using UnityEngine;

namespace NavySpade
{
    public class JoystickInputProvider : InputProviderBase<Vector2>
    {
        [SerializeField] private VariableJoystick _joystick = null;
        [SerializeField] private Vector2 _directions = Vector2.one;
        [SerializeField] private bool _hasRotation;
        
        [SerializeField] 
        [ShowIf(nameof(_hasRotation))]
        private Transform _useObjectRotation;
        
        public static JoystickInputProvider Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            if (IsActive() == false)
                return;

            if (Input.GetMouseButtonDown(0))
                OnPonterDown();
            else if (Input.GetMouseButtonUp(0))
                ClearInput();

            if (IsPointerDown)
                UpdateValue();
        }

        protected override void OnPonterDown()
        {
            IsPointerDown = true;
        }

        protected override void UpdateValue()
        {
            Vector2 rawDir = (GetDirection() * InputConfig.Instance.Sensetivity).normalized;
            Vector2 dir = rawDir;
            if (_hasRotation)
            {
                Vector3 eulerRotation = _useObjectRotation.rotation.eulerAngles;
                dir = Quaternion.Euler(0, 0, eulerRotation.x) * rawDir;
            }
            
            Value = dir;
        }

        private Vector3 GetDirection()
        {
            var direction = Vector3.Scale(_joystick.Direction, _directions);
            return direction;
        }
        
        public void ResetPos()
        {
            ClearInput();
        }
    }
}
