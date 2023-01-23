using NavySpade._PJ71.Level;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade._PJ71.UI
{
    public class FightButtonUI : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onCanAttack;
        [SerializeField] private UnityEvent _onCannotAttack;
        
        private LevelLogic _levelLogic;

        public void Init(LevelLogic logic)
        {
            _levelLogic = logic;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ChangeState();
        }

        public void Hide()
        {
            gameObject.SetActive(false);   
        }
        
        private void ChangeState()
        {
            if (_levelLogic.CanAttack)
            {
                _onCanAttack.Invoke();
            }
            else
            {
                _onCannotAttack.Invoke();
            }
        }
    }
}