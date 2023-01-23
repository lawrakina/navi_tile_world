using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NavySpade._PJ71.UI.Common
{
    [RequireComponent(typeof(Button))]
    public class ButtonHandler : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnActive;
        [SerializeField] private UnityEvent OnBlocked;

        private bool _isActive;
        
        public Button Button { get; private set; }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                if (_isActive)
                {
                    OnActive?.Invoke();
                }
                else
                {
                    OnBlocked?.Invoke();
                }
            }
        }
        
        private void Awake()
        {
            Button = GetComponent<Button>();
        }
    }
}