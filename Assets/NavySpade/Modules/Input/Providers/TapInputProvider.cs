using System.Collections;
using UnityEngine;

namespace Core.Input.Providers
{
    public class TapInputProvider : InputProviderBase<float>
    {
        [Range(0f, 1f)]
        [SerializeField] protected float minumumTapDuration = 0.1f;
        [SerializeField] private float _tapSensetivityMultiplier = 1f;

        private Coroutine _coroutine;

        private void Update()
        {
            if (IsActive() == false)
                return;

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                OnPonterDown();
                UpdateValue();
            }
        }

        protected override void OnPonterDown()
        {
            IsPointerDown = true;
        }

        protected override void UpdateValue()
        {
            OnTap();
        }

        protected virtual void OnTap()
        {
            StopTap();
            _coroutine = StartCoroutine(IncreaseValueSmoothly());
        }

        protected void StopTap()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        protected IEnumerator IncreaseValueSmoothly()
        {
            var elapsedTime = 0f;
            var step = InputConfig.Instance.Sensetivity * _tapSensetivityMultiplier;

            while (elapsedTime < minumumTapDuration)
            {
                Value += step;
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            ClearInput();
        }

        // Debug:
        public float GetTapDuration() => minumumTapDuration;
        public void SetTapDuration(float value) => minumumTapDuration = value;
        public float GetTapMultiplier() => _tapSensetivityMultiplier;
        public void SetTapMultiplier(float value) => _tapSensetivityMultiplier = value;
    }
}