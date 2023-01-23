using UnityEngine;

namespace Core.Input.Providers
{
    public class SwipeInputProvider : InputProviderBase<float>
    {
        [SerializeField] private Camera _camera = default;
        [Min(0f)]
        [SerializeField] private float _maxValue = 30f;

        private Ray _startRay;

        private void Update()
        {
            if (IsActive() == false)
                return;

            if (UnityEngine.Input.GetMouseButtonDown(0))
                OnPonterDown();
            else if (UnityEngine.Input.GetMouseButtonUp(0))
                ClearInput();

            if (IsPointerDown)
                UpdateValue();
        }

        protected override void OnPonterDown()
        {
            IsPointerDown = true;
            _startRay = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
        }

        protected override void UpdateValue()
        {
            var newRay = _camera.ScreenPointToRay(GetInputPosition());
            var oldValueY = _startRay.GetPoint(5).y;
            var newValueY = newRay.GetPoint(5).y;

            var direction = Mathf.Clamp(newValueY - oldValueY, -1f, 1f);
            Value = Mathf.Clamp(direction * InputConfig.Instance.Sensetivity * 100f, -_maxValue, _maxValue);
            _startRay = newRay;
        }

        private Vector3 GetInputPosition()
        {
            var pointerPosition = UnityEngine.Input.mousePosition;
            var inputPosition = new Vector3(Screen.width / 2f, pointerPosition.y, pointerPosition.z);

            return inputPosition;
        }
    }
}
