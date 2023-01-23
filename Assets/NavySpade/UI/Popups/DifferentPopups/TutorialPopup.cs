using System;
using Core.Input;
using Core.UI.Popups.Abstract;
using DG.Tweening;
using EventSystem.Runtime.Core.Managers;
using NavySpade.UI.Popups.Abstract;
using UnityEngine;

namespace Core.UI.Popups
{
    public class TutorialPopup : Popup
    {
        [Serializable]
        public class ControlPrompt
        {
            public ControlType type;
            public CanvasGroup panel;
        }

        [SerializeField] private ControlPrompt[] _controlPrompts = default;
        [SerializeField] private float _animationDuration = 0.2f;

        public bool IsStarted { get; private set; }

        public event Action Started;
        public event Action Ended;

        public override void OnAwake()
        {
            DisableAllControlPrompts();
        }

        public override void OnStart()
        {
            OnOpened();
            Callbacks.Closed.AddListener(OnClosed);
        }

        private void Update()
        {
            if (IsStarted == false)
                return;

            if (UnityEngine.Input.GetMouseButtonDown(0))
                EndTutorial();
        }

        public void StartTutorial()
        {
            if (IsStarted)
                return;

            ShowControlPrompt(InputConfig.Instance.Type);
            IsStarted = true;

            Started?.Invoke();
        }

        public void EndTutorial()
        {
            if (IsStarted == false)
                return;

            Unpause();
            Close();
            IsStarted = false;

            Ended?.Invoke();
        }

        private void OnOpened()
        {
            StartTutorial();
        }

        private void OnClosed()
        {
            EndTutorial();
        }

        private void ShowControlPrompt(ControlType type)
        {
            var prompt = GetPromptByType(type);
            if (prompt == null)
                return;

            prompt.panel.alpha = 0f;
            prompt.panel.gameObject.SetActive(true);
            prompt.panel.DOFade(1f, _animationDuration).OnComplete(() =>
            {
                Pause();
            });
        }

        private void DisableAllControlPrompts()
        {
            foreach (ControlPrompt prompt in _controlPrompts)
            {
                prompt.panel.gameObject.SetActive(false);
            }
        }

        private void Pause()
        {
            EventManager.Invoke(MainEnumEvent.Pause);
        }

        private void Unpause()
        {
            EventManager.Invoke(MainEnumEvent.Pause);
        }

        private ControlPrompt GetPromptByType(ControlType type)
        {
            foreach (ControlPrompt prompt in _controlPrompts)
            {
                if (prompt.type == type)
                    return prompt;
            }

            return null;
        }
    }
}