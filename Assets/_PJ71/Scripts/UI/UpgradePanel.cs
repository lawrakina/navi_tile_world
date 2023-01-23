using NavySpade._PJ71.BuildingSystem;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade._PJ71.UI
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private UpgradeButton[] _buttons;
        [SerializeField] private GameObject _container;
        [SerializeField] private UnityEvent _onOpened;
        [SerializeField] private UnityEvent _onClosed;
        
        private BuildingHandler _buildingHandler;

        private void Start()
        {
            _container.SetActive(false);
        }

        public void Show(BuildingHandler building)
        {
            _buildingHandler = building;
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].Init(this, building, i);
            }
            
            _onOpened?.Invoke();
        }

        public void Hide()
        {
            _onClosed?.Invoke();
        }

        public void UpdateButtons()
        {
            foreach (var button in _buttons)
            {
                button.UpdateView();
            }
        }
    }
}