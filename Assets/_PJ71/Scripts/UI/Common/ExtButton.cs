using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NavySpade._PJ71.UI.Common
{
    public abstract class ExtButton : MonoBehaviour
    {
        [Serializable]
        public struct Events
        {
            public UnityEvent OnCanClick;
            public UnityEvent OnCannotClick;
        }
        
        [SerializeField] private Button _button;
        [SerializeField] private Events _events;

        private bool _canPress;
        
        public bool CanPress
        {
            get => _canPress;
            set
            {
                _canPress = value;
                if (_canPress)
                {
                    _events.OnCanClick?.Invoke();
                }
                else
                {
                    _events.OnCannotClick.Invoke();
                }
            }
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnClicked);
        }

        protected abstract void OnClicked();
    }
}