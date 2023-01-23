using Core.Game;
using Core.Input.Commands.Interfaces;
using Core.Input.Readers.Interfaces;
using NavySpade.Core.Runtime.Game;
using UnityEngine;

namespace Core.Input.Readers
{
    public class TouchInputReader : MonoBehaviour, IInputReader
    {
        [Min(0f)]
        [SerializeField] private float _tapMaxDistance = 0.1f;
        [Min(0f)]
        [SerializeField] private float _doubleClickTime = 0.2f;

        public ICommand StartGameCommand { get; set; }
        public ICommand PauseResumeCommand { get; set; }

        protected Vector2 LastPosition { get; set; }
        protected float TapMaxDistance => _tapMaxDistance;

        private float _lastClickTime;

        protected virtual void Update()
        {
            if (GameLogic.Instance.States.IsStarted == false)
                return;

            if (UnityEngine.Input.GetMouseButtonDown(0))
                OnPointerDown();
        }

        protected virtual void OnPointerDown()
        {
            LastPosition = UnityEngine.Input.mousePosition;

            if (IsDoubleClick())
                PauseResumeCommand?.Execute();

            _lastClickTime = Time.timeSinceLevelLoad;
        }

        protected void SendStartOrPauseCommand()
        {
            PauseResumeCommand?.Execute();
        }

        private bool IsDoubleClick()
        {
            var timeSinceLastClick = Time.timeSinceLevelLoad - _lastClickTime;
            if (timeSinceLastClick <= _doubleClickTime)
                return true;

            return false;
        }
    }
}