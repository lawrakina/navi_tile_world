using Core.Input.Commands;
using Core.Input.Commands.Interfaces;
using Core;
using UnityEngine;

namespace Core.Input.Readers
{
    public class SwipeInputReader : TouchInputReader
    {
        [Min(0f)]
        [SerializeField] private float _swipeMinDistance = 12f;

        RotateCommand RotateLeftCommand { get; set; }
        RotateCommand RotateRightCommand { get; set; }
        ICommand StopRotateCommand { get; set; }

        protected override void Update()
        {
            base.Update();

            if (UnityEngine.Input.GetMouseButton(0))
                OnPointerClamped();

            if (UnityEngine.Input.GetMouseButtonUp(0))
                OnPointerUp();
        }

        protected void OnPointerClamped()
        {
            var xDiff = UnityEngine.Input.mousePosition.x - LastPosition.x;
            var yDiff = UnityEngine.Input.mousePosition.y - LastPosition.y;

            if (Mathf.Abs(xDiff) < TapMaxDistance && Mathf.Abs(yDiff) < TapMaxDistance)
                SendStartOrPauseCommand();
            if (IsNeedToRotate(xDiff, yDiff))
                SendRotateCommand(xDiff);

            LastPosition = UnityEngine.Input.mousePosition;
        }

        protected void SendRotateCommand(float deltaX)
        {
            var angle = Mathf.DeltaAngle(UnityEngine.Input.mousePosition.x, LastPosition.x);

            if (deltaX < -_swipeMinDistance)
                RotateRightCommand?.Execute(angle);
            else if (deltaX > _swipeMinDistance)
                RotateLeftCommand?.Execute(angle);
        }

        protected void SendStopRotateCommand()
        {
            StopRotateCommand?.Execute();
        }

        private void OnPointerUp()
        {
            SendStopRotateCommand();
        }

        private bool IsNeedToRotate(float xDiff, float yDiff)
        {
            if (InputConfig.Instance.Type == ControlType.LeftRigthSwipe && Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
                return true;

            if (InputConfig.Instance.Type == ControlType.UpDownSwipe && Mathf.Abs(yDiff) > Mathf.Abs(xDiff))
                return true;

            return false;
        }
    }
}