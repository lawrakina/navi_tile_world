using Core;
using UnityEngine;

namespace Core.Input.Providers
{
    public class HoldInputProvider : TapInputProvider
    {
        [SerializeField] private float _holdSensetivityMultiplier = 1f;

        private bool _isHeld;
        private float _pressedTime;

        private void Update()
        {
            if (IsActive() == false)
                return;

            if (UnityEngine.Input.GetMouseButtonDown(0))
                OnPonterDown();

            if (UnityEngine.Input.GetMouseButton(0))
            {
                _pressedTime += Time.deltaTime;
                if (_pressedTime < minumumTapDuration)
                    return;

                _isHeld = true;
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
                OnPointerUp();

            UpdateValue();
        }

        protected override void OnPonterDown()
        {
            IsPointerDown = true;
            _isHeld = false;
            _pressedTime = 0f;

            OnTap();
        }

        protected override void UpdateValue()
        {
            if (_isHeld == false)
                return;

            StopTap();
            OnHeld();
        }

        public override void ClearInput()
        {
            base.ClearInput();
            _isHeld = false;
        }

        protected override void OnTap()
        {
            _isHeld = false;
            base.OnTap();
        }

        private void OnHeld()
        {
            var step = InputConfig.Instance.Sensetivity * _holdSensetivityMultiplier;
            Value += step;
        }

        private void OnPointerUp()
        {
            if (_isHeld)
            {
                ClearInput();
                return;
            }

            OnTap();
        }

        // Debug:
        public float GetHoldMultiplier() => _holdSensetivityMultiplier;
        public void SetHoldMultiplier(float value) => _holdSensetivityMultiplier = value;
    }
}