using Core.Input.Commands;
using Core.Input.Commands.Interfaces;
using Core.Input.Readers.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.Input.Readers
{
    public class KeyInputReader : MonoBehaviour, IInputReader
    {
        [SerializeField] private KeyCode _startGameKey = KeyCode.Return;
        [SerializeField] private KeyCode _pauseResumeKey = KeyCode.Space;
        [SerializeField] private KeyCode _rotateLeftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode _rotateRightKey = KeyCode.RightArrow;
        [SerializeField] private KeyCode _quickLandKey = KeyCode.DownArrow;

        public ICommand StartGameCommand { get; set; }
        public ICommand PauseResumeCommand { get; set; }
        public RotateCommand RotateLeftCommand { get; set; }
        public RotateCommand RotateRightCommand { get; set; }
        public ICommand StopRotateCommand { get; set; }

        private KeyCode[] _keys;
        private ICommand[] _commands;

        private void Start()
        {
            Assert.IsNotNull(StartGameCommand);
            Assert.IsNotNull(PauseResumeCommand);
            Assert.IsNotNull(RotateLeftCommand);
            Assert.IsNotNull(RotateRightCommand);

            _keys = new KeyCode[] { _startGameKey, _pauseResumeKey, _rotateLeftKey, _rotateRightKey, _quickLandKey };
            _commands = new ICommand[] { StartGameCommand, PauseResumeCommand, RotateLeftCommand, RotateRightCommand };
        }

        private void Update()
        {
            for (var i = 0; i < _keys.Length; i++)
            {
                if (UnityEngine.Input.GetKeyDown(_keys[i]) && _commands[i] != null)
                    _commands[i].Execute();
            }
        }
    }
}